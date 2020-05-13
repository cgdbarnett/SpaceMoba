using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Resource System.
        public static readonly ComponentSystemId WeaponSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Combat
{
    /// <summary>
    /// Weapon system.
    /// </summary>
    public class WeaponSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a WeaponSystem.
        /// </summary>
        public WeaponSystem()
        {
            Id = ComponentSystemId.WeaponSystem;
            WantsUpdates = false;
        }
    }
}