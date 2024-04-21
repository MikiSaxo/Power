using PlayerIO.GameLibrary;
using SamServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamAssemblyExample
{
    internal class ChooseColorPlayerName : IFunction
    {
        public void Execute(Player player, Message message, GameCode game)
        {
            var playerName = message.GetString(0);
            var index = message.GetInt(1);

            Console.WriteLine($"Choose Color Player Name : {playerName} -- Color Index : {index}");

            game.Broadcast("ChooseColorPlayerName", player.ConnectUserId, playerName, index);
        }
    }
}
