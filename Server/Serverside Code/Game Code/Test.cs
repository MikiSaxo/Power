using PlayerIO.GameLibrary;
using SamServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamAssemblyExample
{
    internal class Test : IFunction
    {
        public void Execute(Player player, Message message, GameCode game)
        {
            Console.WriteLine($"{message.GetInt(0)}  {message.GetString(1)}");
        }
    }
}
