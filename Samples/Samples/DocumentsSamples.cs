using System;
using System.Collections.Generic;
using System.Diagnostics;
using topfact.Archive.Models.Security;

namespace topfact.Archive.Samples
{
    /// <summary>
    /// The DocumentsSamples class provides methods to demonstrate how to retrieve a list of documents from the API using a TfaApiClient and a security token.
    /// </summary>
    public class DocumentsSamples
    {
        private readonly ApiClient.TfaApiClient _ApiClient;

        private readonly Token _Token;

        /// <summary>
        /// Initializes a new instance of the DocumentsSamples class with the specified TfaApiClient and security token.
        /// This client is used to perform authentication operations such as logging in and retrieving security tokens.
        /// </summary>
        /// <param name="apiClient">The TfaApiClient instance used for authentication operations.</param>
        /// <param name="token">The security token used for authentication.</param>
        public DocumentsSamples(topfact.Archive.ApiClient.TfaApiClient apiClient, Token token)
        {
            _ApiClient = apiClient;
            _Token = token;
        }

        /// <summary>
        /// Adds a new document to the archive using the TfaApiClient and the provided security token.
        /// The method creates a request object with the necessary information, including archive fields and files, and sends it to the API.
        /// It then checks the response for success and prints the result to the console.
        /// </summary>
        /// <returns></returns>
        public topfact.Archive.Models.Response.AddDocumentResponse AddDocument()
        {
            var req = new topfact.Archive.Models.Request.AddDocumentRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.ArchiveFields = new List<Models.ArchiveField>
            {
                new Models.ArchiveField() { Fieldname = "belegart", Value = "Rechnung" },
                new Models.ArchiveField() { Fieldname = "belegdatum", Value = DateTime.Now.Date },
                new Models.ArchiveField() { Fieldname = "belegbetragbrutto", Value = 11.99 }
            };

            var fi = new System.IO.FileInfo("C:\\Temp\\document.pdf");
            req.ArchiveFiles = new List<Models.ArchiveFile>
            {
                new Models.ArchiveFile() { Filename = fi.Name, Filebinary = System.IO.File.ReadAllBytes(fi.FullName) },

                // If you want to add more files, you can create additional ArchiveFile objects and add them to the list. For example:
                //new Models.ArchiveFile() { Filename = fi.Name, Filebinary = System.IO.File.ReadAllBytes(fi.FullName) }
            };

            req.Token = _Token;

            var res = _ApiClient.AddDocument(req);

            if (res?.StatusCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"New docid {res.DocId} created.");
                Console.ResetColor();
                return res;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error");
                Console.ResetColor();
                return null;
            }
        }

        /// <summary>
        /// Retrieves a document from the archive using the TfaApiClient and the provided security token.
        /// The method creates a request object with the necessary information, including the document ID, and sends it to the API.
        /// It then checks the response for success and returns the retrieved document if successful, or null if there was an error.
        /// </summary>
        /// <param name="docId">The ID of the document to retrieve.</param>
        /// <param name="withFiles">Indicates whether to include the document files in the response.</param>
        /// <returns>The retrieved document if successful, or null if there was an error.</returns>
        public topfact.Archive.Models.ArchiveDocument GetDocument(int docId, bool withFiles = false)
        {
            var req = new topfact.Archive.Models.Request.GetDocumentRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.DocId = docId;
            req.IncludeFiles = withFiles;
            req.Token = _Token;

            var res = _ApiClient.GetDocument(req);

            if (res?.StatusCode == 0)            
                return res.Document;            
            else            
                return null;            
        }

        public bool ChangeDocument(int docid)
        {
            var req = new topfact.Archive.Models.Request.ChangeDocumentRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.DocId = docid;
            req.Indexdata = new List<Models.ArchiveField>
            {
                new Models.ArchiveField() { Fieldname = "mandant", Value = "myTGA" },
                new Models.ArchiveField() { Fieldname = "gpname", Value = "myTGA GmbH" },
                new Models.ArchiveField() { Fieldname = "gpnummer", Value = "4711" },
                new Models.ArchiveField() { Fieldname = "belegnummer", Value = "0001-2023" },
                new Models.ArchiveField() { Fieldname = "belegbetragbrutto", Value = 99.99 }
            };

            req.Token = _Token;

            var res = _ApiClient.ChangeDocument(req);

            if (res?.StatusCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Document changed successfully.");
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

        public bool DownloadDocument()
        {
            var req = new topfact.Archive.Models.Request.DownloadDocumentRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.DocId = 6882;
            req.AsPdf = false;
            req.WithAnnotations = false;
            req.Token = _Token;

            var res = _ApiClient.DownloadDocument(req);

            if (res?.StatusCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Download document was successfully.");
                Console.ResetColor();

                foreach (var file in res.Files)
                {
                    var path = System.IO.Path.Combine("C:\\Temp\\", file.Filename);
                    System.IO.File.WriteAllBytes(path, file.Filebinary);

                    Process.Start(path);
                }

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
