using PlayerIO.GameLibrary;
using SamServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamAssemblyExample
{
    internal class AddNewPlayer : IFunction
    {
        private int _nbPlayer;
        public void Execute(Player player, Message message, GameCode game)
        {
            _nbPlayer++;
            game.Broadcast("NewPlayerJoin", player.ConnectUserId, _nbPlayer);
        }
    }
}
