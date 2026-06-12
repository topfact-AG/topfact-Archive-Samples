using System;

namespace topfact.Archive.Samples
{
    class Constants
    {
        /// <summary>
        /// TFA API URL
        /// </summary>
        public static string BaseUrl => "https://app.topfactcloud.de/0001/topfact/api";

        /// <summary>
        /// Webviewer URL
        /// </summary>
        public static string ClientUrl => "https://app.topfactcloud.de/0001/topfact/client";

        /// <summary>
        /// TFA Username
        /// </summary>
        public static string Username => Environment.GetEnvironmentVariable("tfa_user_cloud");

        /// <summary>
        /// TFA Password
        /// </summary>
        public static string Password => Environment.GetEnvironmentVariable("tfa_pass_cloud");

        /// <summary>
        /// Archive GUID
        /// </summary>
        public static string ArchiveGuid => "3404D3E3-F975-47B5-A2ED-1208AE02F848";
    }
}
