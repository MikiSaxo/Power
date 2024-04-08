using PlayerIO.GameLibrary;
using SamServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamAssemblyExample
{
    internal interface IFunction
    {
        void Execute(Player player, Message message, GameCode game);
    }
}
