using System;

using Lidgren.Network;

using GameInstanceServer.Game.Objects;
using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Teams
{
    public class TeamComponent : IComponent
    {
        /// <summary>
        /// This is a serializable component.
        /// </summary>
        public bool Serializable => false;

        /// <summary>
        /// Id of team system.
        /// </summary>
        public ComponentSystemId ComponentSystem
            => ComponentSystemId.TeamSystem;

        /// <summary>
        /// Reference to the team.
        /// </summary>
        public Team Team;

        /// <summary>
        /// Serialize this component into an outgoing message.
        /// </summary>
        /// <param name="msg"></param>
        public void Serialize(NetOutgoingMessage msg)
        {
            // Mothership
            msg.Write(Team.Mothership.Position.Position.X);
            msg.Write(Team.Mothership.Position.Position.Y);

            // Team members
            msg.Write(Team.MemberCount);
            for(int i = 0; i < Team.MemberCount; i++)
            {
                msg.Write(Team.Members[i].Id);
                msg.Write(Team.Members[i].Position.Position.X);
                msg.Write(Team.Members[i].Position.Position.Y);
            }
        }
    }
}
