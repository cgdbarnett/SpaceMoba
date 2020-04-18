using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMobaClient.GamePlay.Objects
{
    /// <summary>
    /// Represents a Ship object. To be used for both local and remote players.
    /// </summary>
    public class Ship : IGameObject
    {
        private readonly int Id;

        // Physics and positional information of ship
        private Vector2 Position;
        private Vector2 Momentum;
        private Vector2 Force;
        private float AngularMomentum;
        private float Direction;

        // Sprite
        private Texture2D Sprite;

        /// <summary>
        /// Represents a Ship object. To be used for both local and remote players.
        /// </summary>
        /// <param name="spawn">Vector2 representing spawn location.</param>
        /// <param name="direction">Float representing initial rotation.</param>
        public Ship(int id, Texture2D sprite, Vector2 spawn, float direction)
        {
            Id = id;

            // Create new vector from reference
            Position = new Vector2(spawn.X, spawn.Y);

            // Initialise physics to zero state
            Momentum = new Vector2();
            Force = new Vector2();
            AngularMomentum = 0;

            // Copy direction
            Direction = direction;

            // Load sprite
            Sprite = sprite;
        }

        /// <summary>
        /// Returns the unique identifier of this object.
        /// </summary>
        /// <returns>Id of object.</returns>
        public int GetId()
        {
            return Id;
        }

        /// <summary>
        /// Returns a new vector2 representing the objects current position.
        /// </summary>
        /// <returns>Vector2 of current position.</returns>
        public Vector2 GetPosition()
        {
            return new Vector2(Position.X, Position.Y);
        }

        /// <summary>
        /// Returns the direction this ship is facing.
        /// </summary>
        /// <returns>Direction.</returns>
        public float GetDirection()
        {
            return Direction;
        }

        /// <summary>
        /// Returns a new vector2 representing the objects current momentum.
        /// </summary>
        /// <returns>Vector2 of current momentum.</returns>
        public Vector2 GetMomentum()
        {
            return new Vector2(Momentum.X, Momentum.Y);
        }

        /// <summary>
        /// Returns the angular momentum of this ship.
        /// </summary>
        /// <returns>Angular momentum.</returns>
        public float GetAngularMomentum()
        {
            return AngularMomentum;
        }

        /// <summary>
        /// Returns whether to draw this object.
        /// </summary>
        /// <returns>Returns visibility.</returns>
        public bool IsVisible()
        {
            return true;
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
        /// Sets the position of this ship.
        /// </summary>
        /// <param name="position">Position to move to.</param>
        public void SetPosition(Vector2 position)
        {
            Position.X = position.X;
            Position.Y = position.Y;
        }

        /// <summary>
        /// Sets the angular momentum of this ship.
        /// </summary>
        /// <param name="ang">New angular momentum.</param>
        public void SetAngularMomentum(float ang)
        {
            AngularMomentum = ang;
        }

        /// <summary>
        /// Sets the direction this ship is facing.
        /// </summary>
        /// <param name="dir">New direction to face.</param>
        public void SetDirection(float dir)
        {
            Direction = dir;
        }

        /// <summary>
        /// Sets the momentum this ship is under.
        /// </summary>
        /// <param name="momentum">Momentum of ship.</param>
        public void SetMomentum(Vector2 momentum)
        {
            Momentum.X = momentum.X;
            Momentum.Y = momentum.Y;
        }

        /// <summary>
        /// Draws this ship object to screen.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            // WELCOME TO HIDEOUSNESS
            spriteBatch.Draw(Sprite, 
                new Rectangle(
                    (int)Position.X + camera.OffsetX, (int)Position.Y + camera.OffsetY, 
                    Sprite.Width, Sprite.Height
                    ),
                Sprite.Bounds, Color.White, 
                MathHelper.ToRadians(Direction), 
                new Vector2(Sprite.Width / 2, Sprite.Height / 2),
                SpriteEffects.None, 0);
        }

        /// <summary>
        /// Executes a single step of execution for the ship object.
        /// </summary>
        /// <param name="gameTime">Frame interval</param>
        public void Update(GameTime gameTime)
        {
            // Calculate delta time from the frame interval. (Delta time is in
            // seconds).
            float deltaTime = (float)gameTime.ElapsedGameTime.Milliseconds / (float)1000;

            // Update physics
            const float MaxAngularMomentum = 180; // (Degrees / second)
            const float MinAngularMomentum = 1;
            const float MaxMomentum = 400; // (Pixels / second)
            const float MinMomentum = 3;

            // Update angular momentum and direction
            AngularMomentum += Force.Y * deltaTime;
            if (Math.Abs(AngularMomentum) < MinAngularMomentum)
            {
                AngularMomentum = 0;
            }
            else
            {
                AngularMomentum = MathHelper.Clamp(AngularMomentum,
                    -MaxAngularMomentum, MaxAngularMomentum);
            }
            Direction += AngularMomentum * deltaTime;

            // Update momentum (apply Force.Y in direction of Direction)
            Momentum += new Vector2((float)Math.Cos(
                MathHelper.ToRadians(Direction)),
                (float)Math.Sin(MathHelper.ToRadians(Direction))) 
                * Force.X * deltaTime;
            if(Momentum.Length() < MinMomentum)
            {
                Momentum = Vector2.Zero;
            }
            else if(Momentum.Length() > MaxMomentum)
            {
                Momentum.Normalize();
                Momentum *= MaxMomentum;
            }

            // Update position
            Position += Momentum * deltaTime;
        }
    }
}
