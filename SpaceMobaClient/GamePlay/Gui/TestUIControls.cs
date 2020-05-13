using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonoGame.UI.Forms;

using Microsoft.Xna.Framework;

using SpaceMobaClient.Systems.Scenes;

namespace SpaceMobaClient.GamePlay.Gui
{
    /// <summary>
    /// Class used for testing GUI designs.
    /// </summary>
    public class TestUIControls : ControlManager
    {
        // Mothership forms
        private Form MothershipOptions;
        private Form MothershipUpgrades;
        private Form MothershipBuild;

        // Player forms
        private Form PlayershipUpgrades;

        /// <summary>
        /// Create control manager.
        /// </summary>
        /// <param name="game">GameClient.</param>
        public TestUIControls(Game game) : base(game)
        {

        }

        /// <summary>
        /// Initialise this components forms.
        /// </summary>
        public override void InitializeComponent()
        {
            MothershipOptions = new Form()
            {
                Title = "",
                BackgroundColor = Color.Navy,
                WinBottomTexture = "Gui/tree_mothership",
                Size = new Vector2(250, 160),
                Location = new Vector2(550, 450),
                IsVisible = true,
                Enabled = true,
                IsMovable = false
            };

            MothershipOptions.Controls.AddRange(
                new List<Control>()
                {
                    // Title label
                    new Label()
                    {
                        Size = new Vector2(250, 80),
                        Location = new Vector2(0, 0),
                        FontName = "Fonts/Arial14",
                        TextColor = Color.LightGray,
                        Text = "Mothership"
                    },

                    // Close button
                    new Button()
                    {
                        BackgroundColor = Color.Black,
                        Size = new Vector2(20, 20),
                        Location = new Vector2(229, 1),
                        FontName = "Fonts/Arial14",
                        Text = "X",
                        TextColor = Color.White
                    },

                    // Build button
                    new Button()
                    {
                        Size = new Vector2(250, 40),
                        Location = new Vector2(0, 80),
                        FontName = "Fonts/Arial14",
                        TextColor = Color.LightGray,
                        Text = "Build",
                        BackgroundColor = Color.DarkBlue
                    },

                    // Upgrade button
                    new Button()
                    {
                        Size = new Vector2(250, 40),
                        Location = new Vector2(0, 120),
                        FontName = "Fonts/Arial14",
                        TextColor = Color.LightGray,
                        Text = "Upgrade",
                        BackgroundColor = Color.DarkBlue
                    }
                }
                );

            MothershipOptions.Controls[1].Clicked +=
                (object Sender, EventArgs args) =>
                {
                    MothershipOptions.IsVisible = false;
                    MothershipOptions.Enabled = false;
                };

            MothershipOptions.Controls[2].Clicked +=
                (object Sender, EventArgs args) =>
                {

                };

            MothershipOptions.Controls[3].Clicked +=
                (object Sender, EventArgs args) =>
                {

                };

            Controls.Add(MothershipOptions);
        }

        /// <summary>
        /// Update the controls / forms of the UI.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update values as required here.
        }
    }
}
