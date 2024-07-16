using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topfact.Archive.Samples
{
    class Constants
    {
        /// <summary>
        /// TFA API URL
        /// </summary>
        public static string BaseUrl => "https://dev.topfactcloud.de/topfact/api";

        /// <summary>
        /// Webviewer URL
        /// </summary>
        public static string ClientUrl => "https://dev.topfactcloud.de/topfact/client";

        /// <summary>
        /// TFA Username
        /// </summary>
        public static string Username => Environment.GetEnvironmentVariable("tfa_user_cloud");

        /// <summary>
        /// TFA Password
        /// </summary>
        public static string Password => Environment.GetEnvironmentVariable("tfa_pass_cloud");

        public static string ArchiveGuid => "3404D3E3-F975-47B5-A2ED-1208AE02F848";
    }
}
