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
            NoAction,
            Matched,
            GameStart,
            PlayCard,
            QueueCard
        }
        public struct MsgStruct
        {
            public string Message;
            public MsgType type;
        }
    }
}
