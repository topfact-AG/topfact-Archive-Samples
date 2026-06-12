using System;
using System.Text;
using System.Threading.Tasks;

namespace topfact.Archive.Samples
{
    /// <summary>
    /// The AuthenticationSamples class provides methods to demonstrate how to authenticate with the API using a TfaApiClient.
    /// It includes both synchronous and asynchronous methods for logging in and retrieving a security token,
    /// which can be used for subsequent API calls that require authentication.
    /// </summary>
    public class AuthenticationSamples
    {
        private readonly topfact.Archive.ApiClient.TfaApiClient _ApiClient;

        /// <summary>
        /// Initializes a new instance of the AuthenticationSamples class with the specified TfaApiClient.
        /// This client is used to perform authentication operations such as logging in and retrieving security tokens.
        /// </summary>
        /// <param name="apiClient">The TfaApiClient instance used for authentication operations.</param>
        public AuthenticationSamples(topfact.Archive.ApiClient.TfaApiClient apiClient)
        {
            _ApiClient = apiClient;
        }

        /// <summary>
        /// Logs in to the API using the provided username and password, and retrieves a security token.
        /// The token is printed to the console and returned by the method.
        /// </summary>
        /// <returns>The security token if the login is successful; otherwise, null.</returns>
        public topfact.Archive.Models.Security.Token Logon()
        {
            var token = _ApiClient.Logon(Constants.Username, Constants.Password);

            if (token == null)
                return null;

            Console.WriteLine($"Token: {token.AccessKey}");
            return token;
        }

        /// <summary>
        /// Asynchronous version of Logon method.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the security token.</returns>
        public async Task<topfact.Archive.Models.Security.Token> LogonAsync()
        {
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Constants.Username}:{Constants.Password}"));
            var token = await _ApiClient.LogonAsync(base64);
            if (token == null)
                return null;

            Console.WriteLine($"Token: {token.AccessKey}");
            return token;
        }
    }
}
