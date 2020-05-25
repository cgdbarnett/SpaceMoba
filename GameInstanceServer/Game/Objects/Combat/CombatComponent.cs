using System.Collections.Generic;

using Lidgren.Network;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Systems.Physics;

namespace GameInstanceServer.Game.Objects.Combat
{
    public class CombatComponent : IComponent
    {
        /// <summary>
        /// Gets the ComponentSystem for Combat.
        /// </summary>
        public ComponentSystemId ComponentSystem
        {
            get
            {
                return ComponentSystemId.CombatSystem;
            }
        }

        /// <summary>
        /// Parent entity of component.
        /// </summary>
        public Entity Entity;

        /// <summary>
        /// Current health of entity.
        /// </summary>
        public int Health;

        /// <summary>
        /// Maximum health of entity.
        /// </summary>
        public int MaxHealth;

        /// <summary>
        /// Current armour of entity.
        /// </summary>
        public int Armour;

        /// <summary>
        /// Maximum armour of entity.
        /// </summary>
        public int MaxArmour;

        /// <summary>
        /// Log of recent incoming damage.
        /// </summary>
        public List<Damage> DamageLog = new List<Damage>(3);

        /// <summary>
        /// This is a serializable component, but called
        /// at a custom time.
        /// </summary>
        public bool Serializable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// This is a serializable component.
        /// </summary>
        /// <param name="msg">Outgoing message.</param>
        public void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(Health);
            msg.Write(MaxHealth);
            msg.Write(Armour);
            msg.Write(MaxArmour);
        }
    }
}
