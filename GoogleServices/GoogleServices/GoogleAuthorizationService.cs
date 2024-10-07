using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GoogleServices.GoogleServices
{
    public abstract class GoogleAuthorizationService
    {
        private const string RegistryKeyPath = @"SOFTWARE\Gradient\GoogleOAuth";
        private readonly string userId = "user";
        private ClientSecrets clientSecrets;
        private IAuthorizationCodeFlow authorizationFlow { get; set; }
        private List<string> Scopes { get; set; }

        /// <summary>
        /// Check if the user credential has the required scopes
        /// </summary>
        /// <param name="credential"></param>
        /// <param name="requiredScopes"></param>
        /// <returns></returns>
        private bool AreRequiredScopesPresent(UserCredential credential, IEnumerable<string> requiredScopes)
        {
            if (credential == null || credential.Token == null)
            {
                return false; // No valid credential
            }

            var tokenScopes = GetScopes(credential);
            return requiredScopes.All(scope => tokenScopes.Contains(scope));
        }

        /// <summary>
        /// Retrieves the list of scopes associated with the given UserCredential.
        /// </summary>
        /// <param name="userCredential">The UserCredential object.</param>
        /// <returns>A list of scopes if available; otherwise, an empty list.</returns>
        private List<string> GetScopes(UserCredential userCredential)
        {
            if (userCredential == null || userCredential.Token == null)
            {
                return new List<string>(); // Return an empty list if the credential is null
            }

            // Split the scopes string into a list
            var tokenScopes = userCredential.Token.Scope?.Split(' ') ?? Array.Empty<string>();
            return tokenScopes.ToList(); // Return the scopes as a List<string>
        }

        /// <summary>
        /// Retrieves the list of scopes associated with the given UserCredential.
        /// </summary>
        /// <param name="tokenResponse">The UserCredential object.</param>
        /// <returns>A list of scopes if available; otherwise, an empty list.</returns>
        private List<string> GetScopes(TokenResponse tokenResponse)
        {
            if (tokenResponse == null)
            {
                return new List<string>(); // Return an empty list if the credential is null
            }

            // Split the scopes string into a list
            var tokenScopes = tokenResponse.Scope?.Split(' ') ?? Array.Empty<string>();
            return tokenScopes.ToList(); // Return the scopes as a List<string>
        }

        /// <summary>
        /// Check if the token is expired
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        private bool IsTokenExpired(UserCredential credential)
        {
            return credential.Token.IsStale; // Check if the token is stale or expired
        }

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

        /// <summary>
        /// Load token from the registry
        /// </summary>
        /// <returns>A TokenResponse object if found and valid; otherwise, null.</returns>
        private TokenResponse LoadTokenFromRegistry()
        {
            // Open the specified registry key
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                if (key != null)
                {
                    // Retrieve the token JSON string from the registry
                    string tokenJson = key.GetValue("Token") as string; // Get the token from the registry

                    if (!string.IsNullOrEmpty(tokenJson))
                    {
                        try
                        {
                            // Attempt to deserialize the JSON string into a TokenResponse object
                            return Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(tokenJson);
                        }
                        catch (JsonException)
                        {
                            // If deserialization fails, assume the token is corrupt and return null
                            Console.WriteLine("Failed to deserialize token from registry. Token may be corrupt.");
                        }
                        catch (Exception ex)
                        {
                            // Handle any other unexpected exceptions and log them
                            Console.WriteLine($"Unexpected error while loading token from registry: {ex.Message}");
                        }
                    }
                }
            }

            // Return null if the token was not found or could not be deserialized
            return null;
        }

        /// <summary>
        /// Request user authorization if no valid credential is available
        /// </summary>
        /// <param name="scopes"></param>
        /// <returns></returns>
        private async Task RequestUserAuthorization(List<string> scopes)
        {
            var codeReceiver = new LocalServerCodeReceiver();
            authorizationFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = scopes
            });
            var authorizationCodeUrl = authorizationFlow.CreateAuthorizationCodeRequest(codeReceiver.RedirectUri);

            Console.WriteLine($"Open the following URL in your browser to authorize the application: {authorizationCodeUrl.Build()}");

            var authorizationCode = await codeReceiver.ReceiveCodeAsync(authorizationCodeUrl, CancellationToken.None);

            // Exchange the authorization code for a new token
            TokenResponse newTokenResponse = await authorizationFlow.ExchangeCodeForTokenAsync(
                userId,
                authorizationCode.Code,
                codeReceiver.RedirectUri,
                CancellationToken.None);

            // Create the UserCredential object using the new token response
            UserCredential = new UserCredential(authorizationFlow, userId, newTokenResponse);

            // Save the new token to the registry for future use
            SaveTokenToRegistry(newTokenResponse);
        }

        /// <summary>
        /// Save token to the registry
        /// </summary>
        /// <param name="tokenResponse"></param>
        private void SaveTokenToRegistry(TokenResponse tokenResponse)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
            {
                if (key != null)
                {
                    string tokenJson = Newtonsoft.Json.JsonConvert.SerializeObject(tokenResponse);
                    key.SetValue("Token", tokenJson); // Save the token to the registry
                }
            }
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
                    Scopes = GetScopes(tokenResponse)
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

            // Ensure user is authorized before setting up external services
            AuthorizeAsync().GetAwaiter().GetResult();

            // Setup external services now that we have the UserCredential available
            SetupExternalServices();
        }

        protected UserCredential UserCredential { get; private set; }

        /// <summary>
        /// Handles the authorization process, obtaining a token and managing user consent if necessary.
        /// </summary>
        protected async Task AuthorizeAsync()
        {
            // Load the token from the registry
            var existingTokenResponse = LoadTokenFromRegistry();
            if (existingTokenResponse != null)
            {
                UserCredential = TryCreateUserCredential(existingTokenResponse);

                // If UserCredential is valid
                if (UserCredential != null)
                {
                    // Check if the token is not expired
                    if (!IsTokenExpired(UserCredential))
                    {
                        // Check if the loaded token has the required scopes
                        if (AreRequiredScopesPresent(UserCredential, Scopes))
                        {
                            return; // Already authorized with valid token and required scopes
                        }
                        else
                        {
                            // If the token is valid but lacks scopes, get the existing scopes
                            var existingScopes = UserCredential.Token.Scope?.Split(' ') ?? Array.Empty<string>();

                            // Combine existing scopes with the required scopes
                            Scopes = existingScopes.Union(Scopes).Distinct().ToList();

                            // Clear the credential to force reauthorization since scopes are insufficient
                            UserCredential = null;
                        }
                    }
                    else
                    {
                        // If token is expired, clear the credential
                        UserCredential = null;
                    }
                }
            }

            // Proceed to request user authorization if no valid credential is available
            await RequestUserAuthorization(Scopes);
        }

        public abstract void SetupExternalServices();
    }
}