using System;
using System.Collections.Generic;
using System.Text;

namespace GameInstanceServer.Game.Objects.Combat
{
    /// <summary>
    /// All combat objects must implment this interface. Provides structure
    /// for combat events.
    /// </summary>
    public interface ICombatObject
    {
        /// <summary>
        /// Returns the current health of the object.
        /// </summary>
        /// <returns>Health.</returns>
        int GetHealth();

        /// <summary>
        /// Returns the maximum health of the object.
        /// </summary>
        /// <returns>Max health.</returns>
        int GetMaxHealth();

        /// <summary>
        /// Returns the current shield of the object.
        /// </summary>
        /// <returns>Shield.</returns>
        int GetShield();

        /// <summary>
        /// Returns the maximum shield value of the object.
        /// </summary>
        /// <returns>Max shield.</returns>
        int GetMaxShield();

        /// <summary>
        /// Applies damage to the object.
        /// </summary>
        /// <param name="damage">Damage to apply.</param>
        void ApplyDamage(int damage);
    }
}
