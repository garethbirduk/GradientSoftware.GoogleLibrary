using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;

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