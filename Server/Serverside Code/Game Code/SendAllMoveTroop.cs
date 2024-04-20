using PlayerIO.GameLibrary;
using SamServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamAssemblyExample
{
    internal class SendAllMoveTroop : IFunction
    {
        GameCode _gameCode;
        int _count;
        public SendAllMoveTroop(GameCode gameCode)
        {
            _gameCode = gameCode;
        }

        public void Execute(Player player, Message message, GameCode game)
        {
            _count++;

            if (_count == _gameCode.PlayerCount)
            {
                game.Broadcast("MoveAllTroops");
            }
        }
    }
}
