using System;
using System.Collections.Generic;

namespace GameInstanceServer.Systems.ECS
{
    /// <summary>
    /// Entity Component System.
    /// </summary>
    public static class ECS
    {
        private static Dictionary<ComponentSystemId, ComponentSystem> _Systems;

        /// <summary>
        /// Systems being run by the ECS. (Lazy initialisation).
        /// </summary>
        private static Dictionary<ComponentSystemId, ComponentSystem> Systems
        {
            get
            {
                if(_Systems == null)
                {
                    _Systems = new Dictionary<ComponentSystemId, ComponentSystem>();
                }
                return _Systems;
            }
        }

        /// <summary>
        /// Unique identifier counter.
        /// </summary>
        private static int NextUniqueId = 0;

        /// <summary>
        /// Triggers update events to systems.
        /// </summary>
        public static event EventHandler<TimeSpan> OnUpdate;

        /// <summary>
        /// Returns the next Id for an entity.
        /// </summary>
        /// <returns>Next unique id.</returns>
        public static int GetNextId()
        {
            return NextUniqueId++;
        }

        /// <summary>
        /// Registers a system to the ECS.
        /// </summary>
        /// <param name="system">System to register.</param>
        public static void RegisterSystem(ComponentSystem system)
        {
            try
            {
                Systems.Add(system.Id, system);
                system.RegisterEventHandlers();
            }
            catch(Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// Unregisters a system from the ECS.
        /// </summary>
        /// <param name="system">System to unregister.</param>
        public static void UnregisterSystem(ComponentSystem system)
        {
            try
            {
                Systems.Remove(system.Id);
                system.UnregisterEventHandlers();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }


        /// <summary>
        /// Registers a component to its system.
        /// </summary>
        /// <param name="system">Identifier of system to register to.</param>
        /// <param name="id">Entity id.</param>
        /// <param name="component">Component to register.</param>
        public static void RegisterComponentToSystem(
            ComponentSystemId system, int id, IComponent component
            )
        {
            try
            {
                Systems[system].RegisterComponent(id, component);
            }
            catch(Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// Unregisters a component from its system.
        /// </summary>
        /// <param name="system">Identifier of system to register to.</param>
        /// <param name="id">Entity id.</param>
        /// <param name="component">Component to register.</param>
        public static void UnregisterComponentToSystem(
            ComponentSystemId system, int id, IComponent component
            )
        {
            try
            {
                Systems[system].UnregisterComponent(id);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// Run a single update frame on all systems.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public static void Update(TimeSpan gameTime)
        {
            try
            {
                OnUpdate.Invoke(null, gameTime);
            }
            catch
            {
            }
        }
    }
}
