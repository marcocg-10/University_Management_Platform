// File: Infrastructure/Core/KiotaAccessTokenProvider.cs
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions.Authentication;
using UnityEngine;
using UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication
{
    public interface IAccessTokenSource
    {
        Task<string?> GetTokenAsync(CancellationToken ct = default);
    }

    public sealed class OAuthAccessTokenSource : IAccessTokenSource
    {
        private readonly IOAuth2Service _oauth;
        public OAuthAccessTokenSource(IOAuth2Service oauth) => _oauth = oauth;
        public Task<string?> GetTokenAsync(CancellationToken ct = default) => _oauth.GetValidAccessTokenAsync(ct);
    }

    internal sealed class KiotaAccessTokenProvider : IAccessTokenProvider
    {
        private readonly IAccessTokenSource _tokenSource;
        public AllowedHostsValidator AllowedHostsValidator { get; } = new AllowedHostsValidator();

        public KiotaAccessTokenProvider(IAccessTokenSource tokenSource)
        {
            _tokenSource = tokenSource;
            // Allow localhost (and optionally production host)
            AllowedHostsValidator.AllowedHosts = new HashSet<string>
            {
                "localhost"
                // "themepark-api.yourdomain.tld"
            };
        }

        public async Task<string> GetAuthorizationTokenAsync(
            Uri uri,
            Dictionary<string, object>? additionalAuthenticationContext = null,
            CancellationToken cancellationToken = default)
        {
            var token = await _tokenSource.GetTokenAsync(cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(token))
            {
                Debug.LogWarning("[Kiota] No access token available. Request will be unauthenticated.");
                return string.Empty;
            }

            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = token.Substring("Bearer ".Length).Trim();

            return token;
        }
    }
}
