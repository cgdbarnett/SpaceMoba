using System;

using Lidgren.Network;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Ships
{
    /// <summary>
    /// Limits the maximum speeds of a ship.
    /// </summary>
    public class ShipLimiterComponent : IComponent
    {
        // Stats of ships.
        public static readonly int[] AngularAcceleration =
        {
            180, 240, 300, 360
        };

        public static readonly int[] AngularMaximum =
        {
            90, 120, 150, 180
        };

        public static readonly int[] SideAcceleration =
        {
            0, 90, 150, 180
        };

        public static readonly int[] FowardAcceleration =
        {
            200, 300, 400, 500
        };

        public static readonly int[] LinearMaximum =
        {
            200, 250, 300, 350
        };

        /// <summary>
        /// This is a serializable component.
        /// </summary>
        public bool Serializable => false;

        /// <summary>
        /// Id of ship limiter system.
        /// </summary>
        public ComponentSystemId ComponentSystem
            => ComponentSystemId.ShipLimiterSystem;

        /// <summary>
        /// Position component of entity.
        /// </summary>
        public PositionComponent Position;

        /// <summary>
        /// Engine component of entity.
        /// </summary>
        public EngineComponent Engine;

        // Ranks
        public byte HandlingRank, SpeedRank;

        /// <summary>
        /// Serialize this component into an outgoing message.
        /// </summary>
        /// <param name="msg"></param>
        public void Serialize(NetOutgoingMessage msg)
            => throw (new NotImplementedException());
    }
}
