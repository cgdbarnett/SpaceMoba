using System;

using System.Threading;

namespace SpaceMobaClient
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.Sleep(2000);

            using (var game = GameClient.GetGameClient())
                game.Run();
        }
    }
}
