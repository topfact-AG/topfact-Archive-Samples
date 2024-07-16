using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace topfact.Archive.Samples
{
    internal class Program
    {
        private static topfact.Archive.ApiClient.TfaApiClient TfaClient { get; set; }

        private static topfact.Archive.Models.Security.Token TfaToken { get; set; }

        static void Main(string[] args)
        {
            TfaClient = new ApiClient.TfaApiClient(Constants.BaseUrl);

            // Logon for token
            var token = Logon();

            if (token == null)
            {
                Console.WriteLine("Invalid credentials.");
                return;
            }
            else
            {
                TfaToken = token;
                Console.WriteLine($"Logged on as {token.Username}, expires on {token.ValidTo}.");
            }

            // Get user archives
            //var archives = GetArchives();
            //foreach (var a in archives)
            //{
            //    Console.WriteLine($"Archive: {a.Name}");
            //}

            //SearchDocuments();

            //SearchFulltext("container");

            //SearchDocumentsProfessional();

            //AddDocument(client, out int docid);

            //ChangeDocument(client, docid);

            DownloadDocument();

            //GetDocument();

            GetFiles();

            //ShowInWebviewer(docid);

            //ShowInWebviewerWithSearch();

            Console.WriteLine("Press a key to exit.");
            Console.ReadLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static topfact.Archive.Models.Security.Token Logon()
        {
            try
            {
                var token = TfaClient.Logon(Constants.Username, Constants.Password);
                return token;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static void SearchDocuments()
        {
            var req = new topfact.Archive.Models.Request.SearchRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.Searchfields = new List<Models.SearchField>();
            req.Searchfields.Add(new Models.SearchField("belegart", "Rechnung"));
            req.Searchfields.Add(new Models.SearchField("belegdatum", DateTime.Now.AddYears(-1), DateTime.Now));

            // Optional for best performance
            //req.Columns = new List<string>() { "gpname", "gpnummer", "belegart" };
            req.DocumentCountOnly = false;
            req.Token = TfaToken;

            var res = TfaClient.SearchDocuments(req);

            if (res?.StatusCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Search found {res.DocumentsCount} documents.");
                Console.ResetColor();

                DataTable dt;
                dt = topfact.Archive.ApiClient.DataTableHelper.JsonToDatatable(res.Documents);


                foreach (DataRow row in dt.Rows)
                {
                    Console.WriteLine($"DocID: {row["tf_docid"]}");
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
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static bool SearchFulltext(string fulltextSearch)
        {
            var req = new topfact.Archive.Models.Request.SearchRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.Searchfields = new List<Models.SearchField>();
            req.Searchfields.Add(new Models.SearchField("fulltext", fulltextSearch));
            req.Token = TfaToken;

            var res = TfaClient.SearchDocuments(req);

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
        /// 
        /// </summary>
        /// <returns></returns>
        private static bool SearchDocumentsProfessional()
        {
            var req = new topfact.Archive.Models.Request.ProfessionalSearchRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            //req.Columns = new List<string>() { "gpname", "gpnummer", "belegart" };
            req.ProfessionalQuery = "[gpname] LIKE '%mytga%' AND ([belegnummer] = '4711-0815' OR [belegnummer] = '0001-2023')";
            req.SortQuery = "belegart DESC";
            req.Token = TfaToken;

            var res = TfaClient.SearchDocumentsProfessional(req);

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
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="token"></param>
        private static bool AddDocument()
        {
            var req = new topfact.Archive.Models.Request.AddDocumentRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.ArchiveFields = new List<Models.ArchiveField>
            {
                new Models.ArchiveField() { Fieldname = "belegart", Value = "Rechnung" },
                new Models.ArchiveField() { Fieldname = "belegdatum", Value = DateTime.Now.Date },
                new Models.ArchiveField() { Fieldname = "belegbetragbrutto", Value = 11.99 }
            };

            var fi = new System.IO.FileInfo("C:\\Temp\\123.pdf");
            req.ArchiveFiles = new List<Models.ArchiveFile>
            {
                new Models.ArchiveFile() { Filename = fi.Name, Filebinary = System.IO.File.ReadAllBytes(fi.FullName) }
            };

            req.Token = TfaToken;

            var res = TfaClient.AddDocument(req);

            if (res?.StatusCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"New docid {res.DocId} created.");
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
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="docid"></param>
        /// <returns></returns>
        private static bool ChangeDocument(int docid)
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

            req.Token = TfaToken;

            var res = TfaClient.ChangeDocument(req);

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

        private static bool DownloadDocument()
        {
            var req = new topfact.Archive.Models.Request.DownloadDocumentRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.DocId = 4667;
            req.AsPdf = true;
            req.WithAnnotations = true;
            req.Token = TfaToken;

            var res = TfaClient.DownloadDocument(req);

            if (res?.StatusCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Download document was successfully.");
                Console.ResetColor();

                foreach (var file in res.Files)
                {
                    var path = System.IO.Path.Combine("C:\\Temp\\_UnitTest", file.Filename);
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

        private static bool GetDocument()
        {
            var req = new topfact.Archive.Models.Request.GetDocumentRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.DocId = 4667;
            req.IncludeFiles = false;
            req.Token = TfaToken;

            var res = TfaClient.GetDocument(req);

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

        private static bool GetFiles()
        {
            var req = new topfact.Archive.Models.Request.GetFilesRequest();
            req.ArchiveGuid = Constants.ArchiveGuid;
            req.DocId = 4667;
            req.WithBinaries = true;
            req.Token = TfaToken;

            var res = TfaClient.GetFiles(req);

            if (res?.StatusCode == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Get document was successfully.");
                Console.ResetColor();

                foreach (var file in res.Files)
                {
                    var path = System.IO.Path.Combine("C:\\Temp\\_UnitTest", file.Filename);

                    System.IO.File.WriteAllBytes(path, file.Filebinary);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docid"></param>
        private static void ShowInWebviewer(int docid)
        {
            var url = $"{Constants.ClientUrl}/viewer?guid={Constants.ArchiveGuid}&docid={docid}";

            Process.Start(url);
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ShowInWebviewerWithSearch()
        {
            var s = "belegnummer='0001-2023' AND gpname='myTGA GmbH'";
            s = Base64Url.Encode(s);

            var url = $"{Constants.ClientUrl}/viewer?guid={Constants.ArchiveGuid}&s={s}";

            Process.Start(url);
        }
    }
}
