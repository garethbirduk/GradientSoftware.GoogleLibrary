using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;

namespace GoogleServices.GoogleServices
{
    public static class UserCredentialExtensions
    {
        /// <summary>
        /// Checks if the TokenResponse has the required scopes.
        /// </summary>
        public static bool HasScopes(this TokenResponse tokenResponse, IEnumerable<string> requiredScopes)
        {
            return tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.Scope) &&
                   requiredScopes.All(x => tokenResponse.Scope.Split(' ').Contains(x));
        }

        /// <summary>
        /// Determines if the UserCredential's token is expired.
        /// </summary>
        public static bool IsExpired(this UserCredential userCredential)
        {
            return userCredential?.Token?.IsStale ?? true;
        }

        /// <summary>
        /// Retrieves the list of scopes associated with the given UserCredential.
        /// </summary>
        public static List<string> Scopes(this UserCredential userCredential)
        {
            return userCredential?.Token?.Scopes() ?? new List<string>();
        }
    }
}