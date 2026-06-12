using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication
{
    /// <summary>
    /// OAuth2 / OpenID Connect client for Azure AD B2C with PKCE + loopback redirect.
    /// Intended to be bound as a Zenject singleton (AsSingle()).
    /// </summary>
    public sealed class OAuth2Service : IOAuth2Service
    {
        private readonly OAuthConfig _cfg;
        private readonly ITokenStore _store;
        private readonly HttpClient _http;

        // Concurrency state for interactive sign-in
        private readonly SemaphoreSlim _signInLock = new(1, 1);
        private Task<bool> _signInTask;
        private HttpListener _listener;

        public OAuth2Service(OAuthConfig cfg, ITokenStore store)
        {
            _cfg   = cfg;
            _store = store;
            _http  = new HttpClient();
        }

        /// <summary>
        /// Starts an interactive sign-in if needed, or awaits the one already in progress.
        /// Returns true on success (tokens stored), false on failure.
        /// </summary>
        public Task<bool> SignInAsync(CancellationToken ct = default)
        {
            return GetOrCreateSignInTask(ct);
        }

        // Ensure only one interactive sign-in runs at a time, and share that Task with all callers.
        private async Task<bool> GetOrCreateSignInTask(CancellationToken ct)
        {
            Task<bool> task;

            await _signInLock.WaitAsync(ct);
            try
            {
                if (_signInTask == null || _signInTask.IsCompleted)
                {
                    Debug.Log("[Auth] Starting new interactive sign-in.");
                    _signInTask = SignInInternalAsync(ct);
                }
                else
                {
                    Debug.Log("[Auth] Sign-in already in progress; awaiting existing task.");
                }

                task = _signInTask;
            }
            finally
            {
                _signInLock.Release();
            }

            // Do not hold the lock while awaiting
            return await task;
        }

        // Actual sign-in flow: PKCE + loopback + /token exchange + token store.
        private async Task<bool> SignInInternalAsync(CancellationToken ct)
        {
            try
            {
                // --- 1) Create PKCE pair ---
                var verifier  = Pkce.CreateVerifier();
                var challenge = Pkce.CreateChallenge(verifier);

                // --- 2) Start loopback listener on redirect URI ---
                var redirectTask = StartLoopbackListenerOnceAsync(_cfg.RedirectUri, ct);

                // Common scope string used for both /authorize and /token
                var scopeParam    = string.Join(" ", _cfg.Scopes);
                var scopeParamEsc = Uri.EscapeDataString(scopeParam);

                // --- 3) Build authorization URL ---
                var authUrl =
                    $"{_cfg.AuthorizationEndpoint}" +
                    $"?response_type=code" +
                    $"&client_id={Uri.EscapeDataString(_cfg.ClientId)}" +
                    $"&redirect_uri={Uri.EscapeDataString(_cfg.RedirectUri)}" +
                    $"&scope={scopeParamEsc}" +
                    $"&code_challenge={challenge}" +
                    $"&code_challenge_method=S256";

                Debug.Log($"[OAuth] RedirectUri used: {_cfg.RedirectUri}");
                Debug.Log($"[OAuth] Authorize URL: {authUrl}");

                UnityEngine.Application.OpenURL(authUrl);

                // --- 4) Wait for authorization code from loopback listener ---
                var code = await redirectTask.ConfigureAwait(false);
                if (string.IsNullOrEmpty(code))
                {
                    Debug.LogError("[OAuth] No authorization code received.");
                    return false;
                }

                // --- 5) Exchange authorization code for tokens ---
                Debug.Log($"[OAuth] /token scopes: {scopeParam}");
                Debug.Log($"[OAuth] /token redirect_uri: {_cfg.RedirectUri}");

                var form = new StringBuilder();
                form.Append("grant_type=authorization_code");
                form.Append("&code=").Append(Uri.EscapeDataString(code));
                form.Append("&client_id=").Append(Uri.EscapeDataString(_cfg.ClientId));
                form.Append("&redirect_uri=").Append(Uri.EscapeDataString(_cfg.RedirectUri));
                form.Append("&code_verifier=").Append(Uri.EscapeDataString(verifier));
                form.Append("&scope=").Append(Uri.EscapeDataString(scopeParam));

                var content = new StringContent(form.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
                var resp    = await _http.PostAsync(_cfg.TokenEndpoint, content, ct).ConfigureAwait(false);
                var json    = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

                Debug.Log($"[OAuth] Token response JSON: {json}");

                if (!resp.IsSuccessStatusCode)
                {
                    Debug.LogError($"[OAuth] Token exchange failed: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{json}");
                    return false;
                }

                var tokens = JsonUtility.FromJson<OAuthTokenResponse>(json);

                if (string.IsNullOrEmpty(tokens.access_token))
                {
                    Debug.LogError("[OAuth] No access_token in token response. " +
                                   "Ensure your /token request includes the API scope plus 'openid offline_access'.");
                    return false;
                }

                tokens.obtained_unix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                await _store.SaveAsync(tokens).ConfigureAwait(false);

                Debug.Log("[OAuth] Sign-in successful; tokens stored.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[OAuth] SignInInternalAsync failed: {ex}");
                return false;
            }
            finally
            {
                StopListener();

                // Optionally clear the shared task once everything is done
                await _signInLock.WaitAsync();
                try
                {
                    if (_signInTask != null && _signInTask.IsCompleted)
                        _signInTask = null;
                }
                finally
                {
                    _signInLock.Release();
                }
            }
        }

        /// <summary>
        /// Returns a valid access token if available, refreshing if necessary.
        /// Returns null if no valid token can be obtained.
        /// </summary>
        public async Task<string?> GetValidAccessTokenAsync(CancellationToken ct = default)
        {
            var t = await _store.LoadAsync().ConfigureAwait(false);
            if (t == null)
                return null;

            var now       = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var expiresAt = t.obtained_unix + t.expires_in - 60; // refresh one minute early

            // Still valid
            if (now < expiresAt && !string.IsNullOrEmpty(t.access_token))
                return t.access_token;

            // No refresh token available
            if (string.IsNullOrEmpty(t.refresh_token))
                return null;

            // --- Refresh token flow ---
            var scopeParam = string.Join(" ", _cfg.Scopes);

            var form = new StringBuilder();
            form.Append("grant_type=refresh_token");
            form.Append("&refresh_token=").Append(Uri.EscapeDataString(t.refresh_token));
            form.Append("&client_id=").Append(Uri.EscapeDataString(_cfg.ClientId));
            form.Append("&scope=").Append(Uri.EscapeDataString(scopeParam));

            var content = new StringContent(form.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            var resp    = await _http.PostAsync(_cfg.TokenEndpoint, content, ct).ConfigureAwait(false);
            var json    = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode)
            {
                Debug.LogError($"[OAuth] Refresh failed: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{json}");
                await _store.ClearAsync().ConfigureAwait(false);
                return null;
            }

            var newTokens = JsonUtility.FromJson<OAuthTokenResponse>(json);

            // Some providers don't return a new refresh token; keep the old one in that case.
            if (string.IsNullOrEmpty(newTokens.refresh_token))
                newTokens.refresh_token = t.refresh_token;

            newTokens.obtained_unix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (string.IsNullOrEmpty(newTokens.access_token))
            {
                Debug.LogError("[OAuth] Refresh succeeded but no access_token returned. " +
                               "Verify requested scopes include the API scope.");
                return null;
            }

            await _store.SaveAsync(newTokens).ConfigureAwait(false);
            return newTokens.access_token;
        }

        public Task SignOutAsync()
        {
            return _store.ClearAsync();
        }

        // --- Loopback listener helpers ---

        /// <summary>
        /// Starts a single HttpListener on the given redirectUri and returns a Task
        /// that completes with the "code" query parameter from the first request.
        /// </summary>
        private Task<string> StartLoopbackListenerOnceAsync(string redirectUri, CancellationToken ct)
        {
            if (_listener != null && _listener.IsListening)
                return Task.FromException<string>(
                    new InvalidOperationException("Listener already running. Ensure SignInAsync is not started twice in parallel."));

            var tcs = new TaskCompletionSource<string>();

            _listener = new HttpListener();
            var prefix = redirectUri.EndsWith("/") ? redirectUri : redirectUri + "/";
            _listener.Prefixes.Add(prefix);

            try
            {
                _listener.Start();
            }
            catch (HttpListenerException hlex)
            {
                _listener = null;
                throw new InvalidOperationException(
                    $"Another listener is already bound to {prefix}. Ensure SignInAsync runs only once at a time.", hlex);
            }

            _ = Task.Run(async () =>
            {
                try
                {
                    var ctx   = await _listener.GetContextAsync().ConfigureAwait(false);
                    var code  = ctx.Request.QueryString["code"];
                    var error = ctx.Request.QueryString["error"];

                    var html = string.IsNullOrEmpty(code)
                        ? "<html><body>Login failed. You can close this window.</body></html>"
                        : "<html><body>Login successful. You can close this window.</body></html>";

                    var bytes = Encoding.UTF8.GetBytes(html);
                    ctx.Response.ContentType     = "text/html; charset=utf-8";
                    ctx.Response.ContentLength64 = bytes.Length;
                    await ctx.Response.OutputStream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
                    ctx.Response.OutputStream.Close();
                    ctx.Response.Close();

                    if (!string.IsNullOrEmpty(error))
                        tcs.TrySetException(new InvalidOperationException($"OAuth error: {error}"));
                    else if (!string.IsNullOrEmpty(code))
                        tcs.TrySetResult(code);
                    else
                        tcs.TrySetException(new InvalidOperationException("Missing 'code' on redirect."));
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
                finally
                {
                    StopListener();
                }
            }, ct);

            return tcs.Task;
        }

        private void StopListener()
        {
            var l = _listener;
            _listener = null;
            if (l == null) return;

            try { l.Stop(); }  catch { }
            try { l.Close(); } catch { }
        }
    }
}
