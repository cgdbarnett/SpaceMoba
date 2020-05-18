using System;
using System.Security.Cryptography;

namespace LoginServer
{
    /// <summary>
    /// Server settings.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Max number of players per team.
        /// </summary>
        public const byte MaxTeamSize = 1;

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
        public const int ListenPort = 8080;

        /// <summary>
        /// Maximum simultaneous connections.
        /// </summary>
        public const int MaximumConnections = 32;
    }
}
