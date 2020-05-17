using System;

using Microsoft.Xna.Framework;

namespace GameInstanceServer.Systems.Physics
{
    /// <summary>
    /// Provides collision detection for a line.
    /// </summary>
    public class CollisionMaskLine : CollisionMask
    {
        /// <summary>
        /// First point of the line.
        /// </summary>
        public Vector2 Start;

        /// <summary>
        /// Second point of the line.
        /// </summary>
        public Vector2 End;

        /// <summary>
        /// Creates a new instance of a CollisionMaskLine.
        /// </summary>
        /// <param name="start">First point of line.</param>
        /// <param name="end">Second point of line.</param>
        public CollisionMaskLine(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
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

                return Collision.TestCollisionLineCircle(mask, this);
            }
            else if(other.GetType() == typeof(CollisionMaskLine))
            {
                throw (new NotImplementedException());
            }
            else
            {
                throw (new NotImplementedException());
            }
        }
    }
}
