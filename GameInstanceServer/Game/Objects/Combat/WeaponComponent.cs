using System;

using Lidgren.Network;

using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Combat
{
    public class WeaponComponent : IComponent
    {
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
