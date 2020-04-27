using System;
using System.Collections.Generic;
using System.Text;

using Lidgren.Network;

using GameInstanceServer.Game;
using GameInstanceServer.Game.Objects;
using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Systems.Networking
{
    /// <summary>
    /// Takes given arguments, and creates the NetOutgoingMessage
    /// packet.
    /// </summary>
    public static class PacketWriter
    {
        /// <summary>
        /// Creates the packet to start the game.
        /// </summary>
        /// <param name="target">Target NetConnection.</param>
        /// <param name="seconds">Milliseconds to countdown from.</param>
        /// <returns>Packet to send to client.</returns>
        public static NetOutgoingMessage 
            GameStartCountdown(NetConnection target, int seconds)
        {
            NetOutgoingMessage message = target.Peer.CreateMessage();
            message.Write((short)NetOpCode.StartGameCountdown);
            message.Write(seconds);

            return message;
        }

        /// <summary>
        /// Creates a packet to assign the local player object.
        /// </summary>
        /// <param name="target">Target NetConnection.</param>
        /// <param name="id">Id of object.</param>
        /// <returns>Packet to send to client.</returns>
        public static NetOutgoingMessage
            AssignLocalPlayerObject(NetConnection target, int id)
        {
            NetOutgoingMessage message = target.Peer.CreateMessage();
            message.Write((short)NetOpCode.AssignLocalObject);
            message.Write(id);

            return message;
        }

        /// <summary>
        /// Creates a packet that provides the initial state for a new
        /// client.
        /// </summary>
        /// <param name="target">Target NetConnection.</param>
        /// <param name="player">Player Entity.</param>
        /// <returns>Packet to send to client.</returns>
        public static NetOutgoingMessage
            WelcomePacket(NetConnection target, PlayerEntity player)
        {
            // Create base message
            NetOutgoingMessage message = target.Peer.CreateMessage();
            message.Write((short)NetOpCode.WelcomePacket);

            // Write localplayer object id
            message.Write((int)player.Id);

            // Get list of nearby objects
            List<Entity> objects = 
                player.World.Cell.GetNearbyObjects(
                    player.Position.Position
                    );

            // Write objects into packet
            message.Write((short)objects.Count);
            foreach(Entity obj in objects)
            {
                try
                {
                    // Hmmmm
                    if (obj.Serializable)
                    {
                        obj.Serialize(message);
                    }
                }
                catch
                {
                }
            }

            return message;
        }


        /// <summary>
        /// Creates a packet that updates an object for the client.
        /// </summary>
        /// <param name="target">Target NetConnection.</param>
        /// <param name="obj">Entity.</param>
        /// <returns>Packet to send to client.</returns>
        public static NetOutgoingMessage
            UpdateObject(NetConnection target, Entity obj)
        {
            // Create base message
            NetOutgoingMessage message = target.Peer.CreateMessage();
            message.Write((short)NetOpCode.UpdateObject);

            obj.Serialize(message);

            return message;
        }

        /// <summary>
        /// Creates a packet that creates an object for the client.
        /// </summary>
        /// <param name="target">Target NetConnection.</param>
        /// <param name="obj">Entity.</param>
        /// <returns>Packet to send to client.</returns>
        public static NetOutgoingMessage
            CreateObject(NetConnection target, Entity obj)
        {
            // Create base message
            NetOutgoingMessage message = target.Peer.CreateMessage();
            message.Write((short)NetOpCode.CreateObject);

            obj.Serialize(message);

            return message;
        }

        /// <summary>
        /// Creates a packet that destroys an object for the client.
        /// </summary>
        /// <param name="target">Target NetConnection.</param>
        /// <param name="obj">Entity.</param>
        /// <returns>Packet to send to client.</returns>
        public static NetOutgoingMessage
            DestroyObject(NetConnection target, Entity obj)
        {
            // Create base message
            NetOutgoingMessage message = target.Peer.CreateMessage();
            message.Write((short)NetOpCode.DestroyObject);
            message.Write(obj.Id);

            return message;
        }
    }
}
