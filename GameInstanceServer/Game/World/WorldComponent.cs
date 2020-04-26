using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.World
{
    public class WorldComponent : IComponent
    {
        public PositionComponent PositionComponent;

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
    }
}
