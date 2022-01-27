using PokerBatakGameServer;
using System;
using System.Collections.Generic;

namespace NetworkServer
{
    public static class DataReceiver
    {

        public static void ALERT_MESSAGES(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();
            string msg = buffer.ReadString();

            buffer.Dispose();

            Console.WriteLine(msg);

        }

        #region Messaging
        public static void RECEIVE_PLAYER_MESSAGE(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int totalBytes = buffer.ReadInteger();
            byte[] msgByte = buffer.ReadBytes(totalBytes);

            buffer.Dispose();

            PlayerManager.PlayerMessaged(connectionID, msgByte);

        }
        #endregion


        public static void BET_PASS(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();
            int BET = buffer.ReadInteger();

            buffer.Dispose();



            RoomManager.POKERRooms[PlayerManager.active_Player[connectionID].roomID]
                .GetBet(PlayerManager.active_Player[connectionID].roomChair, -1);

        }

        public static void BET_OK(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int BET = buffer.ReadInteger();

            buffer.Dispose();


            try
            {
                RoomManager.POKERRooms[PlayerManager.active_Player[connectionID].roomID]
                .GetBet(PlayerManager.active_Player[connectionID].roomChair, BET);
            }
            catch (Exception e)
            {
                Console.WriteLine("hataaaa BET_OK: " + e);
                throw;
            }
            

        }

        public static void BET_UP(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();
            int BET = buffer.ReadInteger();

            buffer.Dispose();

            RoomManager.POKERRooms[PlayerManager.active_Player[connectionID].roomID]
                .GetBet(PlayerManager.active_Player[connectionID].roomChair, BET);
        }

        public static void CREATE_NEW_ACCOUNT(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int totalBytes = buffer.ReadInteger();
            byte[] userName = buffer.ReadBytes(totalBytes);

            totalBytes = buffer.ReadInteger();
            byte[] password = buffer.ReadBytes(totalBytes);

            string _userName = System.Text.Encoding.UTF8.GetString(userName);
            string _password = System.Text.Encoding.UTF8.GetString(password);

            buffer.Dispose();

            PlayerManager.CreateNewAccount(_userName, _password, connectionID, false);

        }

        public static void LOG_IN(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int enterenceStatus = buffer.ReadInteger();
            if(enterenceStatus == 1)
            {
                int totalBytes = buffer.ReadInteger();
                byte[] userName = buffer.ReadBytes(totalBytes);

                totalBytes = buffer.ReadInteger();
                byte[] password = buffer.ReadBytes(totalBytes);

                string _userName = System.Text.Encoding.UTF8.GetString(userName);
                string _password = System.Text.Encoding.UTF8.GetString(password);

                buffer.Dispose();

                PlayerManager.LoginPlayer(_userName, _password, connectionID);
            }
            else if(enterenceStatus == 0)
            {
                buffer.Dispose();
                PlayerManager.CreateNewAccount(connectionID);
            }
            else if(enterenceStatus == 2)
            {
                int totalBytes = buffer.ReadInteger();
                byte[] userName = buffer.ReadBytes(totalBytes);

                totalBytes = buffer.ReadInteger();
                byte[] password = buffer.ReadBytes(totalBytes);

                string _userName = System.Text.Encoding.UTF8.GetString(userName);
                string _userFBID = System.Text.Encoding.UTF8.GetString(password);

                buffer.Dispose();

                PlayerManager.LoginPlayer(_userName, _userFBID, connectionID, true);
                //PlayerManager.LoginPlayer(_userName, _password, connectionID);
            }
        }


        public static void LOG_OUT(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int totalBytes = buffer.ReadInteger();
            byte[] userName = buffer.ReadBytes(totalBytes);

            string _userName = System.Text.Encoding.UTF8.GetString(userName);


            buffer.Dispose();
            PlayerManager.LogoutPlayer(connectionID);
        }

        public static void POKER_ROOM_REQUEST(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();
            int minBet = 0;
            try
            {
                minBet = buffer.ReadInteger();
            }
            catch (Exception)
            {
                minBet = 2;
            }

            buffer.Dispose();

            RoomManager.AddPlayerToPOKERRoom(connectionID, minBet);

        }

        public static void CHANGE_IMG(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int newPicIndex = buffer.ReadInteger();

            buffer.Dispose();

            PlayerManager.ChangePlayerPic(connectionID, newPicIndex);

        }

        public static void CHANGE_NAME(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int totalBytes = buffer.ReadInteger();
            byte[] newDisplayName = buffer.ReadBytes(totalBytes);

            string _newDisplayName = System.Text.Encoding.UTF8.GetString(newDisplayName);


            buffer.Dispose();
            PlayerManager.ChangePlayerDisplayName(connectionID, _newDisplayName);
        }

        public static void COLLECT_REEDEM(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int totalBytes = buffer.ReadInteger();
            byte[] reedemCode = buffer.ReadBytes(totalBytes);

            string _reedemCode = System.Text.Encoding.UTF8.GetString(reedemCode);


            buffer.Dispose();
            PlayerManager.CollectReedem(connectionID, _reedemCode);
        }

        public static void CHECK_HOURLY_BONUS(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();


            buffer.Dispose();

            DataSender.CHECK_HOURLY_BONUS(connectionID, false);

        }

        public static void COLLECT_HOURLY_BONUS(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();


            buffer.Dispose();

            PlayerManager.PlayerCollectedHourlyBonus(connectionID);

        }

        public static void COLLECT_DAILY_BONUS(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();


            buffer.Dispose();

            PlayerManager.PlayerCollectedDailyBonus(connectionID);

        }

        public static void LEAVE_ROOM(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int gameRoom = buffer.ReadInteger();

            buffer.Dispose();

            PlayerManager.LeaveRoom(connectionID, gameRoom);

        }

        public static void BJ_TOURNAMENT_ROOM_REQUEST(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();
            int minBet = 0;
            try
            {
                minBet = buffer.ReadInteger();
            }
            catch (Exception)
            {
                minBet = 10;
            }

            buffer.Dispose();

            RoomManager.AddPlayerToBJTRoom(connectionID, minBet);

        }

        public static void BJ_TOURNAMENT_TAKE_CARD(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();
            
            buffer.Dispose();
            RoomManager.BJTRooms[PlayerManager.active_Player[connectionID].roomID].PlayerTakeCard(connectionID);
        }

        public static void BJ_TOURNAMENT_BETDOUBLE(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();

            int x = buffer.ReadInteger();

            buffer.Dispose();


            if(x == 2)
                RoomManager.BJTRooms[PlayerManager.active_Player[connectionID].roomID].PlayerBetingDouble(connectionID);
            if(x == 3)
                RoomManager.BJTRooms[PlayerManager.active_Player[connectionID].roomID].PlayerBetingTrible(connectionID);

        }

        public static void BJ_TOURNAMENT_OK(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();

            buffer.Dispose();
            
            RoomManager.BJTRooms[PlayerManager.active_Player[connectionID].roomID].PlayerOK(connectionID);

        }

        #region BEYKOZPOKER
        public static void TURKISH_POKER_ROOM_REQUEST(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();
            int minBet = 0;
            try
            {
                minBet = buffer.ReadInteger();
            }
            catch (Exception)
            {
                minBet = 4;
            }

            buffer.Dispose();

            RoomManager.AddPlayerTo_TP_Room(connectionID, minBet);
        }

        public static void RECEIVE_CARD_ANSWERR(int connectionID, byte[] data)
        {

            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int _answer = buffer.ReadInteger();

            buffer.Dispose();

            bool answer;
            int roomID = PlayerManager.active_Player[connectionID].roomID;
            if (_answer == 1)
                answer = true;
            else
                answer = false;

            RoomManager.TurkishPOKERRooms[roomID].ReceiveCardAnswers(connectionID, answer);

            
        }

        public static void RECEIVE_BET(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int bet = buffer.ReadInteger();

            buffer.Dispose();


            int roomID = PlayerManager.active_Player[connectionID].roomID;


            RoomManager.TurkishPOKERRooms[roomID].ReceiveBets(connectionID, bet);


        }

        public static void GIVE_REEDEM(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packetID = buffer.ReadInteger();

            int product = buffer.ReadInteger();

            buffer.Dispose();


            int roomID = PlayerManager.active_Player[connectionID].roomID;

            PlayerManager.PlayerPayProduct(connectionID, product);
        }
        #endregion
    }
}