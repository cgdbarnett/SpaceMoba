using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Systems.ECS;
using Lidgren.Network;

namespace GameInstanceServer.Game.World
{
    public class WorldComponent : IComponent
    {
        public PositionComponent PositionComponent;
        public Entity Entity;

        public Vector2 Position
        {
            get
            {
                return PositionComponent.Position;
            }
        }

        public WorldCell Cell;

        /// <summary>
        /// Gets the id of the component system for WorldComponents.
        /// </summary>
        public ComponentSystemId ComponentSystem
        {
            get
            {
                return ComponentSystemId.WorldSystem;
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
        /// <param name="msg"></param>
        public void Serialize(NetOutgoingMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
