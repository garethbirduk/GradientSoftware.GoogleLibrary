using GoogleLibrary.GoogleAuthentication;

namespace GoogleLibrary.GoogleServices
{
    public abstract class GoogleOAuthAuthenticatedService
    {
        public GoogleOAuthAuthenticatedService(GoogleOAuthAuthenticatedResponse googleOAuthAuthenticatedResponse)
        {
            GoogleOAuthAuthenticatedResponse = googleOAuthAuthenticatedResponse;
        }

        public GoogleOAuthAuthenticatedResponse GoogleOAuthAuthenticatedResponse { get; set; }
    }
}