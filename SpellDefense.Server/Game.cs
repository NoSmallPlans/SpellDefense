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
        Queue<byte[]> messages;
        int messagesRecieved = 0;
        NetServer server;
        List<NetConnection> playerConns;

        public Game(List<NetConnection> playerConns, NetServer server)
        {
            this.playerConns = playerConns;
            players = new List<Player>();
            foreach(NetConnection netConn in playerConns)
            {
                Player player = new Player(netConn);
                players.Add(player);
            }
            messages = new Queue<byte[]>();
            this.server = server;
        }

        public void AddMessage(NetIncomingMessage nim)
        {
            foreach (Player p in players)
            {
                if (p.conn == nim.SenderConnection) {
                    if (!p.messageRecieved) {
                        messagesRecieved++;
                        p.messageRecieved = true;                     
                    }
                    p.messages.Add(nim.Data);

                    if (messagesRecieved >= players.Count) {
                        //Console.WriteLine("Sending Messages: " + messagesRecieved);
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
                foreach (byte[] data in player.messages)
                {
                    NetOutgoingMessage nom = server.CreateMessage();
                    nom.Write(data);
                    server.SendMessage(nom, playerConns, NetDeliveryMethod.ReliableOrdered, 0);
                }
                player.messageRecieved = false;
                player.messages.Clear();
            }
        }
    }
}
