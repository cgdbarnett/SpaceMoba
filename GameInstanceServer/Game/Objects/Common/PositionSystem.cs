using System;
using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Position System.
        public static readonly ComponentSystemId PositionSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Common
{
    /// <summary>
    /// Position system.
    /// </summary>
    public class PositionSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a PositionSystem.
        /// </summary>
        public PositionSystem()
        {
            Id = ComponentSystemId.PositionSystem;
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
            float delta = (float)gameTime.TotalSeconds;

            // Update position
            PositionComponent position = (PositionComponent)component;
            position.Position += position.Momentum * delta;

            // Update direction
            position.Direction += position.AngularMomentum * delta;
        }
    }
}
