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
        public static readonly ComponentSystemId EngineSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Common
{
    /// <summary>
    /// The EngineSystem provides force to the PositionSystem to enable
    /// controlled movement of a ship.
    /// </summary>
    public class EngineSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a new EngineSystem.
        /// </summary>
        public EngineSystem()
        {
            Id = ComponentSystemId.EngineSystem;
            WantsUpdates = true;
        }

        /// <summary>
        /// Update the force applied to the PositionComponent.
        /// </summary>
        /// <param name="component">Component to update.</param>
        /// <param name="gameTime">Game frame interval.</param>
        protected override void UpdateComponent(
            IComponent component, TimeSpan gameTime
            )
        {
            // Calculate delta (in seconds)
            float delta = (float)gameTime.TotalSeconds;

            // Update momentum
            EngineComponent engine = (EngineComponent)component;
            
            engine.Position.AngularMomentum += engine.InputAngularForce * delta;
            engine.Position.Momentum += engine.Force * delta;
        }
    }
}
