using GoogleLibrary.GoogleAuthentication;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GoogleLibrary.GoogleServices
{
    public class GoogleUserInformationService : GoogleOAuthAuthenticatedService
    {
        public GoogleUserInformationService(GoogleOAuthAuthenticatedResponse googleOAuthAuthenticatedResponse) : base(googleOAuthAuthenticatedResponse)
        {
        }

        public string UserInformation { get; set; }

        public async Task GetUserInformationAsync()
        {
            // builds the request
            string userinfoRequestUri = "https://www.googleapis.com/oauth2/v3/userinfo";

            // sends the request
            HttpWebRequest userinfoRequest = (HttpWebRequest)WebRequest.Create(userinfoRequestUri);
            userinfoRequest.Method = "GET";
            userinfoRequest.Headers.Add($"Authorization: {GoogleOAuthAuthenticatedResponse.TokenType} {GoogleOAuthAuthenticatedResponse.AccessToken}");
            userinfoRequest.ContentType = "application/x-www-form-urlencoded";
            userinfoRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            // gets the response
            WebResponse userinfoResponse = await userinfoRequest.GetResponseAsync();
            using (StreamReader userinfoResponseReader = new StreamReader(userinfoResponse.GetResponseStream()))
            {
                // reads response body
                UserInformation = await userinfoResponseReader.ReadToEndAsync();
            }
        }
    }
}