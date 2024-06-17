using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Newtonsoft.Json;
using PostSharp.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace GoogleServices
{
    public abstract class GoogleWebAuthorizationBrokeredScopedService
    {
        private static string TokenFilepath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Google.Apis.Auth", "Google.Apis.Auth.OAuth2.Responses.TokenResponse-user");

        private static TokenResponse GetExistingToken()
        {
            if (!File.Exists(TokenFilepath))
                return null;
            var serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using var sr = new StreamReader(TokenFilepath);
            using JsonReader reader = new JsonTextReader(sr);
            return serializer.Deserialize<TokenResponse>(reader);
        }

        private static bool TokenScopesAreSufficient(TokenResponse token, string[] requiredScopes)
        {
            var existing = token.Scope.Split(" ");
            return requiredScopes.All(existing.Contains);
        }

        public ClientSecrets ClientSecrets { get; set; }

        public abstract IEnumerable<string> Scopes { get; }

        public UserCredential UserCredential { get; private set; }

        public abstract void SetupExternalServices();

        public virtual void SetupToken()
        {
            var token = GetExistingToken();
            if (token != null && !TokenScopesAreSufficient(token, Scopes.ToArray()))
                File.Delete(TokenFilepath);

            UserCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(ClientSecrets,
                Scopes,
                "user",
                CancellationToken.None).Result;
        }
    }
}