using System;
using System.Text;

namespace topfact.Archive.Samples.Utils
{
    /// <summary>
    /// Provides methods for encoding and decoding strings using Base64 URL format (RFC 4648).
    /// </summary>
    public class Base64Url
    {
        /// <summary>
        /// Encodes a string to Base64 URL format (RFC 4648)
        /// </summary>
        /// <param name="text">The string to encode.</param>
        /// <returns>The Base64 URL encoded string.</returns>
        public static string Encode(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// Decodes a Base64 URL encoded string (RFC 4648)
        /// </summary>
        /// <param name="text">The Base64 URL encoded string.</param>
        /// <returns>The decoded string.</returns>
        public static string Decode(string text)
        {
            text = text.Replace('_', '/').Replace('-', '+');
            switch (text.Length % 4)
            {
                case 2:
                    text += "==";
                    break;
                case 3:
                    text += "=";
                    break;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }
    }
}
