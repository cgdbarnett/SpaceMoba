using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Resource System.
        public static readonly ComponentSystemId CombatSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Combat
{
    /// <summary>
    /// Combat system.
    /// </summary>
    public class CombatSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a CombatSystem.
        /// </summary>
        public CombatSystem()
        {
            Id = ComponentSystemId.CombatSystem;
            WantsUpdates = false;
        }
    }
}