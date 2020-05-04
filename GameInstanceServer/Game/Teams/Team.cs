using System;

using GameInstanceServer.Game.Objects.Ships;
using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Teams
{
    /// <summary>
    /// Holds state for a team.
    /// </summary>
    public class Team
    {
        /// <summary>
        /// Maximum players per team.
        /// </summary>
        public const int MaxTeamSize = 3;

        /// <summary>
        /// Unique identifier of team.
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Players in this team.
        /// </summary>
        public PlayerEntity[] Members;

        /// <summary>
        /// Active player members in the team.
        /// </summary>
        public byte MemberCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Mothership of the team.
        /// </summary>
        public Mothership Mothership;

        /// <summary>
        /// Creates a new team.
        /// </summary>
        public Team()
        {
            Members = new PlayerEntity[MaxTeamSize];
            Id = ECS.GetNextId();
            MemberCount = 0;
        }

        /// <summary>
        /// Registers a player to the team.
        /// </summary>
        /// <param name="entity">Player to register.</param>
        public void RegisterPlayer(PlayerEntity entity)
        {
            if(MemberCount >= MaxTeamSize)
            {
                throw (new ArgumentOutOfRangeException());
            }

            Members[MemberCount++] = entity;
        }

        /// <summary>
        /// Unregisters a player from the team.
        /// </summary>
        /// <param name="entity">Player to unregister.</param>
        public void UnregisterPlayer(PlayerEntity entity)
        {
            for(int i = 0; i < MemberCount; i++)
            {
                if(entity.Id == Members[i].Id)
                {
                    for(int j = i; j < MemberCount - 1; j++)
                    {
                        Members[j] = Members[j + 1];
                    }
                    Members[MemberCount-- - 1] = null;
                    break;
                }
            }
        }
    }
}
