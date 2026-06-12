using System;
using System.Collections.Generic;
using System.Data;

namespace topfact.Archive.Samples
{
    internal class Program
    {
        private static topfact.Archive.ApiClient.TfaApiClient TfaClient { get; set; }

        private static topfact.Archive.Models.Security.Token TfaToken { get; set; }

        static void Main(string[] args)
        {
            // Initialize API client only one time to reuse http connections and for better performance
            TfaClient = new ApiClient.TfaApiClient(Constants.BaseUrl);

            
            // Logon for token            
            var auth = new AuthenticationSamples(TfaClient);
            var token = auth.Logon();
            if (token == null)
            {
                Console.WriteLine("Invalid credentials.");
                return;
            }
            else
            {
                // Save the token for later use
                TfaToken = token;
                Console.WriteLine($"Logged on as {token.Username}, expires on {token.ValidTo}.");
            }


            // Get user archives
            var archivesSamples = new ArchivesSamples(TfaClient, TfaToken);          
            var archives = archivesSamples.GetArchives();
            foreach (var a in archives)
            {
                Console.WriteLine($"Archive: {a.Name}");
            }


            // Documents
            var documentsSamples = new DocumentsSamples(TfaClient, TfaToken);
            var document = documentsSamples.AddDocument();
            if (document == null)
            {
                Console.WriteLine("Failed to add document.");
            }
            else
            {
                Console.WriteLine($"Document added with ID: {document.DocId}");

                // If we have a document ID, we can also get the document details again to verify that it was added correctly
                var newDocument = documentsSamples.GetDocument(document.DocId);
            }


            // Searching
            var searchSamples = new SearchSamples(TfaClient, TfaToken);
            searchSamples.SearchDocuments();
            searchSamples.SearchFulltext();
            searchSamples.SearchDocumentsProfessional();



            Console.WriteLine("Press a key to exit.");
            Console.ReadLine();
        }
    }
}
