//using GoogleServices.GoogleServices;

//namespace GoogleServices.GoogleAuthentication
//{
//    public static class GoogleOAuthAuthenticatorHelperExtensions
//    {
//        public static async Task<GoogleOAuthAuthenticatorHelper> SetupAsync<T>(this GoogleOAuthAuthenticatorHelper googleOAuthAuthenticator,
//            params GoogleWebAuthorizationBrokeredScopedService[] services)
//            where T : class
//        {
//            await googleOAuthAuthenticator.SetupAuthAsync<T>();
//            foreach (var service in services)
//            {
//                service.ClientSecrets = googleOAuthAuthenticator.ClientSecrets;
//                service.SetupToken();
//                service.SetupExternalServices();
//            }
//            return googleOAuthAuthenticator;
//        }
//    }
//}