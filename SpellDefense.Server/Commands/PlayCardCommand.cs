using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpellDefense.Common.Networking.Messaging;

namespace SpellDefense.Server.Commands
{
    class PlayCardCommand : ICommand
    {
        public void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, Player player, Game game)
        {
            var data = inc.ReadByte();
            if (data == (byte)MsgType.PlayCard)
            {

            }
        }
    }
}
