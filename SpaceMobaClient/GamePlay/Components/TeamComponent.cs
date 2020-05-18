using System;

using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceMobaClient.Systems.Objects;

namespace SpaceMobaClient.GamePlay.Components
{
    public class TeamComponent : IComponent
    {
        /// <summary>
        /// Entity that owns this component.
        /// </summary>
        public Entity Entity
        {
            get;
            private set;
        }

        public int MothershipId;
        public Vector2 MothershipPosition;

        public byte MemberCount;
        public Vector2[] MemberPositions;
        public int[] MemberId;
        public int[] MemberHealth;
        public int[] MemberMaxHealth;
        public int[] MemberArmour;
        public int[] MemberMaxArmour;

        /// <summary>
        /// This component type.
        /// </summary>
        public ComponentId Id => ComponentId.Team;

        /// <summary>
        /// Does not want updates.
        /// </summary>
        public bool WantsUpdates => false;

        /// <summary>
        /// Does not draw.
        /// </summary>
        public bool WantsDraws => false;

        /// <summary>
        /// Creates an instance of the TeamComponent.
        /// </summary>
        /// <param name="entity"></param>
        public TeamComponent(Entity entity)
        {
            Entity = entity;

            MothershipPosition = new Vector2();
            MemberId = new int[3];
            MemberPositions = new Vector2[3]
            {
                new Vector2(),
                new Vector2(),
                new Vector2()
            };
            MemberHealth = new int[3];
            MemberMaxHealth = new int[3];
            MemberArmour = new int[3];
            MemberMaxArmour = new int[3];
        }

        /// <summary>
        /// Deserialize an incoming message into a team component.
        /// </summary>
        /// <param name="message"></param>
        public void Deserialize(NetIncomingMessage message)
        {
            // Mothership
            MothershipId = message.ReadInt32();
            MothershipPosition.X = message.ReadFloat();
            MothershipPosition.Y = message.ReadFloat();

            MemberCount = message.ReadByte();

            // Team members
            for (int i = 0; i < MemberCount; i++)
            {
                MemberId[i] = message.ReadInt32();
                MemberPositions[i].X = message.ReadFloat();
                MemberPositions[i].Y = message.ReadFloat();
                MemberHealth[i] = message.ReadInt32();
                MemberMaxHealth[i] = message.ReadInt32();
                MemberArmour[i] = message.ReadInt32();
                MemberMaxArmour[i] = message.ReadInt32();
            }
        }

        /// <summary>
        /// This component doesn't draw.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="camera"></param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Nothing to link.
        /// </summary>
        public void Link()
        {
            
        }

        /// <summary>
        /// This component doesn't require updates.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
