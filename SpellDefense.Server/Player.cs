﻿using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Server
{
    public class Player
    {
        public NetConnection conn;
        public bool messageRecieved;
        public string message;

        public Player(NetConnection conn)
        {
            this.conn = conn;
            messageRecieved = false;
        }
    }
}
