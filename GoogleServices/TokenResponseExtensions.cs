using Google.Apis.Auth.OAuth2.Responses;

namespace GoogleServices.GoogleServices
{
    public static class TokenResponseExtensions
    {
        /// <summary>
        /// Retrieves the list of scopes associated with the given UserCredential.
        /// </summary>
        /// <param name="userCredential">The UserCredential object.</param>
        /// <returns>A list of scopes if available; otherwise, an empty list.</returns>
        public static List<string> Scopes(this TokenResponse tokenResponse)
        {
            if (tokenResponse == null)
            {
                return new List<string>(); // Return an empty list if the tokenResponse is null
            }

            // Split the scopes string into a list
            var tokenScopes = tokenResponse.Scope?.Split(' ') ?? Array.Empty<string>();
            return tokenScopes.ToList(); // Return the scopes as a List<string>
        }
    }
}