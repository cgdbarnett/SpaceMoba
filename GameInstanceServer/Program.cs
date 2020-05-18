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
                int.TryParse(args[0], out int port);
                int.TryParse(args[1], out int count);
                int[] tokens = new int[count];
                for (int i = 0; i < count; i++)
                {
                    int.TryParse(args[i + 2], out tokens[i]);
                }

                Trace.WriteLine("Starting ServerManager.");
                Trace.IndentLevel++;

                // Create server manager
                ServerManager server = new ServerManager(port, count, tokens);

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
                Trace.WriteLine("Unhandled exception in GameInstanceServer:");
                Trace.WriteLine(e.Message);
            }
        }
    }
}
