using GoogleLibrary.OAuth;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GoogleLibrary.GoogleAuthentication
{
    public class GoogleOAuthAuthenticator : IOAuthAuthenticator
    {
        private const string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";

        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static string Base64UrlEncodeNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");

            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }

        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        /// <param name="length">Input length (nb. output will be longer)</param>
        /// <returns></returns>
        private static string GenerateRandomDataBase64url(uint length)
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);
            return Base64UrlEncodeNoPadding(bytes);
        }

        private static string GetChromePath()
        {
            string lPath = null;
            try
            {
                var lTmp = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", "", null);
                if (lTmp != null)
                    lPath = lTmp.ToString();
                else
                {
                    lTmp = Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", "", null);
                    if (lTmp != null)
                        lPath = lTmp.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Some exception. Dunno. Probably Chrome install path not found!" + ex);
            }

            if (lPath == null)
            {
                throw new Exception("Chrome install path not found!");
            }

            return lPath;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        // ref http://stackoverflow.com/a/3978040
        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        /// <summary>
        /// Appends the given string to the on-screen log, and the debug console.
        /// </summary>
        /// <param name="output">String to be logged</param>
        private static void Log(string output)
        {
            Console.WriteLine(output);
        }

        private static async Task RequestUserInfoAsync(string accessToken)
        {
            Log("Making API Call to Userinfo...");

            // builds the request
            var userinfoRequestUri = "https://www.googleapis.com/oauth2/v3/userinfo";

            // sends the request
            HttpWebRequest userinfoRequest = (HttpWebRequest)WebRequest.Create(userinfoRequestUri);
            userinfoRequest.Method = "GET";
            userinfoRequest.Headers.Add(string.Format("Authorization: Bearer {0}", accessToken));
            userinfoRequest.ContentType = "application/x-www-form-urlencoded";
            userinfoRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            // gets the response
            WebResponse userinfoResponse = await userinfoRequest.GetResponseAsync();
            using var userinfoResponseReader = new StreamReader(userinfoResponse.GetResponseStream());
            // reads response body
            string userinfoResponseText = await userinfoResponseReader.ReadToEndAsync();
            Log(userinfoResponseText);
        }

        // Hack to bring the Console window to front.
        // ref: http://stackoverflow.com/a/12066376
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Returns the SHA256 hash of the input string, which is assumed to be ASCII.
        /// </summary>
        private static byte[] Sha256Ascii(string text)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            return SHA256.HashData(bytes);
        }

        private void BringConsoleToFront()
        {
            SetForegroundWindow(GetConsoleWindow());
        }

        private async Task DoOAuthAsync(string clientId, string clientSecret, bool isConsole)
        {
            // Generates state and PKCE values.
            string state = GenerateRandomDataBase64url(32);
            string codeVerifier = GenerateRandomDataBase64url(32);
            string codeChallenge = Base64UrlEncodeNoPadding(Sha256Ascii(codeVerifier));
            const string codeChallengeMethod = "S256";

            // Creates a redirect URI using an available port on the loopback address.
            string redirectUri = $"http://{IPAddress.Loopback}:{GetRandomUnusedPort()}/";
            Log("redirect URI: " + redirectUri);

            // Creates an HttpListener to listen for requests on that redirect URI.
            var http = new HttpListener();
            http.Prefixes.Add(redirectUri);
            Log("Listening..");
            http.Start();

            // Creates the OAuth 2.0 authorization request.
            string authorizationRequest = string.Format("{0}?response_type=code&scope=openid%20profile&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}",
                AuthorizationEndpoint,
                Uri.EscapeDataString(redirectUri),
                clientId,
                state,
                codeChallenge,
                codeChallengeMethod);

            // Opens request in the browser.
            Process.Start(GetChromePath(), authorizationRequest);

            // Waits for the OAuth authorization response.
            var context = await http.GetContextAsync();

            // Sends an HTTP response to the browser.
            var response = context.Response;
            string responseString = "<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Please return to the app.</body></html>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            await responseOutput.WriteAsync(buffer, 0, buffer.Length);
            responseOutput.Close();
            http.Stop();
            Log("HTTP server stopped.");

            // Checks for errors.
            var error = context.Request.QueryString.Get("error");
            if (error is not null)
            {
                Log($"OAuth authorization error: {error}.");
                return;
            }
            if (context.Request.QueryString.Get("code") is null
                || context.Request.QueryString.Get("state") is null)
            {
                Log($"Malformed authorization response. {context.Request.QueryString}");
                return;
            }

            // extracts the code
            var code = context.Request.QueryString.Get("code");
            var incomingState = context.Request.QueryString.Get("state");

            // Compares the receieved state to the expected value, to ensure that this app made the
            // request which resulted in authorization.
            if (incomingState != state)
            {
                Log($"Received request with invalid state ({incomingState})");
                return;
            }
            Log("Authorization code: " + code);

            // Starts the code exchange at the Token Endpoint.
            await ExchangeCodeForTokensAsync(code, codeVerifier, redirectUri, clientId, clientSecret);
        }

        private async Task ExchangeCodeForTokensAsync(string code, string codeVerifier, string redirectUri, string clientId, string clientSecret)
        {
            Log("Exchanging code for tokens...");

            // builds the request
            string tokenRequestUri = "https://www.googleapis.com/oauth2/v4/token";
            string tokenRequestBody = string.Format("code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&client_secret={4}&scope=&grant_type=authorization_code",
                code,
                Uri.EscapeDataString(redirectUri),
                clientId,
                codeVerifier,
                clientSecret
                );

            // sends the request
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(tokenRequestUri);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            byte[] tokenRequestBodyBytes = Encoding.ASCII.GetBytes(tokenRequestBody);
            tokenRequest.ContentLength = tokenRequestBodyBytes.Length;
            using (Stream requestStream = tokenRequest.GetRequestStream())
            {
                await requestStream.WriteAsync(tokenRequestBodyBytes, 0, tokenRequestBodyBytes.Length);
            }

            try
            {
                // gets the response
                WebResponse tokenResponse = await tokenRequest.GetResponseAsync();
                using var reader = new StreamReader(tokenResponse.GetResponseStream());
                // reads response body
                string responseText = await reader.ReadToEndAsync();
                Console.WriteLine(responseText);

                // converts to dictionary
                var headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                GoogleOAuthAuthenticatedResponse = new GoogleOAuthAuthenticatedResponse(headers);
                await SaveTokenToFileAsync();

                string accessToken = GoogleOAuthAuthenticatedResponse.AccessToken;
                await RequestUserInfoAsync(accessToken);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        Log("HTTP: " + response.StatusCode);
                        using var reader = new StreamReader(response.GetResponseStream());
                        // reads response body
                        string responseText = await reader.ReadToEndAsync();
                        Log(responseText);
                    }
                }
            }
            return;
        }

        private async Task LoadTokenFromFileAsync()
        {
            try
            {
                var serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                using var sr = new StreamReader(CredentialsFilepath);
                using var reader = new JsonTextReader(sr);
                GoogleOAuthAuthenticatedResponse = serializer.Deserialize<GoogleOAuthAuthenticatedResponse>(reader);
            }
            catch
            {
                GoogleOAuthAuthenticatedResponse = null;
            }
            await Task.CompletedTask;
        }

        private async Task SaveTokenToFileAsync()
        {
            var serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using (var sw = new StreamWriter(CredentialsFilepath))
            using (var writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, GoogleOAuthAuthenticatedResponse);
            }
            await Task.CompletedTask;
        }

        public string CredentialsFilepath { get; private set; } = Path.Combine("c:\\", "temp", "credentials.json");

        public GoogleOAuthAuthenticatedResponse GoogleOAuthAuthenticatedResponse { get; set; }

        public async Task AuthenticateForConsole(string clientId, string clientSecret)
        {
            await DoOAuthAsync(clientId, clientSecret, true);
        }

        public async Task AuthenticateForLibrary(string clientId, string clientSecret)
        {
            await LoadTokenFromFileAsync();
            if (GoogleOAuthAuthenticatedResponse == null)
                await DoOAuthAsync(clientId, clientSecret, false);

            if (IsExpired(100))
                await DoOAuthAsync(clientId, clientSecret, false);
        }

        public bool IsExpired(int delta = 10)
        {
            var now = DateTime.UtcNow;
            var expires = GoogleOAuthAuthenticatedResponse.Expiry.AddSeconds(-delta);
            var isExpired = expires < now;
            return isExpired;
        }
    }
}