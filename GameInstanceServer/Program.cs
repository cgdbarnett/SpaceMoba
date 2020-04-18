using System;
using System.Diagnostics;

namespace GameInstanceServer
{
    /// <summary>
    /// Entry point of GameInstanceServer.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry point. Starts and runs server.
        /// </summary>
        /// <param name="args">Command line arguments.</param> 
        static void Main(string[] args)
        {
            // Setup debug log listeners
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            try
            {
                // Read arguments
                int[] tokens = new int[6];
                int.TryParse(args[0], out int port);
                for (int i = 0; i < 6; i++)
                {
                    int.TryParse(args[i + 1], out tokens[i]);
                }

                Trace.WriteLine("Starting ServerManager.");
                Trace.IndentLevel++;
                // Create server manager
                ServerManager server = new ServerManager(port, tokens);
                Trace.IndentLevel--;
                Trace.WriteLine("End ServerManager.");

                Trace.WriteLine("Starting ServerManager.Run loop.");
                Trace.IndentLevel++;
                // Run server
                while (server.Running)
                    server.Run();
            }
            catch(Exception e)
            {
                Trace.WriteLine("Error occured starting GameInstanceServer:");
                Trace.WriteLine(e.Message);
            }
        }
    }
}
