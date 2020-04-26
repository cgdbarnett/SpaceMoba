using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Resource System.
        public static readonly ComponentSystemId ResourceSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Resources
{
    /// <summary>
    /// Resource system.
    /// </summary>
    public class ResourceSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a ResourceSystem.
        /// </summary>
        public ResourceSystem()
        {
            Id = ComponentSystemId.ResourceSystem;
            WantsUpdates = false;
        }
    }
}
