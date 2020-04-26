using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Common
{
    public class AffectedByBlackholeComponent : IComponent
    {
        // Positional and blackhole fields.
        public PositionComponent Position;

        /// <summary>
        /// Gets the ComponentSystem for AffectedByBlackholeComponent.
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
