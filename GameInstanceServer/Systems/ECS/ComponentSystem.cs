using System;
using System.Collections.Generic;

namespace GameInstanceServer.Systems.ECS
{
    /// <summary>
    /// A system within the ECS. Should be implemented by all
    /// systems to update all components.
    /// </summary>
    public abstract class ComponentSystem
    {
        /// <summary>
        /// Identifier of this system. Should be set by implementing classes.
        /// </summary>
        public ComponentSystemId Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// Flags whether to send updates to all components in system.
        /// </summary>
        protected bool WantsUpdates
        {
            get;
            set;
        }

        /// <summary>
        /// All components registered to the system.
        /// </summary>
        protected Dictionary<int, IComponent> Components;

        /// <summary>
        /// Creates the base System class.
        /// </summary>
        public ComponentSystem()
        {
            Components = new Dictionary<int, IComponent>();
        }

        /// <summary>
        /// Registers the update event handlers.
        /// </summary>
        public void RegisterEventHandlers()
        {
            if(WantsUpdates)
            {
                ECS.OnUpdate += Update;
            }
        }

        /// <summary>
        /// Unregisters the update event handlers.
        /// </summary>
        public void UnregisterEventHandlers()
        {
            if (WantsUpdates)
            {
                ECS.OnUpdate -= Update;
            }
        }

        /// <summary>
        /// Register a component to this system.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <param name="component">Component to register.</param>
        public virtual void RegisterComponent(int id, IComponent component)
        {
            try
            {
                Components.Add(id, component);
            }
            catch(Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// Unregister a component from this system.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual void UnregisterComponent(int id)
        {
            try
            {
                Components.Remove(id);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// Can be overwritten by implementing classes. Intended to
        /// update per frame all components in the system.
        /// </summary>
        /// <param name="sender">Sending object. Usually null.</param>
        /// <param name="gameTime">Interval of game frame.</param>
        protected virtual void Update(object sender, TimeSpan gameTime)
        {
            foreach(IComponent component in Components.Values)
            {
                UpdateComponent(component, gameTime);
            }
        }

        /// <summary>
        /// Optional method for updating components individually. Called by
        /// Update so if that is overriden this will not be called.
        /// </summary>
        /// <param name="component">Component to update.</param>
        /// <param name="gameTime">Interval of game frame.</param>
        protected virtual void UpdateComponent(
            IComponent component, TimeSpan gameTime
            )
        {
            // Do nothing
        }
    }
}
