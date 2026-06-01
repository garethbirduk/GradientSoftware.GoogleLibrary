using Google.Apis.Auth.OAuth2;

namespace GoogleServices.Auth
{
    /// <summary>
    /// Supplies a <see cref="GoogleCredential"/> for the requested scopes.
    /// Implementations decide where the credential comes from (ADC, service account, mock, etc).
    /// </summary>
    public interface IGoogleAuthProvider
    {
        GoogleCredential GetCredential(IEnumerable<string> scopes);
    }
}
