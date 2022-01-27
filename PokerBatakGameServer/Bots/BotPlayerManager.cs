using System;
using System.Collections.Generic;
using System.Text;

namespace PokerBatakGameServer.Bots
{
    class BotPlayerManager
    {
        public static Dictionary<int, BotPlayer> Bots = new Dictionary<int, BotPlayer>();

        public static void CreateBotPlayer(string _username, string _password, int _roomID, int minBet)
        {
            BotPlayer newPlayer = new BotPlayer();
            newPlayer.User_Name = _username;

            newPlayer.DisplayName = _username;
            
            newPlayer.Password = _password;
            newPlayer.pictureIndex = 2;
            newPlayer.UserID = (111111 + PlayerManager.Players.Count).ToString();
            newPlayer.Money = minBet * 100;
            newPlayer.DaylyBonus = DateTime.MinValue;
            newPlayer.HourlyBonus = DateTime.MinValue;
            newPlayer.ActiveID = (0 - Bots.Count) -1;

            

            //PlayerManager.Players.Add(_username, newPlayer);
            Bots.Add(newPlayer.ActiveID, newPlayer);
            PlayerManager.active_Player.Add(newPlayer.ActiveID, newPlayer);
            //PlayerManager.LoginPlayer(_username, newPlayer.ActiveID);
            RoomManager.Add_BOT_TP_Room(newPlayer.ActiveID, _roomID);


            RoomManager.TurkishPOKERRooms[_roomID].SetBotINFO(newPlayer.ActiveID, newPlayer.UserID, newPlayer.User_Name);
        }

        public static void StartBOTGame(int _roomID, int botactiveID, int firstBetter, int secondBetter)
        {

        }

        public static void AskCardToBot(int botactiveID, int turnPlayer, int card)
        {
            if (Bots[botactiveID].roomChair == turnPlayer)
            {
                Bots[botactiveID].CardTurn(turnPlayer);
            }
        }

        public static void BeykozPokerRoomAnswer(int botactiveID, int roomID, int roomChair)
        {
            Bots[botactiveID].setBotINFO(roomID, roomChair);
        }

        public static void TurnBetBOT(int ActiveID, int turnPlayer, int RoundBet, int _maxBet)
        {
            Bots[ActiveID].BetTurn(turnPlayer, RoundBet, _maxBet);
        }

        public static void StartGame(int ActiveID, int money)
        {
            Bots[ActiveID].StartGame(money);
        }

        public static void LeftRoom()
        {

        }
    }
}
