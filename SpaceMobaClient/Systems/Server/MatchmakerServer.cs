using System;

namespace SpaceMobaClient.Systems.Server
{
    /// <summary>
    /// This singleton class is used for communicating with the matchmaking
    /// server.
    /// </summary>
    public class MatchmakerServer
    {
        // Singleton instance of MatchmakerServer
        private static MatchmakerServer Instance;

        // Current remote server host / port.
        public string CurrentGameHost;
        public int CurrentGamePort;
        public int CurrentGameToken;
        public bool CurrentGameActive;

        /// <summary>
        /// Private constructor for singleton.
        /// </summary>
        private MatchmakerServer()
        {
            // Test only configuration
            CurrentGameHost = "127.0.0.1";
            CurrentGamePort = 8080;
            CurrentGameToken = 0;
            CurrentGameActive = true;
        }

        /// <summary>
        /// Returns a reference to the singleton instance of MatchmakerServer.
        /// </summary>
        /// <returns>Reference to MatchmakerServer.</returns>
        public static MatchmakerServer GetMatchmakerServer()
        {
            if(Instance == null)
            {
                Instance = new MatchmakerServer();
            }
            return Instance;
        }
    }
}
