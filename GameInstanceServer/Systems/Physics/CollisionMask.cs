namespace GameInstanceServer.Systems.Physics
{
    /// <summary>
    /// An abstract class that collidable entities use to
    /// represent their collidable shape.
    /// </summary>
    public abstract class CollisionMask
    {
        /// <summary>
        /// Implemented by child classes, returns a bool with
        /// whether this collision mask is colliding with another.
        /// </summary>
        /// <param name="other">Other collision mask.</param>
        /// <returns>Whether collision is occuring.</returns>
        public abstract bool TestCollision(CollisionMask other);
    }
}
