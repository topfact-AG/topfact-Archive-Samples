using System;

namespace topfact.Archive.Samples
{
    class Constants
    {
        /// <summary>
        /// TFA API URL
        /// </summary>
        public static string BaseUrl => Environment.GetEnvironmentVariable("tfa_base_url");

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
        public static string ArchiveGuid => Environment.GetEnvironmentVariable("tfa_archive_guid");
    }
}
