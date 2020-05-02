// System libraries
using System;
using System.Collections.Generic;
using System.Diagnostics;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Lidgren libraries
using Lidgren.Network;

// Game libraries
using SpaceMobaClient.GamePlay;
using SpaceMobaClient.GamePlay.Components;

namespace SpaceMobaClient.Systems.Objects
{
    /// <summary>
    /// Manages, creates and updates entities.
    /// </summary>
    public static class EntityManager
    {
        /// <summary>
        /// Entites managed by the Entity.
        /// </summary>
        private static readonly Dictionary<int, Entity> Entities
            = new Dictionary<int, Entity>();

        /// <summary>
        /// Entites flagged to be remove at end of update event.
        /// </summary>
        private static readonly List<int> ToBeRemoved = new List<int>();
        private static readonly List<Entity> ToBeAdded = new List<Entity>();

        /// <summary>
        /// Returns whether the manager is managing a given entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Whether manager contains entity.</returns>
        public static bool Contains(Entity entity)
        {
            return (Entities.ContainsKey(entity.Id)
                || ToBeAdded.Contains(entity))
                && !(ToBeRemoved.Contains(entity.Id));
        }

        /// <summary>
        /// Returns whether the manager is managing an entity with
        /// the given id. Does not check if entity is pending.
        /// </summary>
        /// <param name="entity">Id of entity.</param>
        /// <returns>Whether manager contains entity.</returns>
        public static bool Contains(int entity)
        {
            return Entities.ContainsKey(entity)
                && !(ToBeRemoved.Contains(entity));
        }

        /// <summary>
        /// Adds an existing entity to the manager.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        public static void Add(Entity entity)
        {
            try
            {
                if (!ToBeAdded.Contains(entity))
                {
                    ToBeAdded.Add(entity);
                }
            }
            catch(Exception e)
            {
                Trace.WriteLine("Exception in EntityManager.Add:");
                Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Removes an entity from the manager.
        /// </summary>
        /// <param name="entity">Entity to remove.</param>
        public static void Remove(Entity entity)
        {
            try
            {
                if (!ToBeRemoved.Contains(entity.Id))
                {
                    ToBeRemoved.Add(entity.Id);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Exception in EntityManager.Remove:");
                Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Removes an entity from the manager.
        /// </summary>
        /// <param name="entity">Id of entity to remove.</param>
        public static void Remove(int entity)
        {
            try
            {
                if (!ToBeRemoved.Contains(entity))
                {
                    ToBeRemoved.Add(entity);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Exception in EntityManager.Remove:");
                Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Clears the entity manager of all entities.
        /// </summary>
        public static void Clear()
        {
            Entities.Clear();
        }

        /// <summary>
        /// Creates an entity from an incoming message from the server.
        /// This is also added to the manager.
        /// </summary>
        /// <param name="message">Incoming Message.</param>
        /// <returns>Reference to created entity.</returns>
        public static Entity CreateEntityFromMessage
            (
            NetIncomingMessage message
            )
        {
            byte componentId;

            // Start creating new entity with given id
            Entity entity = new Entity(message.ReadInt32());

            // Components
            while ((componentId = message.ReadByte()) != 0)
            {
                IComponent component = null;
                switch ((ComponentId)componentId)
                {
                    // Position component
                    case ComponentId.Position:
                        component = new PositionComponent(entity);
                        break;

                    // Animation component
                    case ComponentId.Animation:
                        component = new AnimationComponent(entity);
                        break;

                    case ComponentId.AffectedByBlackhole:
                        component = new AffectedByBlackholeComponent(entity);
                        break;

                    case ComponentId.Engine:
                        component = new EngineComponent(entity);
                        break;
                }

                if (component != null)
                {
                    component.Deserialize(message);
                    entity.AddOrUpdateComponent(component);
                }
            }

            // Attempt to link all components, and add to manager.
            entity.LinkAllComponents();
            Add(entity);

            return entity;
        }
        
        /// <summary>
        /// Updates an existing entity from an incoming message.
        /// </summary>
        /// <param name="message">Incoming message from server.</param>
        public static void UpdateEntityFromMessage
            (
            NetIncomingMessage message
            )
        {
            byte componentId;

            // Start creating new entity with given id
            try
            {
                int id = message.ReadInt32();
                if (Entities.ContainsKey(id))
                {
                    Entity entity = Entities[id];

                    // Components
                    while ((componentId = message.ReadByte()) != 0)
                    {
                        IComponent component = entity[(ComponentId)componentId];

                        if (component != null)
                        {
                            component.Deserialize(message);
                        }
                    }
                } 
            }
            catch(Exception e)
            {
                Trace.WriteLine("Exception in EntityManager." +
                    " UpdateEntityFromMessage:");
                Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Updates all entities.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public static void Update(GameTime gameTime)
        {
            // Iterate over all entities and update
            foreach(Entity entity in Entities.Values)
            {
                try
                {
                    entity.Update(gameTime);
                }
                catch(Exception e)
                {
                    Trace.WriteLine("Exception in EntityManager.Update (1):");
                    Trace.WriteLine(e.ToString());
                }
            }

            // Add any entities flagged
            foreach(Entity entity in ToBeAdded)
            {
                try
                {
                    Entities.Add(entity.Id, entity);
                }
                catch(Exception e)
                {
                    Trace.WriteLine("Exception in EntityManager.Update (2):");
                    Trace.WriteLine(e.ToString());
                }
            }
            ToBeAdded.Clear();

            // Remove any entities flagged
            foreach(int id in ToBeRemoved)
            {
                try
                {
                    Entities.Remove(id);
                }
                catch(Exception e)
                {
                    Trace.WriteLine("Exception in EntityManager.Update (3):");
                    Trace.WriteLine(e.ToString());
                }
            }
            ToBeRemoved.Clear();
        }

        /// <summary>
        /// Draws all entities.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="camera">Active camera.</param>
        public static void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (Entity entity in Entities.Values)
            {
                try
                {
                    entity.Draw(spriteBatch, camera);
                }
                catch(Exception e)
                {
                    Trace.WriteLine("Exception in EntityManager.Draw:");
                    Trace.WriteLine(e.ToString());
                }
            }
        }
    }
}
