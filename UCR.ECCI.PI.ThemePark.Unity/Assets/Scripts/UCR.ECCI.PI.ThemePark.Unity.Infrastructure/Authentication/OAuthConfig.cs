// File: Infrastructure/OAuth/OAuthConfig.cs
using UnityEngine;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication
{
    [CreateAssetMenu(menuName = "Auth/OAuthConfig")]
    public sealed class OAuthConfig : ScriptableObject
    {
        // TODO: fill with your actual values
        public string AuthorizationEndpoint = "https://ucrpiis.b2clogin.com/ucrpiis.onmicrosoft.com/B2C_1_ThemeParkUCR-register/oauth2/v2.0/authorize";
        public string TokenEndpoint         = "https://ucrpiis.b2clogin.com/ucrpiis.onmicrosoft.com/B2C_1_ThemeParkUCR-register/oauth2/v2.0/token";
        public string ClientId              = "2aa08c9f-b5cf-42f6-a242-c5a3fe1073b5";
        public string[] Scopes              = new[] { "https://ucrpiis.onmicrosoft.com/0b75128b-2eb3-44e5-8af7-d1728ba64d94/App.Read", "email", "openid", "offline_access" };

        // Loopback redirect for desktop flows (must be registered in your OAuth app)
        public string RedirectUri           = "http://localhost:5010/callback";
    }
}
