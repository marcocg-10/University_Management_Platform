// File: Infrastructure/OAuth/OAuthTokenResponse.cs
using System;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication
{
    [Serializable]
    public sealed class OAuthTokenResponse
    {
        public string access_token;
        public string token_type;     // "Bearer"
        public int    expires_in;     // seconds
        public string refresh_token;
        public string id_token;
        public string scope;

        // calculated locally
        public long   obtained_unix;  // seconds since epoch when stored
    }
}
