using System;
using System.Diagnostics;
using System.Threading;

namespace LoginServer
{
    class Program
    {
        /// <summary>
        /// Entry point of login server.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Setup debug log listeners
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            // Init server
            LoginServerManager server = new LoginServerManager();

            Trace.WriteLine("Server open.");

            // Run server
            while(server.Active)
            {
                // Run
                server.Run();
                Thread.Sleep(50);
            }

            Trace.WriteLine("Server closed.");
        }
    }
}
