using System;
using System.Collections.Generic;
using System.Data;
using topfact.Archive.Models.Security;

namespace topfact.Archive.Samples
{
    /// <summary>
    /// The SearchSamples class provides methods to demonstrate how to perform search operations using the TfaApiClient and a security token.
    /// </summary>
    public class SearchSamples
    {
        private readonly ApiClient.TfaApiClient _ApiClient;

        private readonly Token _Token;

        /// <summary>
        /// Initializes a new instance of the SearchSamples class with the specified TfaApiClient and security token.
        /// This client is used to perform authentication operations such as logging in and retrieving security tokens.
        /// </summary>
        /// <param name="apiClient">The TfaApiClient instance used for authentication operations.</param>
        /// <param name="token">The security token used for authentication.</param>
        public SearchSamples(topfact.Archive.ApiClient.TfaApiClient apiClient, Token token)
        {
            _ApiClient = apiClient;
            _Token = token;
        }

        /// <summary>
        /// Searches for documents in the archive based on specified search fields. The method constructs a search request with the given search criteria,
        /// sends it to the API client, and processes the response to determine if the search was successful. It then outputs the number of documents found
        /// or an error message accordingly.
        /// </summary>
        public void SearchDocuments()
        {
            var req = new topfact.Archive.Models.Request.SearchRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.Searchfields = new List<Models.SearchField>();
            req.Searchfields.Add(new Models.SearchField("belegart", "excel"));
            //req.Searchfields.Add(new Models.SearchField("belegdatum", DateTime.Now.AddYears(-1), DateTime.Now));

            // Optional for best performance
            //req.Columns = new List<string>() { "gpname", "gpnummer", "belegart" };
            req.DocumentCountOnly = false;
            req.Token = _Token;

            var res = _ApiClient.SearchDocuments(req);

            if (res?.StatusCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Search found {res.DocumentsCount} documents.");
                Console.ResetColor();

                DataTable dt = topfact.Archive.ApiClient.DataTableHelper.JsonToDatatable(res.Documents);


                foreach (DataRow row in dt.Rows)
                {
                    int docid = Convert.ToInt32(row["tf_docid"]);

                    Console.WriteLine($"DocID: {docid}");
                    Console.WriteLine("-----------------------------------------");
                    foreach (DataColumn col in dt.Columns)
                    {
                        Console.WriteLine($"{col.ColumnName}: {row[col.ColumnName]}");
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Searches for documents in the archive using a full-text search query. The method constructs a search request 
        /// with the specified full-text search string and sends it to the API client. It then processes the response to determine 
        /// if the search was successful and outputs the number of documents found or an error message accordingly.
        /// </summary>
        /// <returns>True if the search was successful; otherwise, false.</returns>
        public bool SearchFulltext()
        {
            var fulltextSearch = "rechnung 4711";

            var req = new topfact.Archive.Models.Request.SearchRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.Searchfields = new List<Models.SearchField>();
            req.Searchfields.Add(new Models.SearchField("fulltext", fulltextSearch));
            req.Token = _Token;

            var res = _ApiClient.SearchDocuments(req);

            if (res?.StatusCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Search found {res.DocumentsCount} documents.");
                Console.ResetColor();

                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error");
                Console.ResetColor();

                return false;
            }
        }

        /// <summary>
        /// Searches for documents in the archive using a professional search query. The method constructs a search request with a complex
        /// query and sends it to the API client. It then processes the response to determine if the search was successful and outputs
        /// the number of documents found or an error message accordingly.
        /// </summary>
        /// <returns>True if the search was successful; otherwise, false.</returns>
        public bool SearchDocumentsProfessional()
        {
            var req = new topfact.Archive.Models.Request.ProfessionalSearchRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            //req.Columns = new List<string>() { "gpname", "gpnummer", "belegart" };
            req.ProfessionalQuery = "[gpname] LIKE '%mytga%' AND ([belegnummer] = '4711-0815' OR [belegnummer] = '0001-2023')";
            req.SortQuery = "belegart DESC";
            req.Token = _Token;

            var res = _ApiClient.SearchDocumentsProfessional(req);

            if (res?.StatusCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Search found {res.DocumentsCount} documents.");
                Console.ResetColor();

                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error");
                Console.ResetColor();

                return false;
            }
        }
    }
}
