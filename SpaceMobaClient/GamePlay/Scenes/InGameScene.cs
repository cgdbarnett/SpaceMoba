using System;

using Myra.Graphics2D.UI;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

// Game libraries
using SpaceMobaClient.GamePlay.Gui;
using SpaceMobaClient.GamePlay.Network;
using SpaceMobaClient.GamePlay.World;
using SpaceMobaClient.Systems.IO;
using SpaceMobaClient.Systems.Network;
using SpaceMobaClient.Systems.Objects;
using SpaceMobaClient.Systems.Scenes;
using SpaceMobaClient.Systems.Sound;

namespace SpaceMobaClient.GamePlay.Scenes
{
    /// <summary>
    /// The scene where the game takes place.
    /// </summary>
    public class InGameScene : IScene
    {
        // Iterators for scene.
        public IScene Next { get; set; }
        public IScene Previous { get; set; }

        // Drawing
        private Camera Camera;
        private readonly SpriteBatch SpriteBatch;
        private Background Background;
        private InGameGui Gui;

        // Networking
        private GameNetworkHandler NetworkHandler;

        /// <summary>
        /// Creates an instance of the InGame scene.
        /// </summary>
        public InGameScene()
        {
            GraphicsDevice graphics = 
                GameClient.GetGameClient().GraphicsDevice;
            SpriteBatch = new SpriteBatch(graphics);
            Camera = new Camera(0, 0, 
                graphics.Viewport.Width, graphics.Viewport.Height
                );
        }

        /// <summary>
        /// Loads anything that we missed. Really, this should do nothing, but
        /// it is required by IScene.
        /// </summary>
        /// <param name="handover">null.</param>
        public void Load(object handover)
        {
            NetworkHandler = (GameNetworkHandler)handover;
            Background = new Background();
            Gui = new InGameGui();
            Desktop.Root = Gui;
            SoundManager.PlaySong("Music/bg_heartbeat");
            LocalPlayer.StartEffects();
        }

        /// <summary>
        /// Unload game content.
        /// </summary>
        public void Unload()
        {
            GameClient.GetGameClient().Content.Unload();
            NetworkHandler = null;
            Background = null;
            Gui = null;
            LocalPlayer.Reset();
        }

        /// <summary>
        /// Draws a frame of the scene.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Draw(GameTime gameTime)
        {
            // Update camera
            if(LocalPlayer.Entity != null)
            {
                Camera.CenterOnTarget(LocalPlayer.X, LocalPlayer.Y);
            }

            // Start drawing
            GameClient.GetGameClient().GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin();

            // Draw background first
            Background.DrawBottom(SpriteBatch, Camera);

            // Draw all entities
            EntityManager.Draw(SpriteBatch, Camera);

            // Draw foreground last
            Background.DrawTop(SpriteBatch, Camera);

            // End drawing
            SpriteBatch.End();

            Desktop.Render();
        }

        /// <summary>
        /// Updates the scene.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Update(GameTime gameTime)
        {
            // Update networking
            NetworkManager.HandleIncomingMessages();

            // Update input
            InputState io = LocalPlayer.Update(gameTime);
            if(io != null)
            {
                // Send to game server
                NetworkHandler.SendMessage(PacketWriter.UpdatePlayerInput(io));
            }

            // Update entities
            EntityManager.Update(gameTime, Camera);

            // Update GUI
            Gui.Update(gameTime);
        }
    }
}
