using Google.Apis.Auth.OAuth2;

namespace GoogleServices.Auth
{
    /// <summary>
    /// Resolves a credential via Google's Application Default Credentials search:
    /// GOOGLE_APPLICATION_CREDENTIALS env var, then gcloud's well-known file, then GCE metadata.
    /// For local dev: run `gcloud auth application-default login` once.
    /// </summary>
    public class AdcAuthProvider : IGoogleAuthProvider
    {
        public GoogleCredential GetCredential(IEnumerable<string> scopes)
        {
            var credential = GoogleCredential.GetApplicationDefault();
            if (credential.IsCreateScopedRequired)
                credential = credential.CreateScoped(scopes);
            return credential;
        }
    }
}
