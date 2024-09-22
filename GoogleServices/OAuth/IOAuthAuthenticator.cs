namespace GoogleServices.OAuth
{
    public interface IOAuthAuthenticator
    {
        string CredentialsFilepath { get; }
        IOAuthAuthenticatedResponse OAuthAuthenticatedResponse { get; set; }

        Task AuthenticateForConsole(string clientId, string clientSecret);

        Task AuthenticateForLibrary(string clientId, string clientSecret);

        bool IsExpired(int delta = 10);
    }
}