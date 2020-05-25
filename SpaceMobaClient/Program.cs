using System;

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
            GameManager.Run();
        }
    }
}
