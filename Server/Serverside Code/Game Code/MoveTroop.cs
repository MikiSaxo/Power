using PlayerIO.GameLibrary;
using SamServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamAssemblyExample
{
    internal class MoveTroop : IFunction
    {

        public void Execute(Player player, Message message, GameCode game)
        {
            int troopID = message.GetInt(0);
            string newCell = message.GetString(1);
            int troopColor = message.GetInt(2);

            game.Broadcast("MoveTroop", player.ConnectUserId, troopID, newCell, troopColor);
        }
    }
}
