using System;
using System.Collections.Generic;
using System.Text;

namespace GameInstanceServer.Systems.Physics
{
    /// <summary>
    /// Provides collision checking between two components.
    /// </summary>
    public static class Collision
    {
        /// <summary>
        /// Tests a collsion between two masks.
        /// </summary>
        /// <param name="first">First mask to test.</param>
        /// <param name="second">Second mask to test.</param>
        /// <returns>Whether the two masks are colliding.</returns>
        public static bool TestCollision(
            CollisionMask first, CollisionMask second
            )
        {
            return first.TestCollision(second);
        }
    }
}
