using Google.Apis.Auth.OAuth2;
using GoogleServices.OAuth;

namespace GoogleLibrary.OAuth
{
    public interface IOAuthAuthenticatorHelper
    {
        IOAuthAuthenticator Authenticator { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        ClientSecrets ClientSecrets { get; }

        Task SetupAuthAsync();
    }
}