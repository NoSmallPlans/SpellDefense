using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Server
{
    public class Game
    {
        List<Player> players;
        int messagesRecieved = 0;
        NetServer server;
        public List<NetConnection> playerConns;

        public Game(List<NetConnection> playerConns, NetServer server)
        {
            this.playerConns = playerConns;
            players = new List<Player>();
            foreach (NetConnection netConn in playerConns)
            {
                Player player = new Player(netConn);
                players.Add(player);
            }
            this.server = server;
        }

        public void AddMessage(NetIncomingMessage nim, string msg)
        {
            foreach (Player p in players)
            {
                if (p.conn == nim.SenderConnection)
                {
                    if (!p.messageRecieved)
                    {
                        messagesRecieved++;
                        p.messageRecieved = true;
                        p.message = msg;
                        if (!msg.Contains("no"))
                            Console.WriteLine(msg);
                    }

                    if (messagesRecieved >= players.Count)
                    {
                        SendMessages();
                        messagesRecieved = 0;
                    }
                    break;
                }
            }
        }

        private void SendMessages()
        {
            foreach (Player player in players)
            {
                NetOutgoingMessage nom = server.CreateMessage();
                nom.Write(player.message);
                server.SendMessage(nom, playerConns, NetDeliveryMethod.ReliableOrdered, 0);
                player.messageRecieved = false;
            }
        }
    }
}
