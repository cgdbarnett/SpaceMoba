using System;
using System.Security.Cryptography;

namespace SpaceMobaClient
{
    public static class Settings
    {
        // App Identifier is based off the MD5 hash of
        // a buffer containing "SMCBH" (Space Moba Client Black Hole).
        private static readonly byte[] AppByteCode = new byte[] {
            (byte)'S', (byte)'M', (byte)'C', (byte)'B', (byte)'H',
            };

        /// <summary>
        /// Unique identification of Application.
        /// </summary>
        public static string AppIdentifier =
            MD5.Create().ComputeHash(AppByteCode).ToString();

        /// <summary>
        /// Port the login server listens on for new connections.
        /// </summary>
        public const int LoginPort = 8080;

        /// <summary>
        /// Host of the login server.
        /// </summary>
#if DEBUG
        public static readonly string LoginHost = "127.0.0.1";
#else
        public static readonly string LoginHost = "3.14.141.85";
#endif
    }
}
