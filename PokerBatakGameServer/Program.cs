using NetworkServer;
using PokerBatakGameServer.SaveLoad;
using PokerBatakGameServer.TurkishPokerRoomClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace PokerBatakGameServer
{
    class Program
    {
        
        static void Main(string[] args)
        {

            ServerTCP.InitializeNetwork();


            //SaveSystem.SaveData(PlayerManager.Players);
            DataManager dataManager = SaveSystem.LoadData();

            if (dataManager != null)
                PlayerManager.Players = dataManager.players;

            //PlayerManager.Players["mmc"].Money = 15000;
            //PlayerManager.Players["mlh"].Money = 300;
            //SaveSystem.SaveData(PlayerManager.Players);


            Console.ReadLine();
        }

    }
}