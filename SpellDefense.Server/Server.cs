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

        public void StartServer()
        {
            var config = new NetPeerConfiguration("spelldefense") { Port = 14242 };
            server = new NetServer(config);
            server.Start();

            if (server.Status == NetPeerStatus.Running)
            {
                _managerLogger.AddLogMessage(new Util.LogMessage { Id = "0", Message = "Server is running on port " + config.Port });
            }
            else
            {
                _managerLogger.AddLogMessage(new Util.LogMessage { Id = "0", Message = "Server not started..." });
            }
            clients = new List<NetPeer>();
            gameConns = new Dictionary<NetConnection, Game>();
            games = new List<Game>();
        }

        public Server(ManagerLogger managerLogger)
        {
            try
            {
                _managerLogger = managerLogger;
                //var config = new NetPeerConfiguration("spelldefense1") { Port = 14242 };
                //server = new NetServer(config);

                StartServer();
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
                    Game game = new Game(new List<NetConnection> { nc, playerConn }, server);
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
                    _managerLogger.AddLogMessage(new Util.LogMessage { Id = "0", Message = "Players matched!" });
                    return true;
                }
            }
            _managerLogger.AddLogMessage(new Util.LogMessage { Id = "0", Message = "No Opponent found" });
            nom.Write("No Opponents Available");
            server.SendMessage(nom, playerConn, NetDeliveryMethod.ReliableOrdered, 0);
            return false;
        }

        private void Disconnect(NetConnection playerConn)
        {
            EndGame(playerConn);
            clients.Remove(playerConn.Peer);
        }

        private void EndGame(NetConnection playerConn)
        {
            NetOutgoingMessage nom = server.CreateMessage();
            nom.Write((int)MsgType.GameOver);
            if (gameConns.ContainsKey(playerConn))
            {
                Game g = gameConns[playerConn];
                server.SendMessage(nom, g.playerConns, NetDeliveryMethod.ReliableOrdered, 0);
                //Clean up game connections
                foreach (NetConnection nt in g.playerConns)
                {
                    gameConns.Remove(nt);
                }
                //Clean up games list
                games.Remove(g);
            }
        }

        private void SendAction(NetIncomingMessage message)
        {
            var data = message.ReadString();
            if (!data.Contains("no"))
                _managerLogger.AddLogMessage(new Util.LogMessage { Id = "0", Message = data });
            if (gameConns.ContainsKey(message.SenderConnection))
            {
                gameConns[message.SenderConnection].AddMessage(message, data);
            }
        }

        public void ReadMessages()
        {
            NetIncomingMessage message;
            var stop = false;

            while (!stop)
            {
                while ((message = server.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                MsgType msgType = (MsgType)message.ReadByte();
                                switch (msgType)
                                {
                                    case MsgType.NoAction:
                                        SendAction(message);
                                        break;
                                    case MsgType.PlayCard:
                                        SendAction(message);
                                        break;
                                    case MsgType.QueueCard:
                                        SendAction(message);
                                        break;
                                    case MsgType.GameOver:
                                        EndGame(message.SenderConnection);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            }
                        case NetIncomingMessageType.DebugMessage:
                            _managerLogger.AddLogMessage(new Util.LogMessage { Id = "0", Message = message.ReadString() });
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            _managerLogger.AddLogMessage(new Util.LogMessage { Id = "0", Message = message.SenderConnection.Status.ToString() });
                            if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                            {
                                NewConnect(message.SenderConnection);
                                _managerLogger.AddLogMessage(new Util.LogMessage { Id = "0", Message = message.SenderConnection.Peer.Configuration.LocalAddress + " has connected." });
                            }
                            if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
                            {
                                Disconnect(message.SenderConnection);
                                _managerLogger.AddLogMessage(new Util.LogMessage { Id = "0", Message = message.SenderConnection.Peer.Configuration.LocalAddress + " has disconnected." });
                            }
                            break;
                        default:
                            _managerLogger.AddLogMessage(new Util.LogMessage { Id = "0", Message = "Unhandled message type: {message.MessageType}" });
                            break;
                    }
                    server.Recycle(message);
                }
            }

            _managerLogger.AddLogMessage(new Util.LogMessage { Id = "0", Message = "Shutdown package \"exit\" received. Press any key to finish shutdown" });
            Console.ReadKey();
        }
    }
}
