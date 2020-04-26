using System;
using System.Collections.Generic;
using System.Diagnostics;

using Lidgren.Network;

using GameInstanceServer.Game;
using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Position System.
        public static readonly ComponentSystemId NetworkingSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Systems.Networking
{
    /// <summary>
    /// Networking System. Manages networking.
    /// </summary>
    public class NetworkingSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a NetworkingSystem.
        /// </summary>
        public NetworkingSystem() : base()
        {
            Id = ComponentSystemId.NetworkingSystem;
            WantsUpdates = true;
        }

        /// <summary>
        /// Start the net server when the component is registered.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <param name="component">NetworkingComponent.</param>
        public override void RegisterComponent(int id, IComponent component)
        {
            base.RegisterComponent(id, component);

            Trace.WriteLine("Initialising Networking.");
            Trace.Indent();

            // Start NetServer
            ((NetworkingComponent)component).NetServer.Start();

            // Register event listeners
            GameMaster.OnStartCountdown += HandleStartCountdown;

            Trace.Unindent();
            Trace.WriteLine("End Networking.");
        }

        /// <summary>
        /// Shutdown the net server when the component is unregistered.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        public override void UnregisterComponent(int id)
        {
            NetworkingComponent component = 
                (NetworkingComponent)Components[id];
            base.UnregisterComponent(id);

            component.NetServer.Shutdown("0");
        }

        /// <summary>
        /// Update the networking component.
        /// </summary>
        /// <param name="component">NetworkingComponent.</param>
        /// <param name="gameTime">Game frame interval.</param>
        protected override void UpdateComponent(
            IComponent component, TimeSpan gameTime
            )
        {
            NetworkingComponent netComponent = (NetworkingComponent)component;

            // Read incoming messages
            List<NetIncomingMessage> messages = new List<NetIncomingMessage>();
            netComponent.NetServer.ReadMessages(messages);

            // Iterate through all received messages
            foreach (NetIncomingMessage msg in messages)
            {
                switch (msg.MessageType)
                {
                    // Attempt to approve incoming connection
                    case NetIncomingMessageType.ConnectionApproval:
                        HandleIncomingConnection(netComponent, msg);
                        break;

                    // Handle incoming data, expecting ClientIsReady packet.
                    case NetIncomingMessageType.Data:
                        HandleData(netComponent, msg);
                        break;

                    // Handle incoming disconnection
                    case NetIncomingMessageType.StatusChanged:
                        HandleStatusChange(netComponent, msg);
                        break;
                }
            }
        }

        /// <summary>
        /// Reads an incoming message, and determines whether we can approve
        /// the new connection.
        /// </summary>
        /// <param name="msg">Incoming message.</param>
        private void HandleIncomingConnection(
            NetworkingComponent component, NetIncomingMessage msg
            )
        {
            Trace.WriteLine("Incoming Connection.");
            Trace.Indent();

            int token = msg.ReadInt32();

            // Try find token in Tokens
            if (component.Clients.ContainsKey(token))
            {
                // Client is already connected, check if active
                if (component.Clients[token].Active)
                {
                    DenyIncomingConnection(component, msg);
                }
                else
                {
                    // Todo: Clean up old client first.
                    ApproveIncomingConnection(component, token, msg, true);
                }
            }
            else if(component.Tokens.Contains(token))
            {
                ApproveIncomingConnection(component, token, msg);
            }
            else
            {
                DenyIncomingConnection(component, msg);
            }

            Trace.Unindent();
            Trace.WriteLine("End Connection.");
        }

        /// <summary>
        /// Approves, and starts a connecting client.
        /// </summary>
        /// <param name="component">NetworkingComponent.</param>
        /// <param name="msg">Incoming message.</param>
        private void ApproveIncomingConnection(
            NetworkingComponent component, int token, 
            NetIncomingMessage msg, bool cleanup = false
            )
        {
            Trace.WriteLine("Client approved.");

            if (cleanup)
            {
                // To do
                throw (new NotImplementedException());
            }
            else
            {
                // Create new player entity
                PlayerEntity player = new PlayerEntity();
                player.RegisterComponents();

                // Set new player client component
                component.Clients.Add(token, player.Client);
                component.Connections.Add(msg.SenderConnection, player.Client);
                component.Clients[token].NetPeer = msg.SenderConnection.Peer;
                component.Clients[token].NetConnection = msg.SenderConnection;
            }

            // Send approval
            msg.SenderConnection.Approve();
        }

        /// <summary>
        /// Denies an incoming connection.
        /// </summary>
        /// <param name="component">NetworkingComponent.</param>
        /// <param name="msg">Incoming message.</param>
        private void DenyIncomingConnection(
            NetworkingComponent component, NetIncomingMessage msg
            )
        {
            Trace.WriteLine("Client denied.");
            msg.SenderConnection.Deny();
        }

        /// <summary>
        /// Enqueues incoming data to the correct NetworkingClient.
        /// </summary>
        /// <param name="component">NetworkingComponent.</param>
        /// <param name="msg">Incoming message.</param>
        private void HandleData(
            NetworkingComponent component, NetIncomingMessage msg
            )
        {
            component.Connections[msg.SenderConnection]
                .IncomingMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Handles a change in status from a client. This may indicate
        /// a disconnection.
        /// </summary>
        /// <param name="component">NetworkingComponent.</param>
        /// <param name="msg">Message to process.</param>
        private void HandleStatusChange(
            NetworkingComponent component, NetIncomingMessage msg
            )
        {
            switch (msg.SenderConnection.Status)
            {
                case NetConnectionStatus.Disconnected:
                    Trace.WriteLine("Client disconnected.");

                    component.Connections[msg.SenderConnection].Active = false;
                    break;
            }
        }

        /// <summary>
        /// Sends a packet to all connected clients to start the countdown.
        /// </summary>
        /// <param name="sender">Null.</param>
        /// <param name="time">Time in seconds of countdown.</param>
        private void HandleStartCountdown(object sender, int time)
        {
            foreach(NetworkingComponent component in Components.Values)
            {
                foreach(NetworkingClientComponent client 
                    in component.Clients.Values)
                {
                    NetOutgoingMessage msg = 
                        PacketWriter.GameStartCountdown(
                            client.NetConnection, time * 1000
                            );

                    client.OutgoingMessageQueue.Enqueue(msg);
                }
            }
        }
    }
}