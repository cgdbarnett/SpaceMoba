using System;
using System.Collections.Generic;

using Lidgren.Network;

using GameInstanceServer.Game.Teams;
using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Systems.Networking
{
    /// <summary>
    /// The NetworkingComponent holds state for NetworkingSystem. There should
    /// only ever be one instance of this component.
    /// </summary>
    public class NetworkingComponent : IComponent
    {
        // Id used for lidgren to identify game.
        private const string LidgrenAppId = "smc20";

        // Networking data
        public NetServer NetServer;
        public Dictionary<int, NetworkingClientComponent> Clients;
        public Dictionary<NetConnection, NetworkingClientComponent> 
            Connections;

        public readonly Dictionary<int, Team> Tokens;

        /// <summary>
        /// Returns the number of connected clients.
        /// </summary>
        public int ConnectedClients
        {
            get;
            set;
        }

        /// <summary>
        /// Get the ComponentSystemId for the Networking System.
        /// </summary>
        public ComponentSystemId ComponentSystem
        {
            get
            {
                return ComponentSystemId.NetworkingSystem;
            }
        }

        /// <summary>
        /// Creates the networking component.
        /// </summary>
        public NetworkingComponent(
            int port, int[] tokens, Team TeamA, Team TeamB
            )
        {
            ConnectedClients = 0;

            Clients = new Dictionary<int, NetworkingClientComponent>();
            Connections = new Dictionary<NetConnection, NetworkingClientComponent>();

            // Configure server
            NetPeerConfiguration config =
                new NetPeerConfiguration(LidgrenAppId)
                {
                    Port = port
                };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            // Copy tokens into memory
            Tokens = new Dictionary<int, Team>();
            for(int i = 0; i < tokens.Length; i++)
            {
                if (i < tokens.Length / 2)
                {
                    Tokens.Add(tokens[i], TeamA);
                }
                else
                {
                    Tokens.Add(tokens[i], TeamB);
                }
            }

            // Start server
            NetServer = new NetServer(config);
        }

        /// <summary>
        /// This is not a serializable component.
        /// </summary>
        public bool Serializable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// This is not a serializable component.
        /// </summary>
        /// <param name="msg"></param>
        public void Serialize(NetOutgoingMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
