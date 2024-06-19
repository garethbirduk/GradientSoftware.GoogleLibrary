using GoogleServices.OAuth;

namespace GoogleServices.GoogleServices
{
    public abstract class GoogleOAuthAuthenticatedService
    {
        public GoogleOAuthAuthenticatedService(IOAuthAuthenticatedResponse oAuthAuthenticatedResponse)
        {
            GoogleOAuthAuthenticatedResponse = oAuthAuthenticatedResponse;
        }

        public IOAuthAuthenticatedResponse GoogleOAuthAuthenticatedResponse { get; set; }
    }
}