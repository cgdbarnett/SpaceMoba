using System;
using System.Collections.Generic;
using System.Text;

using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Combat
{
    /// <summary>
    /// Represents an instance of damage.
    /// </summary>
    public class Damage
    {
        /// <summary>
        /// Value of the damage.
        /// </summary>
        public int Value;

        /// <summary>
        /// Attacking entity.
        /// </summary>
        public Entity Attacker;
    }
}
