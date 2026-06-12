using System.Collections.Generic;
using System.Threading.Tasks;
using topfact.Archive.Models.Security;

namespace topfact.Archive.Samples
{
    /// <summary>
    /// The ArchivesSamples class provides methods to demonstrate how to retrieve a list of archives from the API using a TfaApiClient and a security token.
    /// </summary>
    public class ArchivesSamples
    {
        private readonly ApiClient.TfaApiClient _ApiClient;

        private readonly Token _Token;

        /// <summary>
        /// Initializes a new instance of the ArchivesSamples class with the specified TfaApiClient and security token.
        /// This client is used to perform authentication operations such as logging in and retrieving security tokens.
        /// </summary>
        /// <param name="apiClient">The TfaApiClient instance used for authentication operations.</param>
        /// <param name="token">The security token used for authentication.</param>
        public ArchivesSamples(topfact.Archive.ApiClient.TfaApiClient apiClient, Token token)
        {
            _ApiClient = apiClient;
            _Token = token;
        }

        /// <summary>
        /// Retrieves a list of archives from the API using the provided security token for authentication.
        /// </summary>
        /// <returns>A list of archives retrieved from the API.</returns>
        public List<topfact.Archive.Models.Archive> GetArchives()
        {
            var archives = _ApiClient.GetArchives(_Token);
            return archives;
        }

        /// <summary>
        /// Asynchronous version of GetArchives method. Retrieves a list of archives from the API using the provided security token for authentication.
        /// </summary>
        /// <returns></returns>
        public async Task<List<topfact.Archive.Models.Archive>> GetArchivesAsync()
        {
            var archives = await _ApiClient.GetArchivesAsync(_Token);
            return archives;
        }
    }
}
