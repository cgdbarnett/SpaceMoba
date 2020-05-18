using System;

using Lidgren.Network;

namespace LoginServer.Networking
{
    /// <summary>
    /// Holds state of a connected client.
    /// </summary>
    public class Client
    {
        private static int NextToken = 50;

        /// <summary>
        /// Value to assign to next token for unique token.
        /// </summary>
        private static int GetNextToken
        {
            get
            {
                if(NextToken == Int32.MaxValue)
                {
                    NextToken = 0;
                }
                return NextToken++;
            }
        }

        /// <summary>
        /// Unique token representing this client.
        /// </summary>
        public readonly int Token = GetNextToken;

        /// <summary>
        /// NetConnection of this client.
        /// </summary>
        public NetConnection Connection;
    }
}
