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
        public Vector2 Start
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
        /// Length of line segment.
        /// </summary>
        public float Length;

        /// <summary>
        /// Second point of the line.
        /// </summary>
        public Vector2 End
        {
            get
            {
                return Position + Length * new Vector2(
                    (float)Math.Cos(MathHelper.ToRadians(Direction)),
                    (float)Math.Sin(MathHelper.ToRadians(Direction))
                    );
            }
            set
            {
                Length = (value - Position).Length();
                Direction = 
                    MathHelper.ToDegrees((float)Math.Atan2(value.Y, value.X));
            }
        }

        /// <summary>
        /// Creates a new instance of a CollisionMaskLine.
        /// </summary>
        /// <param name="start">First point of line.</param>
        /// <param name="end">Second point of line.</param>
        public CollisionMaskLine(Vector2 start, float length, float direction)
        {
            Position = start;
            Length = length;
            Direction = direction;
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
