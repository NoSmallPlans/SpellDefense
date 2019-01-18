﻿using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.GodClass;
using static SpellDefense.Common.Networking.Messaging;

namespace SpellDefense.Common
{
    public class Client
    {
        private NetClient client;
        public Queue<MsgStruct> incomingActionQueue;
        public Queue<string> outActionQueue;
        private string prevAction;
        public TeamColor teamColor;

        public Client()
        {
            incomingActionQueue = new Queue<MsgStruct>();
            outActionQueue = new Queue<string>();
        }

        public void StartClient()
        {
            var config = new NetPeerConfiguration("spelldefense");
            config.AutoFlushSendQueue = false;
            client = new NetClient(config);
            client.Start();

             //TODO make these configurable in UI
            string ip = "73.109.92.27";//"192.168.0.10";
            int port = 14242;
            client.Connect(ip, port);
        }

        public void SendMessage(string text)
        {
            NetOutgoingMessage message = client.CreateMessage(text);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            client.FlushSendQueue();
        }

        public bool ParseMessage(string msg)
        {
            string[] args = msg.Split(',');
            int msgTest;
            if (int.TryParse(args[0], out msgTest))
            {
                MsgType msgType = (MsgType)msgTest;
                switch (msgType)
                {
                    case MsgType.NoAction:
                        MsgStruct ms = new MsgStruct();
                        ms.type = MsgType.NoAction;
                        ms.Message = "no";
                        incomingActionQueue.Enqueue(ms);
                        break;
                    case MsgType.PlayCard:
                        ms = new MsgStruct();
                        ms.type = MsgType.PlayCard;
                        ms.Message = args[1];
                        incomingActionQueue.Enqueue(ms);
                        break;
                    case MsgType.QueueCard:
                        ms = new MsgStruct();
                        ms.type = MsgType.PlayCard;
                        ms.Message = args[1];
                        incomingActionQueue.Enqueue(ms);
                        break;
                    case MsgType.GameStart:
                        teamColor = (TeamColor)int.Parse(args[1]);
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        public bool ReceiveMessage()
        {
            string reply = "";
            NetIncomingMessage im;
            while ((im = client.ReadMessage()) != null)
            {
                // handle incoming message
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.StatusChanged:
                    case NetIncomingMessageType.Data:
                        {
                            reply = im.ReadString();
                            return ParseMessage(reply);
                            break;
                        }
                    default:
                        break;
                }
                client.Recycle(im);
            }
            return false;
        }

        public void SendPrevActionToServer()
        {
            SendMessage(prevAction);
        }

        public void SendActionToServer()
        {
            if (outActionQueue.Count > 0)
            {
                prevAction = outActionQueue.Dequeue();
            }
            else
            {
                prevAction = (((int)MsgType.NoAction).ToString() + ",no");
            }
            SendMessage(prevAction);
        }

        private void StartGame()
        {
            DateTime startTime = DateTime.UtcNow.AddSeconds(2);
            //Queue and Send Game Start Message
            string response = (int)MsgType.GameStart + "," + startTime;
            SendMessage(response);
            ParseMessage(response);
        }
    }
}
