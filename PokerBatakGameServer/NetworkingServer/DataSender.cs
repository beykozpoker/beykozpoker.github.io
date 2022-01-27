using PokerBatakGameServer;
using PokerBatakGameServer.Bots;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkServer
{
    class DataSender
    {
        public static void ALERT_MESSAGES(int index, string msg)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.ALERT_MESSAGES);
            buffer.WriteString(msg);

            ClientManager.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }
        
        public static void CHECK_CONNECTION(int index)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.CHECK_CONNECTION);

            ClientManager.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void PLAYER_MESSAGING(int index, int player, byte[] msg, int roomstate)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.PLAYER_MESSAGING);

            buffer.WriteInteger(player);
            buffer.WriteInteger(roomstate);

            buffer.WriteInteger(msg.Length);
            buffer.WriteBytes(msg);

            ClientManager.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SIGN_IN(int index, int result)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.SIGN_IN);

            buffer.WriteInteger(result);

            ClientManager.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void LOG_IN(int index, Players _joinedPlayer, int DailyBonus, int HourlyBonus, int status)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)Enumerations.ServerPackets.LOG_IN);

            buffer.WriteInteger(status);
            if (status == 0)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(_joinedPlayer.UserID);
                buffer.WriteInteger(bytes.Length);
                buffer.WriteBytes(bytes);

                byte[] bytes1 = Encoding.UTF8.GetBytes(_joinedPlayer.User_Name);
                buffer.WriteInteger(bytes1.Length);
                buffer.WriteBytes(bytes1);

                byte[] bytes2 = Encoding.UTF8.GetBytes(_joinedPlayer.DisplayName);
                buffer.WriteInteger(bytes2.Length);
                buffer.WriteBytes(bytes2);

                buffer.WriteInteger(_joinedPlayer.Money);
                buffer.WriteInteger(_joinedPlayer.pictureIndex);
                buffer.WriteInteger(DailyBonus);
                buffer.WriteInteger(HourlyBonus);
            }

            ClientManager.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void LOG_OUT(int index)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.LOG_OUT);


            ClientManager.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void POKER_ROOM_ANSWER(int index)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.POKER_ROOM_ANSWER);

            buffer.WriteInteger(PlayerManager.active_Player[index].roomID);
            buffer.WriteInteger(PlayerManager.active_Player[index].roomChair);
            buffer.WriteInteger(RoomManager.POKERRooms[PlayerManager.active_Player[index].roomID].StartingBet);



            ClientManager.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void PLAYER_ENTERED_POKER_ROOM(int index, int newPlayerID)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.PLAYER_ENTERED_POKER_ROOM);

            buffer.WriteInteger(PlayerManager.active_Player[newPlayerID].pictureIndex);

            buffer.WriteInteger(PlayerManager.active_Player[newPlayerID].Money);
            //byte[] bytes2 = Encoding.UTF8.GetBytes(PlayerManager.active_Player[newPlayerID].Money);
            //buffer.WriteInteger(bytes2.Length);
            //buffer.WriteBytes(bytes2);


            buffer.WriteInteger(PlayerManager.active_Player[newPlayerID].roomChair);


            byte[] bytes1 = Encoding.UTF8.GetBytes(PlayerManager.active_Player[newPlayerID].User_Name);
            buffer.WriteInteger(bytes1.Length);
            buffer.WriteBytes(bytes1);

            byte[] bytes = Encoding.UTF8.GetBytes(PlayerManager.active_Player[newPlayerID].UserID);
            buffer.WriteInteger(bytes.Length);
            buffer.WriteBytes(bytes);

            ClientManager.SendDataTo(index, buffer.ToArray());


            buffer.Dispose();
        }

        public static void START_GAME(int index, int firstPlayerChair, int secondPlayerChair,
            int thirdPlayerChair, int minBet, int firstCard, int secondCard, Dictionary<int, PlayingPlayer> _playerIndex)
        {

            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)Enumerations.ServerPackets.START_GAME);

            buffer.WriteInteger(firstPlayerChair);
            buffer.WriteInteger(secondPlayerChair);
            buffer.WriteInteger(thirdPlayerChair);
            buffer.WriteInteger(firstCard);
            buffer.WriteInteger(secondCard);
            buffer.WriteInteger(minBet);
            buffer.WriteInteger(_playerIndex.Count);
            foreach (var player in _playerIndex)
                buffer.WriteInteger(player.Value.roomChair);

            ClientManager.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void END_GAME(int index, int WinnerChair, int price, int totalMoney)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.END_GAME);

            buffer.WriteInteger(WinnerChair);

            buffer.WriteInteger(price);

            buffer.WriteInteger(totalMoney);

            ClientManager.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void TURN_PLAYER(int turnPlayer, int roomID, int playerMoney, int RoundBet, int _maxBet)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.TURN_PLAYER);

            buffer.WriteInteger(turnPlayer);

            buffer.WriteInteger(RoundBet);

            buffer.WriteInteger(playerMoney);

            buffer.WriteInteger(_maxBet);

            try
            {
                foreach (var player in RoomManager.POKERRooms[roomID].playerIn_Room)
                {
                    ClientManager.SendDataTo(player.Value.ActiveID, buffer.ToArray());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            buffer.Dispose();

        }

        public static void REFRESH_MONEY(int betterMoney, int roomID, int PlayerChair, int playerBet)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.REFRESH_MONEY);

            buffer.WriteInteger(betterMoney);
            buffer.WriteInteger(PlayerChair);
            buffer.WriteInteger(playerBet);


            foreach (var player in RoomManager.POKERRooms[roomID].playerIn_Room)
            {
                ClientManager.SendDataTo(player.Value.ActiveID, buffer.ToArray());
            }


            buffer.Dispose();
        }

        public static void SEND_FIRST_THREE_CARDS(int index, List<int> Cards, int totalMoney)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.SEND_FIRST_THREE_CARDS);

            buffer.WriteInteger(Cards[0]);
            buffer.WriteInteger(Cards[1]);
            buffer.WriteInteger(Cards[2]);

            buffer.WriteInteger(totalMoney);

            ClientManager.SendDataTo(index, buffer.ToArray());

            buffer.Dispose();
        }

        public static void SEND_FOURTH_CARD(int index, List<int> Cards, int totalMoney)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.SEND_FOURTH_CARD);

            buffer.WriteInteger(Cards[3]);

            buffer.WriteInteger(totalMoney);

            ClientManager.SendDataTo(index, buffer.ToArray());

            buffer.Dispose();
        }

        public static void SEND_FIFTH_CARD(int index, List<int> Cards, int totalMoney)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.SEND_FIFTH_CARD);

            buffer.WriteInteger(Cards[4]);

            buffer.WriteInteger(totalMoney);

            ClientManager.SendDataTo(index, buffer.ToArray());



            buffer.Dispose();
        }

        public static void PLAYER_LEFT_POKER_ROOM(int index, int serverChair)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.PLAYER_LEFT_POKER_ROOM);

            buffer.WriteInteger(serverChair);



            ClientManager.SendDataTo(index, buffer.ToArray());

            buffer.Dispose();
        }

        public static void CHECK_HOURLY_BONUS(int index, bool check)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.CHECK_HOURLY_BONUS);

            int HourlyBonusTime = 3700;
            if (PlayerManager.Players[PlayerManager.active_Player[index].User_Name].HourlyBonus == DateTime.MinValue)
                HourlyBonusTime = 3700;
            else
                HourlyBonusTime = (int)DateTime.Now.Subtract(PlayerManager.Players[PlayerManager.active_Player[index].User_Name].HourlyBonus).TotalSeconds;
            
            
            int a = 0;
            if (HourlyBonusTime >= 3600)
                a = 0;
            else
                a = (int)(3600 - HourlyBonusTime);
            
            buffer.WriteInteger(a);


            if (check)
            {
                int x = PlayerManager.active_Player[index].Money;
                x += 150;
                PlayerManager.active_Player[index].Money = x;
                PlayerManager.Players[PlayerManager.active_Player[index].User_Name].Money = x;
            }


            buffer.WriteInteger(PlayerManager.active_Player[index].Money);
            //byte[] bytes2 = Encoding.UTF8.GetBytes(PlayerManager.active_Player[index].Money);
            //buffer.WriteInteger(bytes2.Length);
            //buffer.WriteBytes(bytes2);

            ClientManager.SendDataTo(index, buffer.ToArray());

            buffer.Dispose();
        }

        public static void CHECK_DAYLY_BONUS(int index, int DailyBonus)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.CHECK_DAYLY_BONUS);
            
            buffer.WriteInteger(DailyBonus);

            buffer.WriteInteger(PlayerManager.active_Player[index].Money);
            //byte[] bytes2 = Encoding.UTF8.GetBytes(PlayerManager.active_Player[index].Money);
            //buffer.WriteInteger(bytes2.Length);
            //buffer.WriteBytes(bytes2);

            ClientManager.SendDataTo(index, buffer.ToArray());

            buffer.Dispose();
        }

        public static void LEAVE_ROOM(int index, int DailyBonus, int HourlyBonus, int GameRoom)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.LEAVE_ROOM);

            
            buffer.WriteInteger(GameRoom);
            buffer.WriteInteger(DailyBonus);
            buffer.WriteInteger(HourlyBonus);

            buffer.WriteInteger(PlayerManager.active_Player[index].Money);

            ClientManager.SendDataTo(index, buffer.ToArray());

            buffer.Dispose();
        }




        #region BJTROOM
        public static void BJT_ROOM_ANSWER(int index)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.BJT_ROOM_ANSWER);

            buffer.WriteInteger(PlayerManager.active_Player[index].roomID);
            buffer.WriteInteger(PlayerManager.active_Player[index].roomChair);
            buffer.WriteInteger(RoomManager.BJTRooms[PlayerManager.active_Player[index].roomID].roomBet);

            ClientManager.SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static void PLAYER_ENTERED_BJT_ROOM(int index, int newPlayerID)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.PLAYER_ENTERED_BJT_ROOM);

            buffer.WriteInteger(PlayerManager.active_Player[newPlayerID].pictureIndex);

            buffer.WriteInteger(PlayerManager.active_Player[newPlayerID].Money);


            buffer.WriteInteger(PlayerManager.active_Player[newPlayerID].roomChair);


            byte[] bytes1 = Encoding.UTF8.GetBytes(PlayerManager.active_Player[newPlayerID].User_Name);
            buffer.WriteInteger(bytes1.Length);
            buffer.WriteBytes(bytes1);

            byte[] bytes = Encoding.UTF8.GetBytes(PlayerManager.active_Player[newPlayerID].UserID);
            buffer.WriteInteger(bytes.Length);
            buffer.WriteBytes(bytes);

            ClientManager.SendDataTo(index, buffer.ToArray());


            buffer.Dispose();
        }

        public static void START_BJT_GAME(int index, int roomID, List<int> playerCards, List<int> otherCards)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.START_BJT_GAME);

            buffer.WriteInteger(playerCards[0]);
            buffer.WriteInteger(playerCards[1]);

            buffer.WriteInteger(otherCards[0]);
            buffer.WriteInteger(otherCards[1]);


            ClientManager.SendDataTo(index, buffer.ToArray());

            buffer.Dispose();
        }

        public static void TURN_BJT_PLAYER(int turnPlayer, int roomID)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.TURN_BJT_PLAYER);

            buffer.WriteInteger(turnPlayer);

            foreach (var player in RoomManager.BJTRooms[roomID].RoomPlayers)
            {
                ClientManager.SendDataTo(player.Value.ActiveID, buffer.ToArray());
            }

            buffer.Dispose();
        }

        public static void BJ_TOURNAMENT_TAKE_CARD(int index, int cardNum, int roomID, int BET)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.BJ_TOURNAMENT_TAKE_CARD);

            buffer.WriteInteger(BET);

            buffer.WriteInteger(index);

            buffer.WriteInteger(cardNum);

            foreach (var player in RoomManager.BJTRooms[roomID].RoomPlayers)
            {
                ClientManager.SendDataTo(player.Value.ActiveID, buffer.ToArray());
            }

            buffer.Dispose();
        }
        
        public static void END_BJT_GAME(int winner, int price, int roomID, int winnerCase)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.END_BJT_GAME);

            buffer.WriteInteger(winner);
            buffer.WriteInteger(price);
            buffer.WriteInteger(winnerCase);

            foreach (var player in RoomManager.BJTRooms[roomID].RoomPlayers)
            {
                ClientManager.SendDataTo(player.Value.ActiveID, buffer.ToArray());
            }

            buffer.Dispose();
        }

        public static void PLAYER_LEFT_BJT_ROOM(int index)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.PLAYER_LEFT_BJT_ROOM);

            ClientManager.SendDataTo(index, buffer.ToArray());

            buffer.Dispose();
        }
        #endregion



        #region TURKISHPOKER
        public static void TP_ROOM_ANSWER(int index)
        {
            if(index > 0)
            {
                ByteBuffer buffer = new ByteBuffer();

                buffer.WriteInteger((int)Enumerations.ServerPackets.TP_ROOM_ANSWER);

                buffer.WriteInteger(PlayerManager.active_Player[index].roomID);
                buffer.WriteInteger(PlayerManager.active_Player[index].roomChair);
                buffer.WriteInteger(RoomManager.TurkishPOKERRooms[PlayerManager.active_Player[index].roomID].roomBet);

                ClientManager.SendDataTo(index, buffer.ToArray());
                buffer.Dispose();

                return;
            }
            else
            {
                BotPlayerManager.BeykozPokerRoomAnswer(index, PlayerManager.active_Player[index].roomID, PlayerManager.active_Player[index].roomChair);
            }
        }

        public static void PLAYER_ENTERED_TP_ROOM(int index, int newPlayerID)
        {
            if(index > 0)
            {
                ByteBuffer buffer = new ByteBuffer();

                buffer.WriteInteger((int)Enumerations.ServerPackets.PLAYER_ENTERED_TP_ROOM);

                buffer.WriteInteger(PlayerManager.active_Player[newPlayerID].pictureIndex);

                byte[] bytes2 = Encoding.UTF8.GetBytes(RoomManager.TurkishPOKERRooms[PlayerManager.active_Player[newPlayerID].roomID].playerIn_Room[PlayerManager.active_Player[newPlayerID].roomChair].roomMoney.ToString());
                //byte[] bytes2 = Encoding.UTF8.GetBytes(PlayerManager.active_Player[newPlayerID].Money);
                buffer.WriteInteger(bytes2.Length);
                buffer.WriteBytes(bytes2);


                buffer.WriteInteger(PlayerManager.active_Player[newPlayerID].roomChair);


                byte[] bytes1 = Encoding.UTF8.GetBytes(PlayerManager.active_Player[newPlayerID].User_Name);
                buffer.WriteInteger(bytes1.Length);
                buffer.WriteBytes(bytes1);

                byte[] bytes = Encoding.UTF8.GetBytes(PlayerManager.active_Player[newPlayerID].UserID);
                buffer.WriteInteger(bytes.Length);
                buffer.WriteBytes(bytes);

                byte[] bytes3 = Encoding.UTF8.GetBytes(PlayerManager.active_Player[newPlayerID].DisplayName);
                buffer.WriteInteger(bytes3.Length);
                buffer.WriteBytes(bytes3);

                ClientManager.SendDataTo(index, buffer.ToArray());


                buffer.Dispose();
            }
            
        }

        public static void PLAYER_LEFT_TP_ROOM(int leftPlayerChair, int roomID)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.PLAYER_LEFT_TP_ROOM);

            buffer.WriteInteger(leftPlayerChair);

            foreach (var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
                if (item.Value.ActiveID > 0)
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());

            buffer.Dispose();
        }

        public static void ASK_CARD_TO_PLAYER(int turnPlayer, int roomID, int cardID, int gameRound)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.ASK_CARD_TO_PLAYER);

            buffer.WriteInteger(turnPlayer);

            buffer.WriteInteger(-1);

            buffer.WriteInteger(gameRound);
            foreach (var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
                if (item.Value.ActiveID > 0 && item.Value.roomChair != turnPlayer)
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());

            buffer.Dispose();

            ///////////////////////////////////////////////////////////////////////////////

            ByteBuffer buffer2 = new ByteBuffer();

            buffer2.WriteInteger((int)Enumerations.ServerPackets.ASK_CARD_TO_PLAYER);

            buffer2.WriteInteger(turnPlayer);

            buffer2.WriteInteger(cardID);

            buffer2.WriteInteger(gameRound);

            if (RoomManager.TurkishPOKERRooms[roomID].playerIn_Room[turnPlayer].ActiveID > 0)
                ClientManager.SendDataTo(RoomManager.TurkishPOKERRooms[roomID].playerIn_Room[turnPlayer].ActiveID, buffer2.ToArray());
            else
                BotPlayerManager.AskCardToBot(RoomManager.TurkishPOKERRooms[roomID].playerIn_Room[turnPlayer].ActiveID, turnPlayer, cardID);


            buffer2.Dispose();
        }

        public static void TURN_BET_PLAYER(int turnPlayer, int roomID, int RoundBet, int _maxBet)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.TURN_BET_PLAYER);

            buffer.WriteInteger(turnPlayer);

            buffer.WriteInteger(RoundBet);

            buffer.WriteInteger(_maxBet);

            int Bot = -5000;
            bool BotC = false;

            foreach (var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
            {
                if (item.Value.ActiveID > 0)
                {
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());
                }
                else
                {
                    BotC = true;
                    Bot = item.Value.ActiveID;
                }
            }

            buffer.Dispose();

            if (BotC)
            {
                BotPlayerManager.TurnBetBOT(Bot, turnPlayer, RoundBet, _maxBet);
            }
        }

        public static void PLAYER_ACCEPT_CARD(int turnPlayer, int cardID, int answer, int roomID)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.PLAYER_ACCEPT_CARD);

            buffer.WriteInteger(turnPlayer);

            buffer.WriteInteger(-1);

            buffer.WriteInteger(answer);

            foreach (var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
                if (item.Value.ActiveID > 0 && item.Value.roomChair != turnPlayer)
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());

            buffer.Dispose();



            ByteBuffer buffer2 = new ByteBuffer();

            buffer2.WriteInteger((int)Enumerations.ServerPackets.PLAYER_ACCEPT_CARD);

            buffer2.WriteInteger(turnPlayer);

            buffer2.WriteInteger(cardID);

            buffer2.WriteInteger(answer);

            if (RoomManager.TurkishPOKERRooms[roomID].playerIn_Room[turnPlayer].ActiveID > 0)
                ClientManager.SendDataTo(RoomManager.TurkishPOKERRooms[roomID].playerIn_Room[turnPlayer].ActiveID, buffer2.ToArray());

            buffer2.Dispose();
        }

        public static void OPEN_FLOP_CARDS_BPR(int roomid, int i, int i2, int i3)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.OPEN_FLOP_CARDS_BPR);

            buffer.WriteInteger(i);
            buffer.WriteInteger(i2);
            buffer.WriteInteger(i3);

            foreach (var item in RoomManager.TurkishPOKERRooms[roomid].playerIn_Room)
                if (item.Value.ActiveID > 0)
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());

            buffer.Dispose();
        }

        public static void OPEN_TURN_CARD_BPR(int roomID, int i)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.OPEN_TURN_CARD_BPR);

            buffer.WriteInteger(i);

            foreach (var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
                if (item.Value.ActiveID > 0)
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());

            buffer.Dispose();
        }

        public static void OPEN_RIVER_CARD_BPR(int roomID, int i)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.OPEN_RIVER_CARD_BPR);

            buffer.WriteInteger(i);

            foreach (var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
                if (item.Value.ActiveID > 0)
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());

            buffer.Dispose();
        }

        public static void TPROOM_END_GAME(int index, int roomID)
        {
            if(index > 0)
            {
                ByteBuffer buffer = new ByteBuffer();

                buffer.WriteInteger((int)Enumerations.ServerPackets.TPROOM_END_GAME);


                ClientManager.SendDataTo(index, buffer.ToArray());

                buffer.Dispose();
            }
            
        }

        public static void SHOW_BPR_WINNER(int roomID, int winnerPlayer, int playerPrice, int playerMoney)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.SHOW_BPR_WINNER);

            buffer.WriteInteger(winnerPlayer);
            buffer.WriteInteger(playerPrice);
            buffer.WriteInteger(playerMoney);

            foreach (var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
                if (item.Value.ActiveID > 0)
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());

            buffer.Dispose();
        }

        public static void PLAYER_BETED_BPR(int player, int Bet, int PlayerMoney, int roomID, int betSound)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.PLAYER_BETED_BPR);

            buffer.WriteInteger(player);
            buffer.WriteInteger(Bet);
            buffer.WriteInteger(PlayerMoney);

            buffer.WriteInteger(betSound);


            foreach (var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
                if (item.Value.ActiveID > 0)
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());

            buffer.Dispose();
        }

        public static void START_BPGAME(int roomID, int firstbeter, int secondbeter, int firstbetterMoney, int secondBetermoney)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.START_BPGAME);

            buffer.WriteInteger(firstbeter);
            buffer.WriteInteger(secondbeter);
            buffer.WriteInteger(firstbetterMoney);
            buffer.WriteInteger(secondBetermoney);

            foreach (var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
            {
                if (item.Value.ActiveID > 0)
                {
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());
                }
                else
                {
                    BotPlayerManager.StartBOTGame(roomID, item.Value.ActiveID, firstbeter, secondbeter);
                }
            }
            
            buffer.Dispose();
        }

        public static void SHOW_TOTAL_BET(int roomID, int totalBet)
        {
            Console.WriteLine("DataSender/SHOW_TOTAL_BET");

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.SHOW_TOTAL_BET);
            buffer.WriteInteger(totalBet);

            foreach (var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
            {
                if (item.Value.ActiveID > 0)
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());
            }

            buffer.Dispose();
        }


        public static void PAYBACK(int player, int BackBet, int PlayerMoney, int roomID)
        {
                ByteBuffer buffer = new ByteBuffer();

                buffer.WriteInteger((int)Enumerations.ServerPackets.PAY_BACK);

                buffer.WriteInteger(player);
                buffer.WriteInteger(BackBet);
                buffer.WriteInteger(PlayerMoney);

                


            foreach(var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
            {
                if(item.Value.ActiveID > 0)
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());
            }


            buffer.Dispose();
        }


        public static void SHOW_PLAYER_CARDS(int player, int playerID, int card1, int card2, int card3, int roomID)
        {

            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((int)Enumerations.ServerPackets.SHOW_PLAYER_CARDS);

            buffer.WriteInteger(player);
            buffer.WriteInteger(card1);
            buffer.WriteInteger(card2);
            buffer.WriteInteger(card3);


            foreach (var item in RoomManager.TurkishPOKERRooms[roomID].playerIn_Room)
            {
                if (item.Value.ActiveID > 0 && item.Value.ActiveID != playerID)
                    ClientManager.SendDataTo(item.Value.ActiveID, buffer.ToArray());
            }

            buffer.Dispose();
        }
        #endregion
    }
}
