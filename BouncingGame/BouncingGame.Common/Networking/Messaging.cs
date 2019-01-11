using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellDefense.Common.Networking
{
    public class Messaging
    {
        public enum MsgType
        {
            Matched,
            GameStart,
            PlayCard,
            ReqTime,
            SendTime,
            QueueCard
        }
        public struct MsgStruct
        {
            public DateTime timeStamp;
            public string Message;
            public MsgType type;
        }
    }
}
