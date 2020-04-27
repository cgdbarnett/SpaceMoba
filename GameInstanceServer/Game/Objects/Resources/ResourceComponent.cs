using System;

using Lidgren.Network;

using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Resources
{
    /// <summary>
    /// Resource component.
    /// </summary>
    public class ResourceComponent : IComponent
    {
        /// <summary>
        /// Gets the ComponentSystem for Resources.
        /// </summary>
        public ComponentSystemId ComponentSystem
        {
            get
            {
                return ComponentSystemId.ResourceSystem;
            }
        }

        /// <summary>
        /// Resources this object is worth / carrying.
        /// </summary>
        public int Resources
        {
            get;
            protected set;
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
        /// <param name="msg"></param>
        public void Serialize(NetOutgoingMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
