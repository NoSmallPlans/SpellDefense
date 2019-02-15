using System;
using System.Collections.Generic;
using Lidgren.Network;
using static SpellDefense.Common.Networking.Messaging;

namespace SpellDefense.Server
{
    class Server
    {
        private NetServer server;
        private List<NetPeer> clients;
        private List<Game> games;
        Dictionary<NetConnection, Game> gameConns;
        private readonly ManagerLogger _managerLogger;

        public Server(ManagerLogger managerLogger)
        {
            try
            {
                _managerLogger = managerLogger;
                var config = new NetPeerConfiguration("spelldefense1") { Port = 14242 };
                server = new NetServer(config);

                clients = new List<NetPeer>();
                gameConns = new Dictionary<NetConnection, Game>();
                games = new List<Game>();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private void NewConnect(NetConnection playerConn)
        {
            clients.Add(playerConn.Peer);
            if (clients.Count > 1)
            {
                MatchPlayer(playerConn);
            }
        }

        private bool MatchPlayer(NetConnection playerConn)
        {
            NetOutgoingMessage nom = server.CreateMessage();
            //Check to see if there is an available opponent
            foreach (NetConnection nc in server.Connections)
            {
                if (!gameConns.ContainsKey(nc))
                {
                    Game game = new Game(new List<NetConnection>{ nc, playerConn }, server);
                    games.Add(game);
                    gameConns.Add(nc, game);
                    gameConns.Add(playerConn, game);

                    //host player set to red team
                    nom.Write((int)MsgType.GameStart + "," + 0);                    
                    server.SendMessage(nom, nc, NetDeliveryMethod.ReliableOrdered, 0);

                    //Joining player set to blue team
                    nom = server.CreateMessage();
                    nom.Write((int)MsgType.GameStart + "," + 1);
                    server.SendMessage(nom, playerConn, NetDeliveryMethod.ReliableOrdered, 0);
                    _managerLogger.AddLogMessage("Server", "Players matched!");
                    return true;
                }
            }
            _managerLogger.AddLogMessage("Server", "No Opponent found");
            nom.Write("No Opponents Available");
            server.SendMessage(nom, playerConn, NetDeliveryMethod.ReliableOrdered, 0);
            return false;
        }

        private void Disconnect(NetConnection playerConn)
        {
            gameConns.Remove(playerConn);
            clients.Remove(playerConn.Peer);
        }

        private void Data(NetIncomingMessage inc)
        {
            var data = inc.ReadString();
            _managerLogger.AddLogMessage("Server", data);

            if (gameConns.ContainsKey(inc.SenderConnection))
            {
                gameConns[inc.SenderConnection].AddMessage(inc);
            }
            /*
            var msgType = (MsgType)inc.ReadByte();
            //var gameRoom = GetGameRoomById(inc.ReadString());
            var command = PacketFactory.GetCommand(msgType);
            command.Run(_managerLogger, this, inc, null, null);
            */
        }

        public void ReadMessages()
        {
            server.Start();
            if (server.Status == NetPeerStatus.Running) {
                _managerLogger.AddLogMessage("Server", "Server is running on port " + server.Port);
            }
            else {
                _managerLogger.AddLogMessage("Server", "Server not started...");
            }

            NetIncomingMessage message;

            while (true)
            {
                while ((message = server.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                Data(message);
                                break;
                            }
                        case NetIncomingMessageType.DebugMessage:
                            _managerLogger.AddLogMessage("Server", message.ReadString());
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            _managerLogger.AddLogMessage("Server", message.SenderConnection.Status.ToString());
                            if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                            {
                                NewConnect(message.SenderConnection);
                                _managerLogger.AddLogMessage("Server", "{0} has connected." + message.SenderConnection.Peer.Configuration.LocalAddress);
                            }
                            if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
                            {
                                Disconnect(message.SenderConnection);
                                _managerLogger.AddLogMessage("Server", "{0} has disconnected." + message.SenderConnection.Peer.Configuration.LocalAddress);
                            }
                            break;
                        default:
                            _managerLogger.AddLogMessage("Server", "Unhandled message type: {message.MessageType}");
                            break;
                    }
                    server.Recycle(message);
                }
            }
        }
    }
}
