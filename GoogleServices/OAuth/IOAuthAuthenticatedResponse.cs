namespace GoogleServices.OAuth
{
    public interface IOAuthAuthenticatedResponse
    {
        string AccessToken { get; set; }
        int ExpiresIn { get; set; }
        DateTime Expiry { get; set; }
        string IdToken { get; set; }
        string RefreshToken { get; set; }
        string Scope { get; set; }
        string TokenType { get; set; }
    }
}