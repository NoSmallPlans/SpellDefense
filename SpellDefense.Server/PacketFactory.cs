using System;
using SpellDefense.Server.Commands;
using static SpellDefense.Common.Networking.Messaging;

namespace SpellDefense.Server
{
    class PacketFactory
    {
        public static ICommand GetCommand(MsgType msgType)
        {
            switch (msgType)
            {
                case MsgType.GameStart:
                    return new LoginCommand();
                case MsgType.Matched:
                    return new PlayerPositionCommand();
                case MsgType.PlayCard:
                    return new AllPlayersCommand();
                default:
                    throw new ArgumentOutOfRangeException("msgType");
            }
        }
    }
}
