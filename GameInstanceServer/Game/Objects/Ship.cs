using System;

using Microsoft.Xna.Framework;

using Lidgren.Network;

using GameInstanceServer.Game.World;

namespace GameInstanceServer.Game.Objects
{
    /// <summary>
    /// Base ship class. Handles physics.
    /// </summary>
    public class Ship : IGameObject
    {
        protected readonly int Id;
        protected WorldCell Cell;

        // Physics
        protected Vector2 Position;
        protected Vector2 Momentum;
        protected Vector2 Force;
        protected Vector2 ExternalForce;
        protected Vector2 ExternalMomentum;
        protected float AngularMomentum;
        protected float Direction;
        protected float MaxAngularMomentum;
        protected float MaxLinearMomentum;

        /// <summary>
        /// Creates a new Ship object.
        /// </summary>
        /// <param name="id">Unique id for this object.</param>
        public Ship(int id)
        {
            Id = id;

            Position = new Vector2();
            Momentum = new Vector2();
            Force = new Vector2();
            ExternalForce = new Vector2();
            ExternalMomentum = new Vector2();

            AngularMomentum = 0;
            Direction = 0;

            MaxAngularMomentum = 0;
            MaxLinearMomentum = 0;
        }

        /// <summary>
        /// Returns a reference to the current cell containing this object.
        /// </summary>
        /// <returns>Cell containing this object.</returns>
        public WorldCell GetCell()
        {
            return Cell;
        }

        /// <summary>
        /// Returns the unique id of this object.
        /// </summary>
        /// <returns>Id</returns>
        public int GetId()
        {
            return Id;
        }

        /// <summary>
        /// Returns the current position of this object.
        /// </summary>
        /// <returns>Returns point this object is at.</returns>
        public Point GetPosition()
        {
            return Position.ToPoint();
        }

        /// <summary>
        /// Sets the current position of this object.
        /// </summary>
        /// <param name="position">New position to move to.</param>
        public void SetPosition(Point position)
        {
            Position = position.ToVector2();
        }

        /// <summary>
        /// Sets the cell reference containing this object.
        /// </summary>
        /// <param name="cell">Cell containing this object.</param>
        public void SetCell(WorldCell cell)
        {
            Cell = cell;
        }

        /// <summary>
        /// Perform logic of a single from for this object.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Update(TimeSpan gameTime)
        {
            // Calculate delta time from the frame interval. (Delta time is in
            // seconds).
            float deltaTime = (float)gameTime.TotalMilliseconds / 1000;

            // Update angular momentum and direction
            AngularMomentum += Force.Y * deltaTime;
                AngularMomentum = MathHelper.Clamp(AngularMomentum,
                    -MaxAngularMomentum, MaxAngularMomentum);
            Direction += AngularMomentum * deltaTime;

            // Update momentum (apply Force.Y in direction of Direction)
            Momentum += new Vector2((float)Math.Cos(
                MathHelper.ToRadians(Direction)),
                (float)Math.Sin(MathHelper.ToRadians(Direction)))
                * Force.X * deltaTime;
            if (Momentum.Length() > MaxLinearMomentum)
            {
                Momentum.Normalize();
                Momentum *= MaxLinearMomentum;
            }

            // Update external forces (these are not limited).
            ExternalMomentum += ExternalForce * deltaTime;

            // Update position
            Position += (Momentum + ExternalMomentum) * deltaTime;
        }

        /// <summary>
        /// Serializes the replication data of this object into an outgoing
        /// message.
        /// </summary>
        /// <param name="message">Outgoing message.</param>
        public void Serialize(NetOutgoingMessage message)
        {
            // Packet:
            // Id (int)
            // Type (short)
            // ** Name (string)
            // Position (vec2)
            // Direction (float)
            // Momentum (vec2)
            // AngularMomentum (float)

            // ** not yet implemented

            message.Write((int)Id);
            message.Write((short)GameObjectType.Ship);
            message.Write(Position.X);
            message.Write(Position.Y);
            message.Write(Direction);
            message.Write(Momentum.X);
            message.Write(Momentum.Y);
            message.Write(AngularMomentum);
        }

        /// <summary>
        /// Serializes the replication data of this object into an outgoing
        /// message. Only serializes positional + physics data.
        /// </summary>
        /// <param name="message">Outgoing message.</param>
        public void SerializePosition(NetOutgoingMessage message)
        {
            message.Write((int)Id);
            message.Write(Position.X);
            message.Write(Position.Y);
            message.Write(Direction);
            message.Write(Momentum.X);
            message.Write(Momentum.Y);
            message.Write(AngularMomentum);
        }

        /// <summary>
        /// Serializes the replication data of this object into an outgoing
        /// message. Only serializes combat data.
        /// </summary>
        /// <param name="message">Outgoing message.</param>
        public void SerializeCombat(NetOutgoingMessage message)
        {

        }

        /// <summary>
        /// Serializes the replication data of this object into an outgoing
        /// message. Only serializes graphic model data.
        /// </summary>
        /// <param name="message">Outgoing message.</param>
        public void SerializeModel(NetOutgoingMessage message)
        {

        }
    }
}
