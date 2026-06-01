using Google.Apis.Services;
using GoogleServices.Auth;

namespace GoogleServices.GoogleServices
{
    /// <summary>
    /// Base class that hands a credential to derived Google service wrappers.
    /// Credential resolution is delegated to an <see cref="IGoogleAuthProvider"/> —
    /// the static <see cref="DefaultAuthProvider"/> by default, or a per-instance
    /// override passed to <see cref="Initialize(IGoogleAuthProvider)"/>.
    /// </summary>
    public abstract class GoogleAuthorizationService
    {
        /// <summary>
        /// Process-wide default. Tests / hosts swap this to redirect every service
        /// (e.g. to a service-account provider for CI). Defaults to <see cref="AdcAuthProvider"/>.
        /// </summary>
        public static IGoogleAuthProvider DefaultAuthProvider { get; set; } = new AdcAuthProvider();

        protected List<string> Scopes { get; }

        protected GoogleAuthorizationService(IEnumerable<string> requiredScopes)
        {
            Scopes = requiredScopes.Distinct().ToList();
        }

        public virtual void Initialize() => Initialize(DefaultAuthProvider);

        /// <summary>
        /// Initialize the service using an explicit provider — used by tests that need a
        /// narrowly-scoped credential per service instance (e.g. scope-correctness tests).
        /// </summary>
        public virtual void Initialize(IGoogleAuthProvider authProvider)
        {
            var credential = authProvider.GetCredential(Scopes);
            SetupExternalServices(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleLibrary",
            });
        }

        public abstract void SetupExternalServices(BaseClientService.Initializer initializer);
    }
}
