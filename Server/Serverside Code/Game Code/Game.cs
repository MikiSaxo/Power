﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using PlayerIO.GameLibrary;
using SamAssemblyExample;

namespace SamServer
{
    public class Player : BasePlayer
    {
        public float posx = 0;
        public float posz = 0;
    }

    [RoomType("SamServer")]
    public class GameCode : Game<Player>
    {
        private Dictionary<string, IFunction> _functions = new Dictionary<string, IFunction>();

        // This method is called when an instance of your the game is created
        public override void GameStarted()
        {
            // anything you write to the Console will show up in the 
            // output window of the development server
            Console.WriteLine("Game is started: " + RoomId);

            //Type[] _types = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
            //                 from assemblyType in domainAssembly.GetTypes()
            //                 where assemblyType.IsSubclassOf(typeof(IFunction))
            //                 select assemblyType).ToArray();

            //foreach (var item in _types)
            //{
            //    IFunction func = (IFunction)Activator.CreateInstance(item);
            //    _functions.Add(func.GetName(), func);
            //}


            _functions.Add("MoveTroop", new MoveTroop());
            _functions.Add("TEST", new Test());
            _functions.Add("ChooseColorPlayerName", new ChooseColorPlayerName());
            _functions.Add("Want_EndOfTurn", new AddNewVoteEndOfTurn());
            _functions.Add("AllMoveTroopSend", new SendAllMoveTroop(this));
        }

        // This method is called when the last player leaves the room, and it's closed down.
        public override void GameClosed()
        {
            Console.WriteLine("RoomId: " + RoomId);
        }

        // This method is called whenever a player joins the game
        public override void UserJoined(Player player)
        {
            foreach (Player pl in Players)
            {
                if (pl.ConnectUserId != player.ConnectUserId)
                {
                    //pl.Send("PlayerJoined", player.ConnectUserId, 0, 0);
                    //player.Send("PlayerJoined", pl.ConnectUserId, pl.posx, pl.posz);
                }
            }
            Broadcast("NewPlayerJoin", player.ConnectUserId, PlayerCount);
            Console.WriteLine("New Player Join");
        }

        // This method is called when a player leaves the game
        public override void UserLeft(Player player)
        {
            Broadcast("PlayerLeft", player.ConnectUserId);
            Console.WriteLine($"Player : {player} Quit");
        }

        // This method is called when a player sends a message into the server code
        public override void GotMessage(Player player, Message message)
        {
            if (!_functions.TryGetValue(message.Type, out IFunction func))
                return;

            func.Execute(player, message, this);
        }
    }
}