using System;
using System.Collections.Generic;

namespace GoogleLibrary.GoogleAuthentication
{
    public class GoogleOAuthAuthenticatedResponse
    {
        public GoogleOAuthAuthenticatedResponse()
        {
        }

        public GoogleOAuthAuthenticatedResponse(Dictionary<string, string> headers)
        {
            Headers = headers;

            AccessToken = Headers["access_token"];

            ExpiresIn = int.Parse(Headers["expires_in"]);

            IdToken = Headers["id_token"];

            RefreshToken = Headers["refresh_token"];

            Scope = Headers["scope"];

            TokenType = Headers["token_type"];

            var delta = 100;
            Expiry = DateTime.UtcNow.AddSeconds(ExpiresIn - delta);
        }

        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }

        public DateTime Expiry { get; set; } = DateTime.UtcNow;

        public string IdToken { get; set; }

        public string RefreshToken { get; set; }

        public string Scope { get; set; }

        public string TokenType { get; set; }

        private Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();
    }
}