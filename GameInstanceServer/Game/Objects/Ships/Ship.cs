// System libraries
using System;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework;

// Lidgren libraries
using Lidgren.Network;

// Game libraries
using GameInstanceServer.Game.World;
using GameInstanceServer.Game.Objects.Combat;

namespace GameInstanceServer.Game.Objects.Ships
{
    /// <summary>
    /// Base ship class. Handles physics.
    /// </summary>
    public class Ship : CollidableObject, IGameObject, ICombatObject
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

        // Combat
        protected int Health, MaxHealth;
        protected int Shield, MaxShield;
        protected int WeaponCooldown;

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

            BoundingBox = new Rectangle(
                new Point((int)Position.X - 32, (int)Position.Y - 32),
                new Point(64, 64)
                );

            // Todo(Remove magic numbers)
            MaxAngularMomentum = 180;
            MaxLinearMomentum = 400;
            Health = 100;
            MaxHealth = 100;
            Shield = 100;
            MaxShield = 100;
            WeaponCooldown = 0;
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
        /// Sets the force being applied to this ship.
        /// </summary>
        /// <param name="force">Force to apply. Force is applied along the
        /// axis of the ship in the direction Direction. X = forward, Y = 
        /// Right.</param>
        public void SetForce(Vector2 force)
        {
            Force.X = force.X;
            Force.Y = force.Y;
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
            WeaponCooldown -= (int)gameTime.TotalMilliseconds;

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
            BoundingBox.Location = Position.ToPoint() - new Point(32, 32);
        }

        #region Serialization
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

        #endregion

        #region Combat

        /// <summary>
        /// Returns health of ship.
        /// </summary>
        /// <returns>Health.</returns>
        public int GetHealth()
        {
            return Health;
        }

        /// <summary>
        /// Returns max health of ship.
        /// </summary>
        /// <returns>Max health.</returns>
        public int GetMaxHealth()
        {
            return MaxHealth;
        }

        /// <summary>
        /// Returns shield value of ship.
        /// </summary>
        /// <returns>Shield.</returns>
        public int GetShield()
        {
            return Shield;
        }

        /// <summary>
        /// Returns max shield value of ship.
        /// </summary>
        /// <returns>Shield.</returns>
        public int GetMaxShield()
        {
            return MaxShield;
        }

        /// <summary>
        /// Applies damage to the ship.
        /// </summary>
        /// <param name="damage">Damage to apply.</param>
        public void ApplyDamage(int damage)
        {
            if(Shield > 0)
            {
                Shield -= damage;
            }

            if(Shield < 0)
            {
                Health += Shield;
                Shield = 0;
            }

            // Death event?!!
            if(Health < 0)
            {
                HandleDeath();
            }
        }

        /// <summary>
        /// Spawns a bullet in direction facing if off cooldown.
        /// </summary>
        public void Attack()
        {
            if(WeaponCooldown <= 0)
            {
                Vector2 spawn = new Vector2(
                    (float)Math.Cos(MathHelper.ToRadians(Direction)),
                    (float)Math.Sin(MathHelper.ToRadians(Direction))
                    ) * 50;

                /*Bullet bullet = new Bullet(
                    GameSimulation.GetGameSimulation().CreateNewUniqueId(),
                    GetPosition() + spawn.ToPoint(), 
                    400, Direction, 40
                    );
                GameSimulation.GetGameSimulation().AddObject(bullet);*/

                WeaponCooldown = 500;
            }
        }

        /// <summary>
        /// Handles the death of this ship.
        /// </summary>
        protected void HandleDeath()
        {
            // Destroy!
            //GameSimulation.GetGameSimulation().RemoveObject(this);
        }

        #endregion
    }
}
