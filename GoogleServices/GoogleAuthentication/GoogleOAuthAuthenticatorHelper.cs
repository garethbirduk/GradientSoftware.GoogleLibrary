using Google.Apis.Auth.OAuth2;

using GoogleLibrary.OAuth;
using GoogleServices.GoogleServices;
using GoogleServices.OAuth;
using Microsoft.Extensions.Configuration;

namespace GoogleServices.GoogleAuthentication
{
    public class GoogleOAuthAuthenticatorHelper : IOAuthAuthenticatorHelper, IDisposable
    {
        private List<GoogleWebAuthorizationBrokeredScopedService> Services { get; } = new List<GoogleWebAuthorizationBrokeredScopedService>();

        protected IConfigurationRoot Configuration;

        /// <summary>
        /// Create using CreateAsync
        /// </summary>
        public GoogleOAuthAuthenticatorHelper()
        {
        }

        public IOAuthAuthenticator Authenticator { get; protected set; }

        public string ClientId { get; protected set; }

        public string ClientSecret { get; protected set; }

        public ClientSecrets ClientSecrets { get; protected set; }

        public static async Task CreateAsync(params GoogleWebAuthorizationBrokeredScopedService[] services)
        {
            var authenticator = new GoogleOAuthAuthenticatorHelper();
            await authenticator.SetupAuthAsync();
            foreach (var service in services)
            {
                service.ClientSecrets = authenticator.ClientSecrets;
                service.SetupToken();
                service.SetupExternalServices();
            }
        }

        public void Dispose()
        {
            ClientSecrets = null;
            foreach (var service in Services)
                service.ClientSecrets = null;
        }

        public async Task SetupAuthAsync()
        {
            Configuration = new ConfigurationBuilder()
               .AddUserSecrets<GoogleOAuthAuthenticatorHelper>()
               .Build();

            ClientId = Configuration["Authentication:Google:ClientId"] ?? "";
            ClientSecret = Configuration["Authentication:Google:ClientSecret"] ?? "";

            ClientSecrets = new ClientSecrets()
            {
                ClientId = ClientId,
                ClientSecret = ClientSecret,
            };

            Authenticator = new GoogleOAuthAuthenticator();
            await Authenticator.AuthenticateForLibrary(ClientId, ClientSecret);
        }
    }
}