// File: Infrastructure/OAuth/Pkce.cs
using System;
using System.Security.Cryptography;
using System.Text;

namespace UCR.ECCI.PI.ThemePark.Unity.Infrastructure.Authentication
{
    public static class Pkce
    {
        public static string CreateVerifier(int length = 64)
        {
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            return Base64UrlNoPad(Convert.ToBase64String(bytes));
        }

        public static string CreateChallenge(string verifier)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(verifier));
            return Base64UrlNoPad(Convert.ToBase64String(hash));
        }

        private static string Base64UrlNoPad(string s)
            => s.Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
}
