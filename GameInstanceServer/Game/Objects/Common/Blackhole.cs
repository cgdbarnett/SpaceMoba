using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using Lidgren.Network;

using GameInstanceServer.Game.World;

namespace GameInstanceServer.Game.Objects.Common
{
    public class Blackhole : CollidableObject, IGameObject
    {
        /// <summary>
        /// Position of the centre of the black hole.
        /// </summary>
        public static Vector2 Position
        {
            get;
            private set;
        }

        /// <summary>
        /// A combined mass and gravity constant value for calculating
        /// gravity of blackhole.
        /// </summary>
        private const float GravityConstant = 50000000f;

        // Object Identifier
        private readonly int Id;

        // Cell object is located in
        private WorldCell Cell;

        /// <summary>
        /// Creates a blachole object.
        /// </summary>
        /// <param name="id">Unique identifier for object.</param>
        /// <param name="position">Spawn position of object.</param>
        public Blackhole(int id, Vector2 position)
        {
            Id = id;
            Position = new Vector2(position.X, position.Y);

            BoundingBox = new Rectangle(
                new Point((int)position.X - 400, (int)position.Y - 400),
                new Point(800, 800)
                );
        }

        /// <summary>
        /// Returns a float that represents how much momentum an orbitting
        /// object should decay per step.
        /// </summary>
        /// <param name="position">Position of object.</param>
        /// <returns>Decay ratio.</returns>
        public static float GetDecay(Vector2 position)
        {
            if(Position == null)
            {
                throw (new InvalidOperationException());
            }

            // Decay ratios
            const float farDecay = 0.000001f; // Decay from far away.
            const float midDecay = 0.0000015f; // Decay from mid point.
            const float nearDecay = 0.00005f; // Decay from nearby point.
            const float insideDecay = 0.005f; // Decay inside "atmosphere."

            // Boundary distances (all squared)
            const float insideBoundary = 1300f * 1300f;
            const float midBoundary = 3000f * 3000f;
            const float maxBoundary = 4400f * 4400f;

            // Note: objects can be further than max boundary, but the decay
            // ratio caps at that point.
            float distance = (position - Position).LengthSquared();
            if(distance < insideBoundary)
            {
                return MathHelper.Lerp(
                    nearDecay, insideDecay, 
                    MathHelper.Clamp(1 - distance / insideBoundary, 0f, 1f)
                    );
            }
            else if(distance < midBoundary)
            {
                return MathHelper.Lerp(
                    midDecay, nearDecay,
                    1 - (distance - insideBoundary) /
                    (midBoundary - insideBoundary)
                    );
            }
            else
            {
                return MathHelper.Lerp(
                    farDecay, midDecay,
                    MathHelper.Clamp(
                        1 - (distance - midBoundary) / 
                        (maxBoundary - midBoundary),
                        0, 1
                        )
                        );
            }
        }

        /// <summary>
        /// Returns the momentum an object needs to enter orbit of this
        /// blackhole from a given position.
        /// </summary>
        /// <param name="position">Centre of object.</param>
        /// <returns>Initial momentum.</returns>
        public static Vector2 GetInitialMomentum(Vector2 position)
        {
            if (Position == null)
            {
                throw (new InvalidOperationException());
            }

            // Get speed, 110px/s at 4400 px, and 2px/s per 150px.
            float distance = (position - Position).Length();
            float momentum = 110f + 2f / 150f * (4400f - distance);

            // Get direction tangent
            float direction = (float)Math.Atan2(
                Position.Y - position.Y, Position.X - position.X
                ) + (float)(Math.PI / 4);

            // Convert polar to cartesian
            return distance * new Vector2(
                (float)Math.Cos(direction), (float)Math.Sin(direction)
                );
        }

        /// <summary>
        /// Returns the momentum to apply to an object due to the blackhole's
        /// gravity.
        /// </summary>
        /// <param name="position">Centre of object.</param>
        /// <returns>Gravity momentum.</returns>
        public static Vector2 GetMomentum(Vector2 position)
        {
            // Equation is constant / distance sqr'd,
            // in the direction of the blackhole.
            float distance = (position - Position).LengthSquared();
            float force = GravityConstant / distance;
            Vector2 direction = position - Position;
            direction.Normalize();

            return direction * force;
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

            // ** not yet implemented

            message.Write((int)Id);
            message.Write((short)GameObjectType.Blackhole);
            message.Write(Position.X);
            message.Write(Position.Y);
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
        /// Apply gravity as external momentum to all IAffectedByBlackhole
        /// objects in the game.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Update(TimeSpan gameTime)
        {
            // Calculate delta time from the frame interval. (Delta time is in
            // seconds).
            float deltaTime = (float)gameTime.TotalMilliseconds / 1000;
        }
    }
}
