using System.Threading.Tasks;
using GoogleLibrary.GoogleAuthentication;

namespace GoogleLibrary.OAuth
{
    public interface IOAuthAuthenticator
    {
        string CredentialsFilepath { get; }
        GoogleOAuthAuthenticatedResponse GoogleOAuthAuthenticatedResponse { get; set; }

        Task AuthenticateForConsole(string clientId, string clientSecret);

        Task AuthenticateForLibrary(string clientId, string clientSecret);

        bool IsExpired(int delta = 10);
    }
}