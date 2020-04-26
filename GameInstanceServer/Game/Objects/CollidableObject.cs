// XNA (Monogame) libraries
using Microsoft.Xna.Framework;

namespace GameInstanceServer.Game.Objects
{
    /// <summary>
    /// Used for objects that 
    /// </summary>
    public partial class CollidableObject
    {
        /// <summary>
        /// BoundingBox of collidable object.
        /// </summary>
        public Rectangle BoundingBox;


        /// <summary>
        /// Returns whether there is a collision between this and another
        /// collidable object.
        /// </summary>
        /// <param name="obj">Object to check for collision.</param>
        /// <returns>Whether there is a collision.</returns>
        public bool Intersects(CollidableObject obj)
        {
            // Note: AABB collision isn't very nice, this will eventually
            // have to be upgrade significantly.
            if(obj.BoundingBox.Intersects(BoundingBox))
            {
                return true;
            }
            return false;
        }
    }
}
