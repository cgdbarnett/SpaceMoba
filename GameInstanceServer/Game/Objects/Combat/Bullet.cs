using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using GameInstanceServer.Game.World;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace GameInstanceServer.Game.Objects.Combat
{
    public class Bullet : CollidableObject, IGameObject
    {
        protected readonly int Id;
        protected WorldCell Cell;

        // Physics
        protected Vector2 Position;
        protected Vector2 Momentum;
        protected Vector2 ExternalForce;
        protected Vector2 ExternalMomentum;
        protected float Direction;

        protected int LifeTime;
        protected const int MaxLifeTime = 5000;
        protected int Damage;

        /// <summary>
        /// Creates a new bullet at given location with a given speed,
        /// that will deal a given damage when it collides with any
        /// ICombatObject.
        /// </summary>
        /// <param name="id">Unique Id.</param>
        /// <param name="spawn">Spawn location.</param>
        /// <param name="speed">Speed.</param>
        /// <param name="direction">Direction.</param>
        /// <param name="damage">Damage to deal.</param>
        public Bullet(int id, Point spawn, float speed, 
            float direction, int damage)
        {
            Id = id;

            Position = new Vector2(spawn.X, spawn.Y);
            Momentum = new Vector2();
            Momentum.X = (float)Math.Cos(MathHelper.ToRadians(direction));
            Momentum.Y = (float)Math.Sin(MathHelper.ToRadians(direction));
            Momentum *= speed;
            
            ExternalForce = new Vector2();
            ExternalMomentum = new Vector2();

            LifeTime = 0;
            Damage = damage;

            BoundingBox = new Rectangle(
                new Point(spawn.X - 1, spawn.Y - 1),
                new Point(3, 3)
                );
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
        /// Serializes this object into a packet.
        /// </summary>
        /// <param name="message">Outgoing message.</param>
        public void Serialize(NetOutgoingMessage message)
        {
            // Packet:
            // Id (int)
            // Type (short)
            // Position (vec2)
            // Direction (float)
            // Momentum (vec2)

            // ** not yet implemented

            message.Write((int)Id);
            message.Write((short)GameObjectType.Bullet);
            message.Write(Position.X);
            message.Write(Position.Y);
            message.Write(Direction);
            message.Write(Momentum.X);
            message.Write(Momentum.Y);
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

            // Increment lifetime
            LifeTime += (int)gameTime.TotalMilliseconds;

            // Update external forces (these are not limited).
            ExternalMomentum += ExternalForce * deltaTime;

            // Update position
            Position += (Momentum + ExternalMomentum) * deltaTime;
            BoundingBox.Location = Position.ToPoint() - new Point(1, 1);

            // Check for out of bounds, or expired life
            if(Position.X < 0 || Position.Y < 0 || 
                Position.X >= 10000 || Position.Y >= 10000
                || LifeTime > MaxLifeTime)
            {
                HandleDeath();
            }
            else
            {
                // Check for collision at new position
                List<IGameObject> objects =
                    GameSimulation.GetGameSimulation().
                    GetCollidingObjects(this);

                foreach(IGameObject obj in objects)
                {
                    if(obj is ICombatObject)
                    {
                        ((ICombatObject)obj).ApplyDamage(Damage);
                    }
                }

                // Destroy this bullet
                if(objects.Count > 0)
                {
                    HandleDeath();
                }
            }
        }

        /// <summary>
        /// Handles the death of this object.
        /// </summary>
        private void HandleDeath()
        {
            GameSimulation.GetGameSimulation().RemoveObject(this);
        }
    }
}
