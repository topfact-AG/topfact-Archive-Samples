Imports topfact.Archive
Imports topfact.Archive.Models.Lists

Module Module1

    Private Property TfaClient As topfact.Archive.ApiClient.TfaApiClient

    Private Property TfaToken As topfact.Archive.Models.Security.Token


    Private Property BaseUrl As String = "https://dev.topfactcloud.de/topfact/api" ' replace with customer url

    Private Property ArchiveGuid As String = "3404D3E3-F975-47B5-A2ED-1208AE02F848" ' replace with correct archive guid

    Sub Main()

        TfaClient = New topfact.Archive.ApiClient.TfaApiClient(BaseUrl)

        Dim token = Logon()

        Dim tokenApp = LogonAsApp()

        If token Is Nothing Then
            Console.WriteLine("Invalid credentials.")
            Return
        Else
            TfaToken = token
            Console.WriteLine($"Logged on as {token.Username}, expires on {token.ValidTo}.")
        End If


        SearchDocuments()

    End Sub

    Private Function Logon() As topfact.Archive.Models.Security.Token

        Dim un As String = Environment.GetEnvironmentVariable("tfa_user", EnvironmentVariableTarget.User)
        Dim pw As String = Environment.GetEnvironmentVariable("tfa_pass", EnvironmentVariableTarget.User)

        Try
            Dim token = TfaClient.Logon(un, pw)
            Return token
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Private Function LogonAsApp() As topfact.Archive.Models.Security.Token

        Dim clientID As String = Environment.GetEnvironmentVariable("tfa_clientid", EnvironmentVariableTarget.User)
        Dim clientKey As String = Environment.GetEnvironmentVariable("tfa_clientkey", EnvironmentVariableTarget.User)

        Try
            Dim token = TfaClient.LogonApp(clientID, clientKey)
            Return token
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Private Sub SearchDocuments()

        Dim req = New topfact.Archive.Models.Request.SearchRequest()
        req.ArchiveGuid = ArchiveGuid
        req.Searchfields = New List(Of Models.SearchField)
        'req.Searchfields.Add(New Models.SearchField("belegart", "Rechnung"))
        req.Searchfields.Add(New Models.SearchField("belegnummer", "IN(7082776117,7082776152,7082776154)"))
        'req.Searchfields.Add(New Models.SearchField("belegdatum", DateTime.Now.AddYears(-1), DateTime.Now))

        'Optional for best performance
        req.Columns = {"gpname", "gpnummer", "belegart"}.ToList()
        req.DocumentCountOnly = False
        req.Token = TfaToken

        Dim res = TfaClient.SearchDocuments(req)

        If res.StatusCode = 0 Then

            Dim dt = topfact.Archive.ApiClient.DataTableHelper.JsonToDatatable(res.Documents)

            For Each row As DataRow In dt.Rows
                Dim docid As Integer = Convert.ToInt32(row("tf_docid"))
                DownloadDocument(docid)
            Next

        End If

    End Sub

    Private Sub DownloadDocument(docid As Integer)

        Dim req = New topfact.Archive.Models.Request.DownloadDocumentRequest()
        req.ArchiveGuid = ArchiveGuid
        req.DocId = docid
        req.Token = TfaToken

        Dim res = TfaClient.DownloadDocument(req)

        If res.StatusCode = 0 Then

            For Each file In res.Files

                Dim path = System.IO.Path.Combine("C:\Temp", file.Filename)
                System.IO.File.WriteAllBytes(path, file.Filebinary)

            Next

        End If

    End Sub

End Module
