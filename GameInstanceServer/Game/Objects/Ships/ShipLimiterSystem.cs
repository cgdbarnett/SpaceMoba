using System;
using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for ShipLimiter System.
        public static readonly ComponentSystemId ShipLimiterSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Ships
{
    /// <summary>
    /// Limits the speed of certain game objects.
    /// </summary>
    public class ShipLimiterSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a ShipLimiterSystem.
        /// </summary>
        public ShipLimiterSystem()
        {
            Id = ComponentSystemId.ShipLimiterSystem;
            WantsUpdates = true;
        }

        /// <summary>
        /// Updates the ShipLimiter system.
        /// </summary>
        /// <param name="component">ShipLimiterComponent.</param>
        /// <param name="gameTime">Game frame interval.</param>
        protected override void UpdateComponent(
            IComponent component, TimeSpan gameTime
            )
        {
            ShipLimiterComponent limiter = (ShipLimiterComponent)component;

            // Limit linear momentum
            if(limiter.Position.Momentum.LengthSquared() > 
                Math.Pow(
                    ShipLimiterComponent.LinearMaximum[limiter.SpeedRank], 2
                    )
                )
            {
                limiter.Position.Momentum.Normalize();
                limiter.Position.Momentum *= 
                    ShipLimiterComponent.LinearMaximum[limiter.SpeedRank];
            }

            // Limit angular momentum
            if(Math.Abs(limiter.Position.AngularMomentum) > 
                ShipLimiterComponent.AngularAcceleration[limiter.HandlingRank]
                )
            {
                limiter.Position.AngularMomentum = 
                    Math.Sign(limiter.Position.AngularMomentum) * 
                    ShipLimiterComponent.AngularAcceleration[
                        limiter.HandlingRank
                        ];
            }

            // Set Accelerations
            limiter.Engine.EngineForce.X = 
                ShipLimiterComponent.FowardAcceleration[limiter.SpeedRank];
            limiter.Engine.EngineForce.Y =
                ShipLimiterComponent.SideAcceleration[limiter.HandlingRank];
            limiter.Engine.EngineAngularForce =
                ShipLimiterComponent.AngularAcceleration[limiter.HandlingRank];
        }
    }
}
