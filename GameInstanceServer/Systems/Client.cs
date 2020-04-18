using System;
using System.Collections.Generic;
using System.Text;

using Lidgren.Network;

using GameInstanceServer.Game.Objects;

namespace GameInstanceServer.Systems
{
    public class Client
    {
        public int Id;

        public byte Team;

        public bool Active
        {
            get;
            set;
        }

        public bool IsReady;

        public NetConnection NetPeer;

        public IGameObject GameObject;
    }
}
