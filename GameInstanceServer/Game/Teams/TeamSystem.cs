using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Team System.
        public static readonly ComponentSystemId TeamSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Teams
{
    public class TeamSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a TeamSystem..
        /// </summary>
        public TeamSystem()
        {
            Id = ComponentSystemId.TeamSystem;
            WantsUpdates = false;
        }
    }
}
