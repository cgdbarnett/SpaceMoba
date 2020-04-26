using Microsoft.Xna.Framework;

using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Game.World;

namespace GameInstanceServer.Game.Objects.Common
{
    /// <summary>
    /// Position component.
    /// </summary>
    public class PositionComponent : IComponent
    {
        // Positional data.
        public Vector2 Position;
        public Vector2 Momentum;
        public WorldCell Cell;

        /// <summary>
        /// Gets the ComponentSystem for Positioned objects.
        /// </summary>
        public ComponentSystemId ComponentSystem
        {
            get
            {
                return ComponentSystemId.PositionSystem;
            }
        }
    }
}
