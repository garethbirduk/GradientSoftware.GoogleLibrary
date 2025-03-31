using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Gradient.Utils.Windows;
using Microsoft.Extensions.Configuration;

namespace GoogleServices.GoogleServices
{
    public abstract class GoogleAuthorizationService
    {
        private const string TokenReponseRegistryKeyName = @"Token";

        private const string TokenResponseRegistryKeyPath = @"SOFTWARE\Gradient\GoogleOAuth";

        private readonly string userId = "user";

        private ClientSecrets clientSecrets;

        private IAuthorizationCodeFlow authorizationFlow { get; set; }

        private List<string> Scopes { get; set; }

        /// <summary>
        /// Loads the client secrets from user secrets or environment variables using the configuration.
        /// </summary>
        private ClientSecrets LoadClientSecretsFromConfiguration()
        {
            var configuration = new ConfigurationBuilder()
               .AddUserSecrets<GoogleAuthorizationService>()
               .Build();

            return new ClientSecrets
            {
                ClientId = configuration["Authentication:Google:ClientId"] ?? "",
                ClientSecret = configuration["Authentication:Google:ClientSecret"] ?? ""
            };
        }

        private TokenResponse LoadTokenFromRegistry()
        {
            return RegistryHelper.LoadObject<TokenResponse>(TokenResponseRegistryKeyPath, TokenReponseRegistryKeyName);
        }

        /// <summary>
        /// Requests user authorization if no valid token response is available.
        /// </summary>
        /// <param name="scopes">The list of scopes required for authorization.</param>
        /// <returns>A UserCredential object with the authorized token.</returns>
        private async Task<UserCredential> RequestUserAuthorization(List<string> scopes)
        {
            try
            {
                // Initialize the authorization flow with client secrets and required scopes
                var codeReceiver = new LocalServerCodeReceiver();
                authorizationFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets,
                    Scopes = scopes
                });

                // Generate the authorization code request URL
                var authorizationCodeUrl = authorizationFlow.CreateAuthorizationCodeRequest(codeReceiver.RedirectUri);

                // Prompt the user to open the URL in their browser
                Console.WriteLine($"Open the following URL in your browser to authorize the application: {authorizationCodeUrl.Build()}");

                // Receive the authorization code from the user's browser interaction
                var authorizationCode = await codeReceiver.ReceiveCodeAsync(authorizationCodeUrl, CancellationToken.None);

                // Exchange the authorization code for a new token
                TokenResponse newTokenResponse = await authorizationFlow.ExchangeCodeForTokenAsync(
                    userId,
                    authorizationCode.Code,
                    codeReceiver.RedirectUri,
                    CancellationToken.None);

                // Create the UserCredential object using the new token response
                var userCredential = new UserCredential(authorizationFlow, userId, newTokenResponse);

                // Save the new token to the registry for future use
                SaveTokenToRegistry(newTokenResponse);

                return userCredential;
            }
            catch (Exception ex)
            {
                // Log the exception or handle errors in the authorization process
                Console.WriteLine($"An error occurred during the authorization process: {ex.Message}");
                throw;
            }
        }

        private void SaveTokenToRegistry(TokenResponse tokenResponse)
        {
            RegistryHelper.SaveObject(TokenResponseRegistryKeyPath, tokenResponse, TokenReponseRegistryKeyName);
        }

        /// <summary>
        /// Create UserCredential from token response
        /// </summary>
        /// <param name="tokenResponse"></param>
        /// <returns></returns>
        private UserCredential TryCreateUserCredential(TokenResponse tokenResponse)
        {
            try
            {
                authorizationFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets,
                    Scopes = tokenResponse.Scopes(),
                });
                return new UserCredential(authorizationFlow, userId, tokenResponse);
            }
            catch
            {
                return null; // Return null if mapping fails
            }
        }

        /// <summary>
        /// Constructor that initializes the Google Authorization service with client secrets from configuration.
        /// </summary>
        /// <param name="requiredScopes">A list of scopes required by the service, can be empty.</param>
        protected GoogleAuthorizationService(IEnumerable<string> requiredScopes)
        {
            Scopes = requiredScopes.Distinct().ToList();
            clientSecrets = LoadClientSecretsFromConfiguration();
        }

        protected UserCredential UserCredential { get; private set; }

        /// <summary>
        /// Gets the UserCredential either handling the authorization process or retrieving from store; it refreshes if necessary.
        /// </summary>
        protected async Task<UserCredential> GetUserCredentialAsync()
        {
            var existingTokenResponse = LoadTokenFromRegistry();
            if (existingTokenResponse == null && false)
            {
                return await RequestUserAuthorization(Scopes);
            }

            var userCredential = TryCreateUserCredential(existingTokenResponse);
            if (userCredential != null && userCredential.Token.HasScopes(Scopes))
            {
                if (userCredential.IsExpired() && !string.IsNullOrEmpty(existingTokenResponse.RefreshToken))
                {
                    try
                    {
                        var refreshedToken = await authorizationFlow.RefreshTokenAsync(userId, existingTokenResponse.RefreshToken, CancellationToken.None);
                        SaveTokenToRegistry(refreshedToken); // Save the refreshed token
                        return new UserCredential(authorizationFlow, userId, refreshedToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to refresh token: {ex.Message}. Reverting to new authorization.");
                    }
                }
                else
                {
                    return userCredential; // Return the valid and non-expired credential
                }
            }

            if (userCredential != null)
            {
                Scopes = userCredential.Scopes().Union(Scopes).Distinct().ToList();
            }
            return await RequestUserAuthorization(Scopes);
        }

        public BaseClientService.Initializer BaseClientServiceInitializer { get; set; }

        public virtual void Initialize()
        {
            SetupExternalServices(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetUserCredentialAsync().GetAwaiter().GetResult(),
                ApplicationName = "Google Calender API v3",
            });
        }

        public abstract void SetupExternalServices(BaseClientService.Initializer initializer);
    }
}