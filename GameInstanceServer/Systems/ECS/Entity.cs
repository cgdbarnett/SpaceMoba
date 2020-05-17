using System;

using Lidgren.Network;

namespace GameInstanceServer.Systems.ECS
{
    /// <summary>
    /// An Entity within the ECS.
    /// </summary>
    public partial class Entity
    {
        /// <summary>
        /// Components this entity contains. Should be populated by
        /// classes implementing Entity.
        /// </summary>
        protected IComponent[] Components;

        /// <summary>
        /// Identifier of this entity.
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// A flag to indicate whether this entity should be serialized
        /// when replicating.
        /// </summary>
        public virtual bool Serializable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Flags when the last update was made to this entity. Intended
        /// for use with serialization.
        /// </summary>
        public long LastUpdated;

        /// <summary>
        /// Creates a new entity with a given id.
        /// </summary>
        /// <param name="id">Identifier of entity.</param>
        public Entity(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Registers all components to their systems in the ECS.
        /// </summary>
        public void RegisterComponents()
        {
            if(Components == null)
            {
                throw (new NullReferenceException());
            }

            foreach(IComponent component in Components)
            {
                ECS.RegisterComponentToSystem(
                    component.ComponentSystem, Id, component
                    );
            }
        }

        /// <summary>
        /// Unregisters all components from their systems in the ECS.
        /// </summary>
        public void UnregisterComponents()
        {
            if (Components == null)
            {
                throw (new NullReferenceException());
            }

            foreach (IComponent component in Components)
            {
                ECS.UnregisterComponentToSystem(
                    component.ComponentSystem, Id, component
                    );
            }
        }

        /// <summary>
        /// Gets a reference to the component with a given id.
        /// </summary>
        /// <param name="id">ComponentSystemId of component.</param>
        /// <returns>Component or null.</returns>
        public IComponent GetComponent(ComponentSystemId id)
        {
            if (Components == null)
            {
                throw (new NullReferenceException());
            }

            foreach (IComponent component in Components)
            {
                if(component.ComponentSystem == id)
                {
                    return component;
                }
            }

            return null;
        }

        /// <summary>
        /// Serializes the entity to a given outgoing message.
        /// </summary>
        /// <param name="msg">Outgoing message to append to.</param>
        public virtual void Serialize(NetOutgoingMessage msg)
        {
            msg.Write(Id);
            foreach(IComponent component in Components)
            {
                if(component.Serializable)
                {
                    component.Serialize(msg);
                }
            }

            // Null termination
            msg.Write((byte)0);
        }
    }
}
