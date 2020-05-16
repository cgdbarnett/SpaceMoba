// System libraries
using System;
using System.Collections.Generic;
using System.Diagnostics;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Game libraries
using SpaceMobaClient.GamePlay;
using SpaceMobaClient.GamePlay.Components;

namespace SpaceMobaClient.Systems.Objects
{
    /// <summary>
    /// An object with various components as received from the server.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Unique identifier of entity. (From server).
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// Fires when the entity is clicked in game.
        /// </summary>
        public event EventHandler OnClick;

        /// <summary>
        /// Maps component ids to the instanced component.
        /// </summary>
        private readonly Dictionary<ComponentId, IComponent> Components;

        /// <summary>
        /// Returns the instanced component with a given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IComponent this[ComponentId index]
        {
            get
            {
                try
                {
                    return Components[index];
                }
                catch(Exception e)
                {
                    Trace.WriteLine("Exception in Entity.this:");
                    Trace.WriteLine(e.ToString());
                    return null;
                }
            }
        }

        /// <summary>
        /// Creates a new entity with a given id.
        /// </summary>
        /// <param name="id">Id of entity from server.</param>
        public Entity(int id)
        {
            Id = id;
            Components = new Dictionary<ComponentId, IComponent>();
        }

        /// <summary>
        /// Adds, or updates a component.
        /// </summary>
        /// <param name="component">New component.</param>
        public void AddOrUpdateComponent(IComponent component)
        {
            if(Components.ContainsKey(component.Id))
            {
                Components[component.Id] = component;
            }
            else
            {
                Components.Add(component.Id, component);
            }
        }

        /// <summary>
        /// Runs the link methods of all components.
        /// </summary>
        public void LinkAllComponents()
        {
            foreach(IComponent component in Components.Values)
            {
                try
                {
                    component.Link();
                }
                catch(Exception e)
                {
                    Trace.WriteLine("Exception in Entity.LinkAllComponents:");
                    Trace.WriteLine(e.ToString());
                }
            }
        }

        /// <summary>
        /// Update all components of this entity.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        /// <param name="camera">Active view camera.</param>
        /// <param name="mouse">Current mouse state.</param>
        public void Update(GameTime gameTime, Camera camera, MouseState mouse)
        {
            // Check mouse input
            if(mouse.LeftButton == ButtonState.Pressed)
            {
                if(OnClick != null)
                {
                    Vector2 pos = (
                        (PositionComponent)Components[ComponentId.Position]
                        ).Position;
                    Vector2 offsetMousePos = new Vector2(
                        mouse.X - camera.OffsetX, mouse.Y - camera.OffsetY
                        );
                    if ((pos - offsetMousePos).LengthSquared() <= 128 * 128)
                    {
                        OnClick.Invoke(this, null);
                    }
                }
            }

            // Update all components
            foreach(IComponent component in Components.Values)
            {
                try
                {
                    if (component.WantsUpdates)
                    {
                        component.Update(gameTime);
                    }
                }
                catch(Exception e)
                {
                    Trace.WriteLine("Exception in Entity.Update: ");
                    Trace.WriteLine(e.ToString());
                }
            }
        }

        /// <summary>
        /// Draws all components of this entity.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="camera">Currently active camera.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (IComponent component in Components.Values)
            {
                try
                {
                    if (component.WantsDraws)
                    {
                        component.Draw(spriteBatch, camera);
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Exception in Entity.Draw: ");
                    Trace.WriteLine(e.ToString());
                }
            }
        }
    }
}
