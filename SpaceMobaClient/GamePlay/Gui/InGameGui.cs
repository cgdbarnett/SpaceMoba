// Myra libraries
using Myra.Graphics2D.UI;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework;

// Game libraries
using SpaceMobaClient.GamePlay.Components;
using SpaceMobaClient.Systems.Objects;

namespace SpaceMobaClient.GamePlay.Gui
{
    /// <summary>
    /// GUI for in game.
    /// </summary>
	public partial class InGameGui
	{
        // References to entities whose events are registered.
        private Entity RegisteredLocalPlayer,
            RegisteredMothership;

        /// <summary>
        /// Creates an instance of the InGame GUI.
        /// </summary>
		public InGameGui()
		{
			BuildUI();

            PlayerUpgradeCloseButton.Click += 
                (s, a) => ClosePanel(PlayershipUpgradePanel);

            MotherUpgradeCloseButton.Click +=
                (s, a) => ClosePanel(MothershipUpgradePanel);
		}

        /// <summary>
        /// Updates the values of UI elements.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Register entity event handlers
            ReregisterEntityEventHandlers();

            // Calculate fps, and update label
            int fps = (int)(1000 / gameTime.ElapsedGameTime.TotalMilliseconds);
            FpsLabel.Text = fps.ToString();

            // Update minimap
            UpdateMinimap(gameTime);
        }

        /// <summary>
        /// Update icons for players on the minimap.
        /// </summary>
        private void UpdateMinimap(GameTime gameTime)
        {
            // Player icon
            int player_x = (int)((float)LocalPlayer.X / 12000 * 180);
            int player_y = (int)((float)LocalPlayer.Y / 12000 * 180);
            MinimapPlayerIcon.Left = player_x;
            MinimapPlayerIcon.Top = player_y;
            MinimapPlayerIcon.Visible = true;

            // Team member icons
            TeamComponent team =
                (TeamComponent)LocalPlayer.Entity[ComponentId.Team];
            if (team != null)
            {
                // Mothership
                if (team.MothershipPosition != null)
                {
                    int mother_x =
                        (int)(team.MothershipPosition.X / 12000 * 180);
                    int mother_y =
                        (int)(team.MothershipPosition.Y / 12000 * 180);
                    MinimapMothershipIcon.Left = mother_x;
                    MinimapMothershipIcon.Top = mother_y;
                    MinimapMothershipIcon.Visible = true;
                }
                else
                {
                    MinimapMothershipIcon.Visible = false;
                }

                // Team mates
                MinimapTeamPlayer1Icon.Visible = false;
                MinimapTeamPlayer2Icon.Visible = false;
                byte currentMember = 0;
                for (int i = 0; i < team.MemberCount; i++)
                {
                    if (team.MemberId[i] != LocalPlayer.Entity.Id)
                    {
                        int team_x =
                            (int)(team.MemberPositions[i].X / 12000 * 180);
                        int team_y =
                            (int)(team.MemberPositions[i].Y / 12000 * 180);
                        if(currentMember == 0)
                        {
                            MinimapTeamPlayer1Icon.Left = team_x;
                            MinimapTeamPlayer1Icon.Top = team_y;
                            MinimapTeamPlayer1Icon.Visible = true;
                        }
                        else
                        {
                            MinimapTeamPlayer2Icon.Left = team_x;
                            MinimapTeamPlayer2Icon.Top = team_y;
                            MinimapTeamPlayer2Icon.Visible = true;
                        }
                        currentMember++;
                    }
                }
            }
        }

        /// <summary>
        /// Registers event handlers as required.
        /// </summary>
        private void ReregisterEntityEventHandlers()
        {
            // Register localplayer
            if(RegisteredLocalPlayer != LocalPlayer.Entity)
            {
                RegisteredLocalPlayer = LocalPlayer.Entity;
                RegisteredLocalPlayer.OnClick += 
                    (s, a) => ShowPlayerUpgradePanel();
            }

            // Register mothership so long as it exists.
            if(RegisteredLocalPlayer != null)
            {
                TeamComponent team =
                    (TeamComponent)LocalPlayer.Entity[ComponentId.Team];
                Entity mothership = 
                    EntityManager.GetEntityById(team.MothershipId);
                if(RegisteredMothership != mothership)
                {
                    RegisteredMothership = mothership;
                    if(RegisteredMothership != null)
                    {
                        RegisteredMothership.OnClick +=
                            (s, a) => ShowMothershipUpgradePanel();
                    }
                }
            }
        }

        /// <summary>
        /// Closes the indicated panel.
        /// </summary>
        /// <param name="panel">Panel to close.</param>
        private void ClosePanel(Panel panel)
        {
            panel.Visible = false;
        }

        /// <summary>
        /// Shows the player upgrade panel.
        /// </summary>
        private void ShowPlayerUpgradePanel()
        {
            MothershipUpgradePanel.Visible = false;
            PlayershipUpgradePanel.Visible = true;
        }

        /// <summary>
        /// Shows the mothership upgrade panel.
        /// </summary>
        private void ShowMothershipUpgradePanel()
        {
            MothershipUpgradePanel.Visible = true;
            PlayershipUpgradePanel.Visible = false;
        }
	}
}