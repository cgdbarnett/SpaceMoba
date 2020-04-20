// System libraries
using System;
using System.Collections.Generic;

// XNA (Monogame) libraries.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// Game libraries.
using SpaceMobaClient.Systems.Gui;

namespace SpaceMobaClient.Content.Gui
{
    /// <summary>
    /// The Gui used when in game.
    /// </summary>
    public class InGameGui : IGui
    {
        // List of all components active with this gui.
        private Dictionary<string, IGuiComponent> Components;

        // Uses default constructor.

        /// <summary>
        /// Load resources for this GUI into memory.
        /// </summary>
        public void Load()
        {
            // Get reference to content
            ContentManager content = GameClient.GetGameClient().
                GetContentManager();

            // Define Gui here:
            Components = new Dictionary<string, IGuiComponent>()
            {
                // Game duration timer
                {
                    "timer",
                    new GuiLabel(960, 16, "Timer", content.
                    Load<SpriteFont>("Fonts/Arial14"), Color.White)
                },
                {
                    "shield",
                    new GuiHealthbar(16, 16, 150, 15,
                    content.Load<Texture2D>("Gui/barbackground"),
                    content.Load<Texture2D>("Gui/barforeground"),
                    0.8f)
                },
                {
                    "health",
                    new GuiHealthbar(16, 33, 150, 15,
                    content.Load<Texture2D>("Gui/barbackground"),
                    content.Load<Texture2D>("Gui/barforeground"),
                    0.8f)
                }
            };
        }

        /// <summary>
        /// Returns a reference to the GuiComponent with a given id.
        /// </summary>
        /// <param name="id">Id of the component to get reference to.</param>
        /// <returns>The reference to the GuiComponent.</returns>
        public IGuiComponent GetComponent(string id)
        {
            try
            {
                return Components[id];
            }
            catch
            {
                throw (new IndexOutOfRangeException());
            }
        }

        /// <summary>
        /// Draws the Gui to screen.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        /// <param name="spriteBatch">Current sprite batch.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(IGuiComponent element in Components.Values)
            {
                element.Draw(gameTime, spriteBatch);
            }
        }
    }
}
