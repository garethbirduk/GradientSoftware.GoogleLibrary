//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Auth.OAuth2.Responses;
//using Newtonsoft.Json;

//namespace GoogleServices.GoogleServices
//{
//    public abstract class GoogleWebAuthorizationBrokeredScopedService
//    {
//        private static string TokenFilepath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Google.Apis.Auth", "Google.Apis.Auth.OAuth2.Responses.TokenResponse-user");

//        private static TokenResponse GetExistingToken()
//        {
//            if (!File.Exists(TokenFilepath))
//                return null;
//            var serializer = new JsonSerializer
//            {
//                NullValueHandling = NullValueHandling.Ignore
//            };

//            using var sr = new StreamReader(TokenFilepath);
//            using JsonReader reader = new JsonTextReader(sr);
//            return serializer.Deserialize<TokenResponse>(reader);
//        }

//        private static bool TokenScopesAreSufficient(TokenResponse token, string[] RequiredScopes)
//        {
//            var existing = token.Scope.Split(" ");
//            return RequiredScopes.All(existing.Contains);
//        }

//        public ClientSecrets ClientSecrets { get; set; }

//        public UserCredential UserCredential { get; private set; }

//        public abstract void SetupExternalServices();

//        public virtual void SetupToken()
//        {
//            var token = GetExistingToken();
//            if (token != null && !TokenScopesAreSufficient(token, Scopes.ToArray()))
//                File.Delete(TokenFilepath);

//            UserCredential = GoogleWebAuthorizationBroker.GetUserCredentialAsync(ClientSecrets,
//                Scopes,
//                "user",
//                CancellationToken.None).Result;
//        }
//    }
//}