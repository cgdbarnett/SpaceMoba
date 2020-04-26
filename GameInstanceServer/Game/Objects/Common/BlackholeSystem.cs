using System;

using Microsoft.Xna.Framework;

using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Position System.
        public static readonly ComponentSystemId BlackholeSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Common
{
    /// <summary>
    /// Resource system.
    /// </summary>
    public class BlachholeSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a BlackholeSystem.
        /// </summary>
        public BlachholeSystem()
        {
            Id = ComponentSystemId.BlackholeSystem;
            WantsUpdates = true;
        }

        /// <summary>
        /// Update the position of the component.
        /// </summary>
        /// <param name="component">Component to update.</param>
        /// <param name="gameTime">Game frame interval.</param>
        protected override void UpdateComponent(
            IComponent component, TimeSpan gameTime
            )
        {
            // Calculate delta (in seconds)
            float delta = (float)gameTime.TotalMilliseconds / 1000;

            // Get position
            AffectedByBlackholeComponent blackhole = 
                (AffectedByBlackholeComponent)component;
            PositionComponent position = blackhole.Position;

            // Get decay, and gravity vector
            float decay = Blackhole.GetDecay(position.Position);
            Vector2 gravity = Blackhole.GetMomentum(position.Position);

            // Apply decay to momentums, and then apply gravity
            position.Momentum -= position.Momentum * decay;
            gravity -= gravity * decay;

            position.Position += gravity;
        }
    }
}
