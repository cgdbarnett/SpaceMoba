using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using Lidgren.Network;

using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Game;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Position System.
        public static readonly ComponentSystemId NetworkingClientSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Systems.Networking
{
    /// <summary>
    /// Networking Client System. Runs the client components of networking.
    /// </summary>
    public class NetworkingClientSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a NetworkingSystem.
        /// </summary>
        public NetworkingClientSystem() : base()
        {
            Id = ComponentSystemId.NetworkingClientSystem;
            WantsUpdates = true;
        }

        /// <summary>
        /// Update a client in the game.
        /// </summary>
        /// <param name="component">NetworkingClientComponent.</param>
        /// <param name="gameTime">Game frame interval.</param>
        protected override void UpdateComponent(
            IComponent component, TimeSpan gameTime
            )
        {
            NetworkingClientComponent client = (NetworkingClientComponent)component;

            // Replicate
            ReplicateForClient(client);

            // Flush outgoing messages
            while(client.OutgoingMessageQueue.Count > 0)
            {
                client.NetPeer.SendMessage(
                    client.OutgoingMessageQueue.Dequeue(),
                    client.NetConnection,
                    NetDeliveryMethod.UnreliableSequenced
                    );
            }

            // Flush incoming messages
            while(client.IncomingMessageQueue.Count > 0)
            {
                HandleMessage(client, client.IncomingMessageQueue.Dequeue());
            }
        }

        /// <summary>
        /// Creates replication updates for client.
        /// </summary>
        private void ReplicateForClient(NetworkingClientComponent client)
        {
            // Get nearby objects
            List<Entity> objects =
                client.Entity.World.Cell.GetNearbyObjects(
                    client.Entity.Position.Position
                    );

            // Iterate through objects
            Dictionary<Entity, long> newReplicated = new Dictionary<Entity, long>();

            foreach(Entity entity in objects)
            {
                if (entity.Serializable)
                {
                    if (client.ReplicatedEntities.ContainsKey(entity))
                    {
                        // Update
                        newReplicated.Add(
                            entity, client.ReplicatedEntities[entity]
                            );

                        // Only update if entity has had a state change, or if
                        // it hasn't been updated in over 1 second.
                        if (entity.LastUpdated > newReplicated[entity] ||
                            GameMaster.ElapsedMilliseconds
                            > newReplicated[entity] 
                            + Settings.ReplicationMaxUpdatePeriod
                            )
                        {
                            NetOutgoingMessage msg =
                                PacketWriter.UpdateObject(
                                    client.NetConnection, entity
                                    );
                            client.OutgoingMessageQueue.Enqueue(msg);
                            newReplicated[entity] = 
                                GameMaster.ElapsedMilliseconds;
                        }

                        client.ReplicatedEntities.Remove(entity);
                    }
                    else
                    {
                        // New
                        newReplicated.Add(
                            entity, GameMaster.ElapsedMilliseconds
                            );
                        NetOutgoingMessage msg =
                            PacketWriter.CreateObject(
                                client.NetConnection, entity
                                );
                        client.OutgoingMessageQueue.Enqueue(msg);
                    }
                }
            }

            // Any entities left in Replicated Entities need to be destroyed
            foreach(Entity entity in client.ReplicatedEntities.Keys)
            {
                // Destroy
                NetOutgoingMessage msg =
                    PacketWriter.DestroyObject(
                        client.NetConnection, entity
                        );
                client.OutgoingMessageQueue.Enqueue(msg);
            }
            
            // GC should clear up the old dictionary
            client.ReplicatedEntities = newReplicated;

            // Replicate team
            {
                NetOutgoingMessage msg = PacketWriter.UpdateTeam(
                    client.NetConnection, client.Entity);
                client.OutgoingMessageQueue.Enqueue(msg);
            }
        }

        /// <summary>
        /// Handles an incoming message for this client.
        /// </summary>
        /// <param name="msg"></param>
        private void HandleMessage(
            NetworkingClientComponent client, NetIncomingMessage msg
            )
        {
            NetOpCode opcode = (NetOpCode)msg.ReadInt16();

            try
            {
                switch (opcode)
                {
                    // Handle the client is ready.
                    case NetOpCode.ClientIsReady:
                        {
                            Trace.WriteLine("Client is ready.");
                            client.Ready = true;

                            // Send welcome packet
                            NetOutgoingMessage welcome = 
                                PacketWriter.WelcomePacket(
                                    client.NetConnection, client.Entity
                                    );
                            client.OutgoingMessageQueue.Enqueue(welcome);
                        }
                        break;

                    // Handle input from the client.
                    case NetOpCode.UpdatePlayerInput:
                        {
                            // Temporary force input
                            client.Entity.Engine.InputForce.X =
                                (msg.ReadByte() - 1f) * 
                                client.Entity.Engine.EngineForce.X;
                            client.Entity.Engine.InputForce.Y =
                                (msg.ReadByte() - 1f) *
                                client.Entity.Engine.EngineForce.Y;
                            client.Entity.Engine.InputAngularForce =
                                (msg.ReadByte() - 1f) *
                                client.Entity.Engine.EngineAngularForce;
                            bool attack = msg.ReadBoolean(); // Disregard atm
                            client.Entity.LastUpdated = 
                                GameMaster.ElapsedMilliseconds;
                        }
                        break;

                    default:
                        Trace.WriteLine("Received unexpected opcode.");
                        break;
                }
            }
            catch
            {

            }
        } 
    }
}