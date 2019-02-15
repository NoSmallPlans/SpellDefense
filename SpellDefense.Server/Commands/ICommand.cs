//------------------------------------------------------
// 
// Copyright - (c) - 2014 - Mille Boström 
//
// Youtube channel - http://www.speedcoding.net
//------------------------------------------------------

using Lidgren.Network;

namespace SpellDefense.Server.Commands
{
    interface ICommand
    {
        void Run(ManagerLogger managerLogger, Server server, NetIncomingMessage inc, Player player, Game game);
    }
}
