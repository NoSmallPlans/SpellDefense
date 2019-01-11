using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.Networking.Messaging;

namespace SpellDefense.Common
{
    public class Client
    {
        private NetClient client;
        Queue<MsgStruct> messageQueue;
        double deviceTimeDiff = 0;
        double travelTimeInSeconds = 0;
        public bool host;

        public Client()
        {
            messageQueue = new Queue<MsgStruct>();
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
            host = false;
        }

        public void SendMessage(string text)
        {
            NetOutgoingMessage message = client.CreateMessage(text);
            client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            client.FlushSendQueue();
        }

        public void ParseMessage(string msg)
        {
            string[] args = msg.Split(',');
            if (args.Length > 1)
            {
                int msgTest;
                if (int.TryParse(args[0], out msgTest))
                {
                    MsgType msgType = (MsgType)msgTest;
                    switch (msgType)
                    {
                        case MsgType.Matched:
                            host = (args[1] == "host");
                            if (host)
                                SendMessage((int)MsgType.ReqTime + "," + DateTime.UtcNow.ToString());
                            break;
                        case MsgType.ReqTime:
                            string hostTime = args[1];
                            string clientTime = DateTime.UtcNow.ToString();
                            string response = (int)MsgType.SendTime + "," + hostTime + "," + clientTime;
                            SendMessage(response);
                            break;
                        case MsgType.SendTime:
                            if (host)
                            {
                                //Calculate time difference between devices
                                DateTime now = DateTime.UtcNow;
                                DateTime hostDevTime = DateTime.Parse(args[1]);
                                DateTime clientDevTime = DateTime.Parse(args[2]);
                                travelTimeInSeconds = (now - hostDevTime).TotalSeconds;
                                deviceTimeDiff = (hostDevTime - clientDevTime).TotalSeconds;
                                deviceTimeDiff -= travelTimeInSeconds;
                                //Send time difference to opponent
                                response = (int)MsgType.SendTime + "," + (deviceTimeDiff).ToString();
                                SendMessage(response);
                            }
                            else
                            {
                                //Receive time diff from opponent
                                deviceTimeDiff = double.Parse(args[1]) * -1;
                                StartGame();
                            }
                            break;
                        case MsgType.PlayCard:
                            MsgStruct ms = new MsgStruct();
                            ms.type = MsgType.PlayCard;
                            ms.timeStamp = DateTime.Parse(args[2]);
                            ms.timeStamp = ms.timeStamp.AddSeconds(deviceTimeDiff);
                            ms.Message = args[1];
                            messageQueue.Enqueue(ms);
                            break;
                        case MsgType.QueueCard:
                            ms = new MsgStruct();
                            ms.type = MsgType.PlayCard;
                            ms.timeStamp = DateTime.Parse(args[2]);
                            ms.Message = args[1];
                            messageQueue.Enqueue(ms);
                            break;
                        case MsgType.GameStart:
                            ms = new MsgStruct();
                            ms.type = MsgType.GameStart;
                            ms.timeStamp = DateTime.Parse(args[1]);
                            if(host)
                            {
                                ms.timeStamp = ms.timeStamp.AddSeconds(deviceTimeDiff);
                            }
                            messageQueue.Enqueue(ms);
                            break;
                        default:
                            return;
                    }
                }
            }
        }

        public string ReceiveMessage()
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
                            ParseMessage(reply);
                            break;
                        }
                    default:
                        break;
                }
                client.Recycle(im);
            }
            return reply;
        }

        public MsgStruct CheckQueue()
        {
            MsgStruct ms = new MsgStruct();
            if (messageQueue.Count > 0)
            {
                ms = messageQueue.Peek();
                if (ms.timeStamp <= DateTime.UtcNow)
                {
                    messageQueue.Dequeue();
                }
                else
                {
                    ms = new MsgStruct();
                }
            }
            else
            {
                ms.Message = "none";
            }
            return ms;
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
