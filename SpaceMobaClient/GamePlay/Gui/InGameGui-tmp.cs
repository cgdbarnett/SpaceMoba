using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SpaceMobaClient.GamePlay.Components;
using SpaceMobaClient.Systems.Objects;

namespace SpaceMobaClient.GamePlay.Gui
{
    /// <summary>
    /// Handles drawing and updating the UI in game.
    /// </summary>
    public class InGameGui
    {
        // Gui Textures (Preloaded).
        private readonly Texture2D Minimap;
        private readonly Texture2D Dot;
        private readonly Texture2D ResourceBorder;
        private readonly Texture2D IconMothership;
        private readonly Texture2D IconPlayership;

        // Fonts
        private readonly SpriteFont Arial12;

        /// <summary>
        /// Creates an instance of the InGameGui.
        /// </summary>
        public InGameGui()
        {
            Minimap = GameClient.GetGameClient().Content
                .Load<Texture2D>("Gui/minimap");
            Dot = GameClient.GetGameClient().Content
                .Load<Texture2D>("Gui/dot");
            ResourceBorder = GameClient.GetGameClient().Content
                .Load<Texture2D>("Gui/resources");
            IconMothership = GameClient.GetGameClient().Content
                .Load<Texture2D>("Gui/ico_mothership");
            IconPlayership = GameClient.GetGameClient().Content
                .Load<Texture2D>("Gui/ico_playership");

            Arial12 = GameClient.GetGameClient().Content
                .Load<SpriteFont>("Fonts/Arial12");
        }

        /// <summary>
        /// Draws the GUI.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="camera">Active camera.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            DrawMinimap(spriteBatch, camera);
            DrawResources(spriteBatch, camera);
        }

        /// <summary>
        /// Draws the minimap element.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="camera">Active camera.</param>
        private void DrawMinimap(SpriteBatch spriteBatch, Camera camera)
        {
            // Background texture
            spriteBatch.Draw(Minimap, 
                new Rectangle(10, 890, 180, 180), 
                Color.White
                );

            // Player icon
            int player_x = (int)((float)LocalPlayer.X / 12000 * 180);
            int player_y = (int)((float)LocalPlayer.Y / 12000 * 180);
            spriteBatch.Draw(Dot, 
                new Rectangle(10 + player_x, 890 + player_y, 2, 2),
                Color.White
                );

            // Mothership
            TeamComponent team = 
                (TeamComponent)LocalPlayer.Entity[ComponentId.Team];
            if (team != null)
            {
                if (team.MothershipPosition != null)
                {
                    int mother_x = 
                        (int)(team.MothershipPosition.X / 12000 * 180);
                    int mother_y = 
                        (int)(team.MothershipPosition.Y / 12000 * 180);
                    spriteBatch.Draw(Dot,
                        new Rectangle(10 + mother_x, 890 + mother_y, 2, 2),
                        Color.Blue
                        );
                }

                // Team mates
                for (int i = 0; i < team.MemberCount; i++)
                {
                    if (team.MemberId[i] != LocalPlayer.Entity.Id)
                    {
                        int team_x =
                            (int)(team.MemberPositions[i].X / 12000 * 180);
                        int team_y =
                            (int)(team.MemberPositions[i].Y / 12000 * 180);
                        spriteBatch.Draw(Dot,
                            new Rectangle(10 + team_x, 890 + team_y, 2, 2),
                            Color.Green
                            );
                    }
                }
            }
        }

        /// <summary>
        /// Draws the resource count.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="camera">Active camera.</param>
        private void DrawResources(SpriteBatch spriteBatch, Camera camera)
        {
            // Background texture
            spriteBatch.Draw(ResourceBorder,
                new Rectangle(10, 869, 180, 21),
                Color.White
                );

            // Mothership icon + resource value
            spriteBatch.Draw(IconMothership,
                new Rectangle(12, 872, 16, 16),
                Color.White
                );
            spriteBatch.DrawString(Arial12, "5000",
                new Vector2(31, 871), Color.White
                );

            // Playership icon + resource value
            spriteBatch.Draw(IconPlayership,
                new Rectangle(102, 872, 16, 16),
                Color.White
                );
            spriteBatch.DrawString(Arial12, "500",
                new Vector2(121, 871), Color.White
                );
        }
    }
}
