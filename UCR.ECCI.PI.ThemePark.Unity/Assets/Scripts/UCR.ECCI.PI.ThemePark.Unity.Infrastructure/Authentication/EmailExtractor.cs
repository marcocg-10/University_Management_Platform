using System;
using System.Text;
using UnityEngine;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication
{
    /// <summary>
    /// Extracts the email address from the stored OAuth token in player preferences.
    /// </summary>
    /// <remarks>This method retrieves the OAuth token stored in the Unity PlayerPrefs under the key
    /// "oauth_tokens", decodes the JWT payload, and extracts the email address from the claims. If no email address is
    /// found or the token is invalid, an empty string is returned.</remarks>
    public static class EmailExtractor
    {
        public static string GetEmailFromPrefs()
        {
            string rawJson = PlayerPrefs.GetString("oauth_tokens", "");
            if (string.IsNullOrEmpty(rawJson)) return "";

            var tokens = JsonUtility.FromJson<OAuthTokenResponse>(rawJson);
            if (string.IsNullOrEmpty(tokens.id_token)) return "";

            string payloadJson = DecodeJwtPayload(tokens.id_token);
            var claims = JsonUtility.FromJson<JwtClaims>(payloadJson);

            return claims.emails != null && claims.emails.Length > 0 ? claims.emails[0] : "";
        }

        /// <summary>
        /// Decodes the payload of a JSON Web Token (JWT) and returns it as a UTF-8 encoded string.
        /// </summary>
        /// <remarks>This method assumes the payload is Base64Url-encoded and converts it to a
        /// Base64-encoded string before decoding.  Padding is added to the Base64 string if necessary to ensure proper
        /// decoding.</remarks>
        /// <param name="jwt">The JSON Web Token (JWT) string to decode. Must be a valid JWT with at least two parts separated by periods.</param>
        /// <returns>The decoded payload of the JWT as a UTF-8 encoded string, or <see langword="null"/> if the input JWT is
        /// invalid or does not contain a payload.</returns>
        private static string DecodeJwtPayload(string jwt)
        {
            var parts = jwt.Split('.');
            if (parts.Length < 2) return null;

            string payload = parts[1].Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            var bytes = Convert.FromBase64String(payload);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Represents the claims contained in a JSON Web Token (JWT), including email addresses and custom extensions.
        /// </summary>
        /// <remarks>This class provides fields for storing standard and custom claims extracted from a
        /// JWT.  The fields include email addresses and custom extensions such as identification and full
        /// name.</remarks>
        [Serializable]
        private class JwtClaims
        {
            public string[] emails;
            public string extension_Identification;
            public string extension_FullName;
        }
    }
}