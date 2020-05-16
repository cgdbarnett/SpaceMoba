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
        public Vector2 Center;

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
            Center = center;
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
            if(other.GetType() == typeof(CollisionMaskCircle))
            {
                CollisionMaskCircle mask = (CollisionMaskCircle)other;

                // Check distance between center of both circles is less
                // than or equal to their combined radius.
                float distanceSqr = (Center - mask.Center).LengthSquared();
                float radiusSqr = 
                    (Radius + mask.Radius) * (Radius + mask.Radius);

                return distanceSqr <= radiusSqr;
            }
            else
            {
                throw (new NotImplementedException());
            }
        }
    }
}
