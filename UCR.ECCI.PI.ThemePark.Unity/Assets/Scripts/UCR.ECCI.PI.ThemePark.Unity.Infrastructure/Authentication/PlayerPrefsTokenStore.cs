// File: Infrastructure/OAuth/PlayerPrefsTokenStore.cs
using System.Threading.Tasks;
using UnityEngine;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication
{
    public sealed class PlayerPrefsTokenStore : ITokenStore
    {
        private const string Key = "oauth_tokens";

        public async Task SaveAsync(OAuthTokenResponse t)
        {
            var json = JsonUtility.ToJson(t);

            // Force back onto the Unity main thread before touching PlayerPrefs
            await Awaiters.UnityMainThread;

            PlayerPrefs.SetString(Key, json);
            PlayerPrefs.Save();
        }

        public Task<OAuthTokenResponse?> LoadAsync()
        {
            var json = PlayerPrefs.GetString(Key, "");
            if (string.IsNullOrEmpty(json)) return Task.FromResult<OAuthTokenResponse?>(null);
            var obj = JsonUtility.FromJson<OAuthTokenResponse>(json);
            return Task.FromResult<OAuthTokenResponse?>(obj);
        }

        public async Task ClearAsync()
        {
            await Awaiters.UnityMainThread;

            PlayerPrefs.DeleteKey(Key);
            PlayerPrefs.Save();
        }
    }
}
