using Google.Apis.Auth.OAuth2;

namespace GoogleServices.Auth
{
    /// <summary>
    /// Resolves a credential from a service-account JSON key file, narrowed to the requested scopes.
    /// Used by CI and by tests that need to mint a credential with an exact scope set
    /// (e.g. scope-correctness tests).
    /// </summary>
    public class ServiceAccountAuthProvider : IGoogleAuthProvider
    {
        private readonly string keyPath;

        public ServiceAccountAuthProvider(string keyPath)
        {
            this.keyPath = keyPath;
        }

        public GoogleCredential GetCredential(IEnumerable<string> scopes)
        {
            return GoogleCredential.FromFile(keyPath).CreateScoped(scopes);
        }
    }
}
