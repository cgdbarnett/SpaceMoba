using System;

using Microsoft.Xna.Framework;

namespace GameInstanceServer.Systems.Physics
{
    /// <summary>
    /// Provides collision detection for a circle.
    /// </summary>
    public class CollisionMaskCircle : CollisionMask
    {
        /// <summary>
        /// Center of collision mask.
        /// </summary>
        public Vector2 Center
        {
            get
            {
                return Position;
            }
            set
            {
                Position = value;
            }
        }

        /// <summary>
        /// Radius of collision mask.
        /// </summary>
        public float Radius;

        /// <summary>
        /// Creates a new instance of a CollisionMaskCircle.
        /// </summary>
        /// <param name="center">Initial center point.</param>
        /// <param name="radius">Initial radius.</param>
        public CollisionMaskCircle(Vector2 center, float radius)
        {
            Position = center;
            Radius = radius;
        }
        
        /// <summary>
        /// Tests for a collision with another CollisionMask.
        /// </summary>
        /// <param name="other">Other mask to test with.</param>
        /// <returns>Returns whether the two masks are colliding.</returns>
        public override bool TestCollision(CollisionMask other)
        {
            // Circle circle collision
            if (other.GetType() == typeof(CollisionMaskCircle))
            {
                CollisionMaskCircle mask = (CollisionMaskCircle)other;

                return Collision.TestCollisionCircleCircle(this, mask);
            }
            else if(other.GetType() == typeof(CollisionMaskLine))
            {
                CollisionMaskLine mask = (CollisionMaskLine)other;

                return Collision.TestCollisionLineCircle(this, mask);
            }
            else
            {
                throw (new NotImplementedException());
            }
        }
    }
}
