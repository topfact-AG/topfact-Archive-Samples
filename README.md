# topfact Archive Samples

**Code samples and documentation for integrating with the topfact Archive API using .NET**

## Table of Contents

- [What This Project Does](#what-this-project-does)
- [Key Features](#key-features)
- [Getting Started](#getting-started)
- [Usage Examples](#usage-examples)
- [Project Structure](#project-structure)
- [Requirements](#requirements)
- [Configuration](#configuration)
- [Support & Resources](#support--resources)
- [Contributing](#contributing)
- [License](#license)

---

## What This Project Does

This project provides comprehensive C# code samples demonstrating how to interact with the **topfact Archive API**. It showcases common operations like authentication, document management, searching, and archive operations using the `TfaApiClient` library.

Whether you're building a new integration or learning the API, these samples provide a solid foundation for working with the topfact document archive platform.

---

## Key Features

- **Authentication**: Secure login and token-based authentication examples
- **Archive Management**: Retrieve and manage your archives
- **Document Operations**: Add, retrieve, and manage documents with custom metadata
- **Full-Text Search**: Search documents using fulltext indexing
- **Advanced Search**: Professional search capabilities with filters and date ranges
- **Async Support**: Both synchronous and asynchronous API methods
- **Error Handling**: Proper error handling and response validation patterns

---

## Getting Started

### Requirements

- **.NET Framework 4.7.2** or later
- Visual Studio 2019 or higher (or any compatible IDE)
- topfact Cloud account with API access
- topfact Archive API client and models libraries

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/topfact-AG/topfact-Archive-Samples.git
   cd topfact-Archive-Samples
   ```

2. **Open the solution**
   ```bash
   # Open in Visual Studio
   start topfact.Archive.Samples.sln
   ```

3. **Restore dependencies**
   - The project uses NuGet packages:
     - `log4net` (v3.3.1)
     - `Microsoft.AspNet.WebApi.Client` (v6.0.0)
     - `Newtonsoft.Json` (v13.0.4)
   - Visual Studio will automatically restore these on build

4. **Configure credentials** (see [Configuration](#configuration) section below)

5. **Build and run**
   ```bash
   dotnet build
   dotnet run --project Samples\topfact.Archive.Samples.csproj
   ```

---

## Usage Examples

### Authentication

The first step is always to authenticate and obtain a security token:

```csharp
// Initialize the API client (uses Constants.BaseUrl from environment variable)
var client = new topfact.Archive.ApiClient.TfaApiClient(Constants.BaseUrl);

// Create authentication helper
var auth = new AuthenticationSamples(client);

// Login and get token (uses Constants.Username and Constants.Password from environment variables)
var token = auth.Logon();
if (token != null)
{
    Console.WriteLine($"Logged in as {token.Username}, expires on {token.ValidTo}");
}
```

### Retrieve Archives

```csharp
var archivesSamples = new ArchivesSamples(client, token);
var archives = archivesSamples.GetArchives();

foreach (var archive in archives)
{
    Console.WriteLine($"Archive: {archive.Name}");
}
```

### Add a Document

```csharp
var documentsSamples = new DocumentsSamples(client, token);

var request = new topfact.Archive.Models.Request.AddDocumentRequest();
request.ArchiveGuid = Constants.ArchiveGuid;  // Uses environment variable tfa_archive_guid

// Set metadata fields
request.ArchiveFields = new List<Models.ArchiveField>
{
    new Models.ArchiveField { Fieldname = "belegart", Value = "Rechnung" },
    new Models.ArchiveField { Fieldname = "belegdatum", Value = DateTime.Now.Date },
    new Models.ArchiveField { Fieldname = "belegbetragbrutto", Value = 99.99 }
};

// Add files
var fileInfo = new System.IO.FileInfo("C:\\Temp\\document.pdf");
request.ArchiveFiles = new List<Models.ArchiveFile>
{
    new Models.ArchiveFile 
    { 
        Filename = fileInfo.Name, 
        Filebinary = System.IO.File.ReadAllBytes(fileInfo.FullName) 
    }
};

var response = documentsSamples.AddDocument(request);
if (response?.StatusCode == 0)
{
    Console.WriteLine($"Document added with ID: {response.DocId}");
}
```

### Search Documents

```csharp
var searchSamples = new SearchSamples(client, token);

// Simple search
searchSamples.SearchDocuments();

// Full-text search
searchSamples.SearchFulltext();

// Professional search with advanced filters
searchSamples.SearchDocumentsProfessional();
```

### Asynchronous Operations

All samples include async variants:

```csharp
var auth = new AuthenticationSamples(client);
var token = await auth.LogonAsync();

var archives = await new ArchivesSamples(client, token).GetArchivesAsync();
```

---

## Project Structure

```
topfact-Archive-Samples/
├── Samples/
│   ├── App.config                    # Application configuration
│   ├── Program.cs                    # Main entry point - orchestrates all samples
│   ├── Constants.cs                  # Configuration constants and environment variables
│   ├── Samples/
│   │   ├── AuthenticationSamples.cs  # Login and token management
│   │   ├── ArchivesSamples.cs        # Archive retrieval operations
│   │   ├── DocumentsSamples.cs       # Document management operations
│   │   └── SearchSamples.cs          # Search and fulltext operations
│   ├── Utils/
│   │   └── Base64Url.cs              # Utility for Base64 URL encoding/decoding
│   └── Properties/
│       └── AssemblyInfo.cs           # Assembly metadata
├── topfact.Archive.Samples.sln       # Visual Studio solution file
└── README.md                         # This file
```

---

## Requirements

### System Requirements
- Windows or .NET-compatible operating system
- .NET Framework 4.7.2+
- Visual Studio 2019, 2022, or equivalent IDE

### API Requirements
- Valid topfact Cloud account
- Configured environment variables: `tfa_user_cloud` and `tfa_pass_cloud` (required)
- Optional environment variables: `tfa_base_url`, `tfa_client_url`, and `tfa_archive_guid` (uses defaults if not set)

### Dependencies
- **topfact.Archive.ApiClient** (v2024.7.1.15) - Main API client library
- **topfact.Archive.Models** (v2024.7.1.36) - Data models and contracts
- **log4net** (v3.3.1) - Logging framework
- **Microsoft.AspNet.WebApi.Client** (v6.0.0) - HTTP client utilities
- **Newtonsoft.Json** (v13.0.4) - JSON serialization

---

## Configuration

### Environment Variables

All configuration is managed through environment variables for security and flexibility. Set these before running:

#### Required Credentials
```powershell
# PowerShell
$env:tfa_user_cloud = "your_username"
$env:tfa_pass_cloud = "your_password"

# Command Prompt
set tfa_user_cloud=your_username
set tfa_pass_cloud=your_password

# Linux/macOS
export tfa_user_cloud="your_username"
export tfa_pass_cloud="your_password"
```

#### Optional API Configuration
You can customize the API endpoints and archive settings via environment variables:

```powershell
# PowerShell
$env:tfa_base_url = "https://app.topfactcloud.de/xxxx/topfact/api"
$env:tfa_client_url = "https://app.topfactcloud.de/xxxx/topfact/client"
$env:tfa_archive_guid = "<Guid>"

# Command Prompt
set tfa_base_url=https://app.topfactcloud.de/xxxx/topfact/api
set tfa_client_url=https://app.topfactcloud.de/xxxx/topfact/client
set tfa_archive_guid=<Guid>

# Linux/macOS
export tfa_base_url="https://app.topfactcloud.de/xxxx/topfact/api"
export tfa_client_url="https://app.topfactcloud.de/xxxx/topfact/client"
export tfa_archive_guid="<Guid>"
```

#### Environment Variable Summary

| Variable | Description | Default Value | Required |
|----------|-------------|---------------|---------:|
| `tfa_user_cloud` | topfact Cloud username | - | ✓ Yes |
| `tfa_pass_cloud` | topfact Cloud password | - | ✓ Yes |
| `tfa_base_url` | API base URL | `https://app.topfactcloud.de/xxxx/topfact/api` | ✗ No |
| `tfa_client_url` | Web client URL | `https://app.topfactcloud.de/xxxx/topfact/client` | ✗ No |
| `tfa_archive_guid` | Default archive GUID | `<Guid>` | ✗ No |

### Constants Configuration

Update `Constants.cs` to read from environment variables:

```csharp
using System;

namespace topfact.Archive.Samples
{
    class Constants
    {
        /// <summary>
        /// TFA API URL (from environment variable tfa_base_url)
        /// </summary>
        public static string BaseUrl => 
            Environment.GetEnvironmentVariable("tfa_base_url") 
            ?? "https://app.topfactcloud.de/xxxx/topfact/api";

        /// <summary>
        /// Webviewer URL (from environment variable tfa_client_url)
        /// </summary>
        public static string ClientUrl => 
            Environment.GetEnvironmentVariable("tfa_client_url") 
            ?? "https://app.topfactcloud.de/xxxx/topfact/client";

        /// <summary>
        /// TFA Username (from environment variable tfa_user_cloud)
        /// </summary>
        public static string Username => 
            Environment.GetEnvironmentVariable("tfa_user_cloud");

        /// <summary>
        /// TFA Password (from environment variable tfa_pass_cloud)
        /// </summary>
        public static string Password => 
            Environment.GetEnvironmentVariable("tfa_pass_cloud");

        /// <summary>
        /// Archive GUID (from environment variable tfa_archive_guid)
        /// </summary>
        public static string ArchiveGuid => 
            Environment.GetEnvironmentVariable("tfa_archive_guid") 
            ?? "<Guid>";
    }
}
```

### App.config

The application configuration can be customized in `App.config`. Refer to the project's App.config file for logging and other framework settings.

### Example Setup Scripts

#### Windows (PowerShell)
```powershell
# Set all required variables
$env:tfa_user_cloud = "your_username"
$env:tfa_pass_cloud = "your_password"
$env:tfa_base_url = "https://app.topfactcloud.de/xxxx/topfact/api"
$env:tfa_archive_guid = "<Guid>"

# Run the application
.\bin\Debug\topfact.Archive.Samples.exe
```

#### Linux/macOS (Bash)
```bash
# Set all required variables
export tfa_user_cloud="your_username"
export tfa_pass_cloud="your_password"
export tfa_base_url="https://app.topfactcloud.de/xxxx/topfact/api"
export tfa_archive_guid="<Guid>"

# Run the application
dotnet run --project Samples/topfact.Archive.Samples.csproj
```

---

## Support & Resources

### Getting Help

- **GitHub Issues**: Report bugs or request features via [GitHub Issues](https://github.com/topfact-AG/topfact-Archive-Samples/issues)
- **topfact Documentation**: Visit the official [topfact documentation](https://www.topfact.de) for API reference and advanced topics
- **Code Comments**: Each sample class includes XML documentation with detailed descriptions
- **API Help**: For detailed endpoint documentation, see [topfact Archive API Help](https://dev.topfactcloud.de/topfact/api/Help)

### Related Documentation

- **API Reference**: Check the topfact Archive API documentation for detailed endpoint specifications
- **Authentication Guide**: Review `AuthenticationSamples.cs` for token management best practices
- **Error Handling**: See sample implementations for proper error handling patterns

---

## Contributing

We welcome contributions! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

Please ensure:
- Code follows the existing style and conventions
- Changes include appropriate comments and documentation
- New features include corresponding sample methods
- All existing samples continue to work

---

## License

This project is part of the topfact Archive ecosystem. For licensing information, refer to the [LICENSE](LICENSE) file in this repository.

---

## Additional Notes

### Performance Considerations

- **Reuse API Client**: Initialize `TfaApiClient` once and reuse it for better performance and connection pooling
- **Token Management**: Cache tokens appropriately to reduce authentication calls
- **Async Operations**: Use async methods for long-running operations in production environments
- **Document Size**: For large file uploads, consider chunking or streaming approaches

### Best Practices

1. Always validate API responses before processing
2. Handle HTTP exceptions appropriately
3. Use environment variables for sensitive data (never hardcode credentials)
4. Implement proper logging using log4net
5. Test search queries before deploying to production

---

**topfact AG** | [Visit Our Website](https://www.topfact.de)

For the latest updates and support, visit the [topfact Archive GitHub repository](https://github.com/topfact-AG/topfact-Archive-Samples).
