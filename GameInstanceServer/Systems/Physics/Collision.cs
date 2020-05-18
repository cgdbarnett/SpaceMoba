using System;

using Microsoft.Xna.Framework;

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

        /// <summary>
        /// Tests a collision between two circles.
        /// </summary>
        /// <param name="circleA">First circle mask to test.</param>
        /// <param name="circleB">Second circle mask to test.</param>
        /// <returns>Whether the two circles intersect.</returns>
        public static bool TestCollisionCircleCircle(
            CollisionMaskCircle circleA, CollisionMaskCircle circleB
            )
        {
            // Check distance between center of both circles is less
            // than or equal to their combined radius.
            float distanceSqr = 
                (circleA.Center - circleB.Center).LengthSquared();
            float radiusSqr =
                (circleA.Radius + circleB.Radius) * 
                (circleA.Radius + circleB.Radius);

            return distanceSqr <= radiusSqr;
        }

        /// <summary>
        /// Tests a collision between a circle and line mask.
        /// </summary>
        /// <param name="circle">Circle mask to test.</param>
        /// <param name="line">Line mask to test.</param>
        /// <returns>Whether the line intersects the circle.</returns>
        public static bool TestCollisionLineCircle(
            CollisionMaskCircle circle, CollisionMaskLine line
            )
        {
            // Early exit if end points of line are inside circle.
            if(TestPointInCircle(line.Start, circle) || 
                TestPointInCircle(line.End, circle)
                )
            {
                return true;
            }

            // Constants
            float constA = 
                (line.End.X - line.Start.X) * (line.Start.X - line.End.X);
            float constB = 
                (line.Start.Y - line.End.Y) * (line.End.Y - line.Start.Y);
            float constC = 
                (line.Start.Y - line.End.Y) * (line.End.X - line.Start.X);

            // Get x point
            float pointX = (
                constC * line.Start.Y - constC * circle.Center.Y + constA *
                circle.Center.X - constB * line.Start.X) / (constA - constB);

            // Only test if point is on line segment.
            if (
                (pointX >= line.Start.X && pointX <= line.End.X) ||
                (pointX >= line.End.X && pointX <= line.Start.X)
                )
            {

                // Create vector from point on line
                Vector2 tangentLineIntersection = new Vector2(
                    pointX,
                    (line.End.Y - line.Start.Y) * (pointX - line.Start.X)
                    / (line.End.X - line.Start.X) + line.Start.Y
                    );

                // Is distance between tangent intersection and circle center
                // less than radius?
                float distance =
                    (circle.Center - tangentLineIntersection).LengthSquared();

                return distance <= circle.Radius * circle.Radius;
            }
            return false;
        }

        /// <summary>
        /// Returns whether a point is inside a circle.
        /// </summary>
        /// <param name="point">Point to test.</param>
        /// <param name="circle">Circle to test.</param>
        /// <returns>Whether point is inside circle.</returns>
        private static bool TestPointInCircle(
            Vector2 point, CollisionMaskCircle circle
            )
        {
            return (point - circle.Center).LengthSquared() <= 
                circle.Radius * circle.Radius;
        }
    }
}
