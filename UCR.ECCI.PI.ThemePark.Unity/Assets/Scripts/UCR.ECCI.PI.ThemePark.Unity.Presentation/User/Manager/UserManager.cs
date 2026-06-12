using Codice.Client.BaseCommands.FastExport;
using System.Threading.Tasks;
using UCR.ECCI.PI.ThemePark.Unity.Application.Authentication.Services;
using UCR.ECCI.PI.ThemePark.Unity.Application.User.Services;
using UCR.ECCI.PI.ThemePark.Unity.Application.User.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Unity.Domain.User.ValueObjects;
using UnityEngine;
using Zenject;

namespace UCR.ECCI.PI.ThemePark.Unity.Presentation.User.Manager
{
    /// <summary>
    /// Responsible for User Operations
    /// </summary>
    public class UserManager : MonoBehaviour
    {
        /// <summary>
        /// Service used to fetch building data from the application layer.
        /// Injected via Zenject dependency injection.
        /// </summary>
        [Inject]
        private IUserService _userService;

        [Inject] private IAuthReady _authReady;

        [Inject] private IOAuth2Service _oauth;

        /// <summary>
        /// Unity lifecycle method called on the first frame.
        /// This to check services are available
        /// </summary>
        private async void Start()
        {
            // Ensure DI provided dependencies are present
            if (_oauth == null || _userService == null)
            {
                Debug.LogError("Zenject failed to inject dependencies into UserManager. Ensure a Context exists and bindings for IOAuth2Service and IUserService are configured.");
                return;
            }

            // Acquire a valid access token, triggering sign-in if required.
            var token = await _oauth.GetValidAccessTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                var ok = await _oauth.SignInAsync();
                if (!ok)
                {
                    Debug.LogError("Sign-in failed; aborting data load.");
                    return;
                }

                token = await _oauth.GetValidAccessTokenAsync();

                if (string.IsNullOrEmpty(token))
                {
                    Debug.LogError("[Login] No valid access token after sign-in. Check redirect_uri, scopes, and token exchange.");
                    return;
                }
            }
        }

        /// <summary>
        /// Unity lifecycle method called once per frame.
        /// Currently unused but available for future per-frame updates.
        /// </summary>
        private void Update()
        {
            // Reserved for per-frame update logic if needed
        }

        /// <summary>
        /// Asynchronously Save the Avatar Id, returns true if sucess, false if error
        /// </summary>
        public async Task SaveAvatarIdAync(string avatarId)
        {
            if (_userService == null)
            {
                Debug.LogError("_userService was not injected.");
            }

            // Validate avatarId
            AvatarId? validAvatarId = null;

            var isValid = AvatarId.TryCreate(avatarId, out validAvatarId, out var error);

            if (!isValid)
            {
                Debug.LogError($"Invalid avatarId provided: {error}");
            }
          
            try
            {
                await _userService.SaveAvatarIdAsync(validAvatarId);
            }
            catch
            {
                Debug.LogError("Error saving avatar");
            }
        }

        /// <summary>
        /// Asynchronously Retrieve the Avatar Id, returns true if success
        /// </summary>
        public async Task<(string value, bool isSucesss)> GetAvatarIdAsync()
        {
            if (_userService == null)
            {
                Debug.LogError("_userService was not injected.");
                return (null, false);
            }

            try
            {
                var avatarId = await _userService.GetAvatarIdAsync();
                return (avatarId.Value, true);
            }
            catch
            {
                Debug.LogError("Error retrieving avatar");
                return (null, false);
            }
        }
    }
}