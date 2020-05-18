using System;
using System.Collections.Generic;
using System.Text;

using Lidgren.Network;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Game.Teams;
using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Systems.Physics;

namespace GameInstanceServer.Game.Objects.Combat
{
    public class ProjectileComponent : IComponent
    {
        /// <summary>
        /// Parent entity of component.
        /// </summary>
        public Entity Entity;

        /// <summary>
        /// Team of component.
        /// </summary>
        public Team Team;

        /// <summary>
        /// World component of entity.
        /// </summary>
        public WorldComponent World;

        /// <summary>
        /// Position component of entity.
        /// </summary>
        public PositionComponent Position;
        
        /// <summary>
        /// Gets the ComponentSystem for Projectiles.
        /// </summary>
        public ComponentSystemId ComponentSystem
        {
            get
            {
                return ComponentSystemId.ProjectileSystem;
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
