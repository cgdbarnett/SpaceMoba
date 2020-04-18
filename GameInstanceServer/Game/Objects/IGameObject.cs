using System;

using Microsoft.Xna.Framework;

using Lidgren.Network;

using GameInstanceServer.Game.World;

namespace GameInstanceServer.Game.Objects
{
    /// <summary>
    /// Represents a game object that is managed by the GameSimulation.
    /// </summary>
    public interface IGameObject
    {
        /// <summary>
        /// Returns the id of this object.
        /// </summary>
        /// <returns>Id of this object.</returns>
        int GetId();

        /// <summary>
        /// Executes a single frame of logic for this object.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        void Update(TimeSpan gameTime);

        /// <summary>
        /// Returns the position in world coordinates.
        /// </summary>
        /// <returns>Position of object.</returns>
        Point GetPosition();

        /// <summary>
        /// Returns a reference to the current cell containing this object.
        /// </summary>
        /// <returns>Cell containing this object.</returns>
        WorldCell GetCell();

        /// <summary>
        /// Sets the current cell to the referenced cell.
        /// </summary>
        /// <param name="cell">Cell now containing object.</param>
        void SetCell(WorldCell cell);

        /// <summary>
        /// Serializes the replication data of this object into an outgoing
        /// message.
        /// </summary>
        /// <param name="message">Outgoing message.</param>
        void Serialize(NetOutgoingMessage message);

        /// <summary>
        /// Serializes the replication data of this object into an outgoing
        /// message. Only serializes positional + physics data.
        /// </summary>
        /// <param name="message">Outgoing message.</param>
        void SerializePosition(NetOutgoingMessage message);

        /// <summary>
        /// Serializes the replication data of this object into an outgoing
        /// message. Only serializes combat data.
        /// </summary>
        /// <param name="message">Outgoing message.</param>
        void SerializeCombat(NetOutgoingMessage message);

        /// <summary>
        /// Serializes the replication data of this object into an outgoing
        /// message. Only serializes graphic model data.
        /// </summary>
        /// <param name="message">Outgoing message.</param>
        void SerializeModel(NetOutgoingMessage message);
    }
}
