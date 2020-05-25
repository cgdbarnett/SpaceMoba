using System;

using Lidgren.Network;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Game.Teams;
using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Combat
{
    /// <summary>
    /// A base weapon component.
    /// </summary>
    public class WeaponComponent : IComponent
    {
        /// <summary>
        /// Flags whether the trigger is currently active.
        /// </summary>
        public bool Trigger;

        /// <summary>
        /// Current cooldown remaining on weapon.
        /// </summary>
        public int CurrentCooldown;

        /// <summary>
        /// Length of cooldown.
        /// </summary>
        public int Cooldown;

        /// <summary>
        /// Direction weapon is currently facing.
        /// </summary>
        public float Direction;

        /// <summary>
        /// Reference to the parent entity.
        /// </summary>
        public Entity Parent = null;

        /// <summary>
        /// Position component of parent entity.
        /// </summary>
        public PositionComponent Position;

        /// <summary>
        /// Team component of parent entity.
        /// </summary>
        public TeamComponent Team = null;

        /// <summary>
        /// Gets the ComponentSystem for Weapons.
        /// </summary>
        public ComponentSystemId ComponentSystem
        {
            get
            {
                return ComponentSystemId.WeaponSystem;
            }
        }

        /// <summary>
        /// This is not a serializable component.
        /// </summary>
        public bool Serializable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// This is not a serializable component.
        /// </summary>
        /// <param name="msg">Outgoing message.</param>
        public void Serialize(NetOutgoingMessage msg)
        {
            throw (new NotImplementedException());
        }
    }
}
