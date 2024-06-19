using GoogleServices.GoogleServices;

namespace GoogleServices.GoogleAuthentication
{
    public static class GoogleOAuthAuthenticatorHelperExtensions
    {
        public static async Task<GoogleOAuthAuthenticatorHelper> SetupAsync(this GoogleOAuthAuthenticatorHelper googleOAuthAuthenticator,
            params GoogleWebAuthorizationBrokeredScopedService[] services)
        {
            await googleOAuthAuthenticator.SetupAuthAsync();
            foreach (var service in services)
            {
                service.ClientSecrets = googleOAuthAuthenticator.ClientSecrets;
                service.SetupToken();
                service.SetupExternalServices();
            }
            return googleOAuthAuthenticator;
        }
    }
}