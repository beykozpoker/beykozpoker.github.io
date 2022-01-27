using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PokerBatakGameServer.Bots
{
    public class BotPlayer : Players
    {
        //bool bot;
        int gameCount;

        public void setBotINFO(int _roomID, int playerChair)
        {
            roomID = _roomID;
            roomChair = playerChair;
            gameCount = 0;
        }

        public void StartGame(int money)
        {
            Console.WriteLine("BotPlayer / startGame");
            gameCount++;
            EndGame(money);
        }

        public void BetTurn(int turnPlayer, int roundBet, int _maxBet)
        {
            
            Thread.Sleep(2300);
            if (roomChair == turnPlayer)
            {
                RoomManager.TurkishPOKERRooms[roomID].ReceiveBets(ActiveID, 0);
            }
                
        }

        public void CardTurn(int turnPlayer)
        {
            Thread.Sleep(2100);

            if (roomChair == turnPlayer)
            {
                RoomManager.TurkishPOKERRooms[roomID].ReceiveCardAnswers(ActiveID, true);
            }
        }

        public void EndGame(int money)
        {
            if (money <= 0)
            {
                RoomManager.TurkishPOKERRooms[roomID].PlayerLeftRoom(ActiveID);
            }
            else if (gameCount > 4)
            {
                RoomManager.TurkishPOKERRooms[roomID].PlayerLeftRoom(ActiveID);
            }
        }
    }
}
