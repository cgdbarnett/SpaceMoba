// System libraries
using System;

// XNA (Monogame) libraries
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

// Game libraries
using SpaceMobaClient.GamePlay.Components;
using SpaceMobaClient.Systems.IO;
using SpaceMobaClient.Systems.Objects;

namespace SpaceMobaClient.GamePlay
{
    /// <summary>
    /// LocalPlayer handles player input, and drawing GUI.
    /// </summary>
    public static class LocalPlayer
    {
        public static Entity Entity;
        private static InputState PreviousInputState;

        private static SoundEffectInstance EngineSound;

        /// <summary>
        /// X position of local player.
        /// </summary>
        public static int X
        {
            get
            {
                try
                {
                    return (int)(
                        (PositionComponent)Entity[ComponentId.Position])
                        .Position.X;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Y position of local player.
        /// </summary>
        public static int Y
        {
            get
            {
                try
                {
                    return (int)(
                        (PositionComponent)Entity[ComponentId.Position])
                        .Position.Y;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Starts playing sound effects
        /// </summary>
        public static void StartEffects()
        {
            SoundEffect engine = GameClient.GetGameClient().Content
                .Load<SoundEffect>("Sounds/engine_idle");
            EngineSound = engine.CreateInstance();
            EngineSound.IsLooped = true;
            EngineSound.Volume = 0.25f;
            EngineSound.Play();
        }

        /// <summary>
        /// Resets the local player.
        /// </summary>
        public static void Reset()
        {
            Entity = null;

            // Init input state
            PreviousInputState = new InputState
            {
                Backward = false,
                Forward = false,
                Left = false,
                Right = false
            };
        }

        /// <summary>
        /// Executes input handling for the local player.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public static InputState Update(GameTime gameTime)
        {
            // Poll keyboard for current state
            KeyboardState state = Keyboard.GetState();

            // Todo: Key mapping
            InputState currentInputState = new InputState
            {
                Backward = state.IsKeyDown(Keys.S),
                Forward = state.IsKeyDown(Keys.W),
                Left = state.IsKeyDown(Keys.A),
                Right = state.IsKeyDown(Keys.D),
                StrafLeft = state.IsKeyDown(Keys.Q),
                StrafRight = state.IsKeyDown(Keys.E),
                Attack = state.IsKeyDown(Keys.Space)
            };

            if (currentInputState.Forward)
            {
                EngineSound.Pitch = 1.0f;
            }
            else
            {
                EngineSound.Pitch = 0.0f;
            }

            // Check if any input state has changed
            if (currentInputState != PreviousInputState)
            {
                // It has, so we should send the new state to the server.
                PreviousInputState = currentInputState;

                return currentInputState;
            }

            return null;
        }
    }
}
