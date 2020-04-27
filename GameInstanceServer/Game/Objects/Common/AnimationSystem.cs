using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Position System.
        public static readonly ComponentSystemId AnimationSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Common
{
    /// <summary>
    /// Animation of game objects.
    /// </summary>
    public class AnimationSystem : ComponentSystem 
    {
        /// <summary>
        /// Creates a AnimationSystem.
        /// </summary>
        public AnimationSystem()
        {
            Id = ComponentSystemId.AnimationSystem;
            WantsUpdates = false;
        }

        // Possible extension to system to work in actual animations
        // when the time comes.
    }
}
