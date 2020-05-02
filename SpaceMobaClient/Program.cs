using System;

using System.Threading;

namespace SpaceMobaClient
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public static string[] Parameters;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        [STAThread]
        static void Main(string[] args)
        {
            Parameters = args;

            Thread.Sleep(2000);

            using (var game = GameClient.GetGameClient())
                game.Run();
        }
    }
}
