using NetworkServer;
using PokerBatakGameServer.SaveLoad;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerBatakGameServer
{
    public class PlayerManager
    {
        public static Dictionary<string, Players> Players = new Dictionary<string, Players>();
        public static Dictionary<int, Players> active_Player = new Dictionary<int, Players>();


        public static void CreateNewAccount(string _username, string _password, int ConnectionID, bool Fbuser)
        {
            if (!Fbuser)
            {
                if (!Players.ContainsKey(_username))
                {
                    Players newPlayer = new Players();
                    newPlayer.User_Name = _username;

                    newPlayer.DisplayName = _username;

                    newPlayer.Password = _password;
                    newPlayer.pictureIndex = 0;
                    newPlayer.UserID = (111111 + Players.Count).ToString();
                    newPlayer.Money = 100;
                    newPlayer.DaylyBonus = DateTime.MinValue;
                    newPlayer.HourlyBonus = DateTime.MinValue;
                    newPlayer.guest = false;
                    newPlayer.FBUser = false;

                    Players.Add(_username, newPlayer);

                    LoginPlayer(_username, _password, ConnectionID);

                }
                else
                {
                    DataSender.LOG_IN(ConnectionID, null, 0, 0, 3);
                    //DataSender.SIGN_IN(ConnectionID, 0);
                }
            }
            else if (Fbuser)
            {
                if (!Players.ContainsKey(_username))
                {
                    Players newPlayer = new Players();
                    newPlayer.User_Name = _password;
                    newPlayer.Password = _username;

                    newPlayer.DisplayName = _username;

                    newPlayer.pictureIndex = 0;
                    newPlayer.UserID = (111111 + Players.Count).ToString();
                    newPlayer.Money = 100;
                    newPlayer.DaylyBonus = DateTime.MinValue;
                    newPlayer.HourlyBonus = DateTime.MinValue;
                    newPlayer.guest = false;
                    newPlayer.FBUser = false;

                    Players.Add(newPlayer.User_Name, newPlayer);

                    LoginPlayer(_username, _password, ConnectionID, true);

                }
                else
                {
                    Console.WriteLine("fb user already registered.");
                    DataSender.LOG_IN(ConnectionID, null, 0, 0, 3);
                    //DataSender.SIGN_IN(ConnectionID, 0);
                }
            }


            SaveSystem.SaveData(Players);
        }

        public static void CreateNewAccount(int ConnectionID)
        {
            Players newPlayer = new Players();
            newPlayer.UserID = (111111 + Players.Count).ToString();
            newPlayer.User_Name = newPlayer.UserID;
            newPlayer.Password = "00" + newPlayer.UserID + "00";

            newPlayer.DisplayName = newPlayer.User_Name;

            newPlayer.pictureIndex = 0;
            newPlayer.Money = 100;
            newPlayer.DaylyBonus = DateTime.MinValue;
            newPlayer.HourlyBonus = DateTime.MinValue;
            newPlayer.guest = true;

            Players.Add(newPlayer.User_Name, newPlayer);


            LoginPlayer(newPlayer.User_Name, newPlayer.Password, ConnectionID);


        }

        public static void LoginPlayer(string _username, string _password, int ConnectionID)
        {
            if (Players.ContainsKey(_username))
            {
                if (active_Player.ContainsKey(Players[_username].ActiveID))
                {
                    LogoutPlayer(ConnectionID);
                    Console.WriteLine("User Already Loged In");
                    LoginPlayer(_username, _password, ConnectionID);
                    return;
                }

                if (Players[_username].Password == _password)
                {
                    Players[_username].ActiveID = ConnectionID;
                    active_Player.Add(Players[_username].ActiveID, Players[_username]);

                    DateTime today = DateTime.Today;
                    int dailyBonus = 2;
                    if (today != Players[_username].DaylyBonus)
                        dailyBonus = 1;
                    else
                        dailyBonus = 0;


                    int HourlyBonusTime = 4000;
                    if (Players[_username].HourlyBonus == DateTime.MinValue)
                        HourlyBonusTime = 3700;
                    else
                        HourlyBonusTime = (int)DateTime.Now.Subtract(Players[_username].HourlyBonus).TotalSeconds;

                    int a = 0;
                    if (HourlyBonusTime >= 3600)
                        a = 0;
                    else
                        a = (int)(3600 - HourlyBonusTime);
                    
                    DataSender.LOG_IN(ConnectionID, Players[_username], dailyBonus, a, 0);
                }
                else
                {
                    DataSender.LOG_IN(ConnectionID, Players[_username], 0, 0, 1);
                }
            }
            else
            {
                DataSender.LOG_IN(ConnectionID, null, 0, 0, 2);
            }
        }

        public static void LoginPlayer(string _username, string FbuserID, int ConnectionID, bool FBUser)
        {
            if (Players.ContainsKey(FbuserID))
            {
                if (active_Player.ContainsKey(Players[FbuserID].ActiveID))
                {
                    LogoutPlayer(ConnectionID);
                    Console.WriteLine("User Already Loged In");
                    LoginPlayer(_username, FbuserID, ConnectionID, true);
                    return;
                }

                Players[FbuserID].ActiveID = ConnectionID;
                active_Player.Add(Players[FbuserID].ActiveID, Players[FbuserID]);

                DateTime today = DateTime.Today;
                int dailyBonus = 2;
                if (today != Players[FbuserID].DaylyBonus)
                    dailyBonus = 1;
                else
                    dailyBonus = 0;


                int HourlyBonusTime = 4000;
                if (Players[FbuserID].HourlyBonus == DateTime.MinValue)
                    HourlyBonusTime = 3700;
                else
                    HourlyBonusTime = (int)DateTime.Now.Subtract(Players[FbuserID].HourlyBonus).TotalSeconds;

                int a = 0;
                if (HourlyBonusTime >= 3600)
                    a = 0;
                else
                    a = (int)(3600 - HourlyBonusTime);

                DataSender.LOG_IN(ConnectionID, Players[FbuserID], dailyBonus, a, 0);
            }
            else
            {
                CreateNewAccount(_username, FbuserID, ConnectionID, true);
            }

        }

        public static void LogoutPlayer(int connectionID)
        {
            if(active_Player[connectionID].RoomState == 1 && active_Player[connectionID].IsInRoom)
            {
                try
                {
                    RoomManager.POKERRooms[active_Player[connectionID].roomID].LeftPlayer(connectionID,
                        active_Player[connectionID].roomChair);
                }
                catch (Exception)
                {
                    Console.WriteLine("Kullanıcı POKER Odasında Değil ||| PlayerManager\\LogoutPlayer");
                }
            }
            else if (active_Player[connectionID].RoomState == 2 && active_Player[connectionID].IsInRoom)
            {
                Console.WriteLine("Kullanıcı BJT Odasında Değil ||| PlayerManager\\LogoutPlayer");
                try
                {
                    RoomManager.BJTRooms[active_Player[connectionID].roomID].LeftRoom(connectionID);
                }
                catch (Exception)
                {
                    Console.WriteLine("Kullanıcı BJT Odasında Değil ||| PlayerManager\\LogoutPlayer");
                }
            }
            else if (active_Player[connectionID].RoomState == 3 && active_Player[connectionID].IsInRoom)
            {
                try
                {
                    RoomManager.TurkishPOKERRooms[active_Player[connectionID].roomID].PlayerLeftRoom(connectionID);
                }
                catch (Exception)
                {
                    Console.WriteLine("Kullanıcı TurkishPOKERRooms Odasında Değil ||| PlayerManager\\LogoutPlayer");
                }
            }


            string _username = active_Player[connectionID].User_Name;
            try
            {
                if (!active_Player.ContainsKey(connectionID))
                {
                    Console.WriteLine("player kayıtlı değil...");
                    return;
                }
                    

                
                Players[_username].Money = active_Player[connectionID].Money;
                active_Player.Remove(connectionID);
                Players[_username].ActiveID = 0000;
                Players[_username].IsInRoom = false;
                Players[_username].RoomState = 0000;
                Players[_username].LastOnline = DateTime.Now;
                Console.WriteLine(_username + "Left Game" + DateTime.Now);

                if(ClientManager.client.ContainsKey(connectionID))
                    DataSender.LOG_OUT(connectionID);
            }
            catch (Exception)
            {
                Console.WriteLine("player server'da değil...");
            }

            if (Players[_username].guest)
            {
                Players.Remove(_username);
            }
        }

        public static void LeaveRoom(int connectionID, int game)
        {
            switch (game)
            {
                case 1:
                    if (active_Player[connectionID].IsInRoom)
                    {
                        try
                        {
                            RoomManager.POKERRooms[active_Player[connectionID].roomID].LeftPlayer(connectionID,
                                active_Player[connectionID].roomChair);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Kullanıcı Odada Değil ||| PlayerManager\\LogoutPlayer");
                        }
                        DateTime today = DateTime.Today;
                        int dailyBonus = 2;
                        if (today != Players[active_Player[connectionID].User_Name].DaylyBonus)
                            dailyBonus = 1;
                        else
                            dailyBonus = 0;

                        int HourlyBonusTime = 3700;
                        if (Players[active_Player[connectionID].User_Name].HourlyBonus == DateTime.MinValue)
                            HourlyBonusTime = 3700;
                        else
                            HourlyBonusTime = (int)DateTime.Now.Subtract(Players[active_Player[connectionID].User_Name].HourlyBonus).TotalSeconds;

                        int a = 0;
                        if (HourlyBonusTime >= 3600)
                            a = 0;
                        else
                            a = (int)(3600 - HourlyBonusTime);

                        DataSender.LEAVE_ROOM(connectionID, dailyBonus, a, 1);
                    }
                    break;
                case 2:
                    if (active_Player[connectionID].IsInRoom)
                    {
                        try
                        {

                            RoomManager.BJTRooms[active_Player[connectionID].roomID].LeftRoom(connectionID);
                            Console.WriteLine("bjt roomdan ayrılındı");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Kullanıcı 2 numarali Odada Değil ||| PlayerManager\\LogoutPlayer");
                        }

                        DateTime today = DateTime.Today;
                        int dailyBonus = 2;
                        if (today != Players[active_Player[connectionID].User_Name].DaylyBonus)
                            dailyBonus = 1;
                        else
                            dailyBonus = 0;

                        int HourlyBonusTime = 3700;
                        if (Players[active_Player[connectionID].User_Name].HourlyBonus == DateTime.MinValue)
                            HourlyBonusTime = 3700;
                        else
                            HourlyBonusTime = (int)DateTime.Now.Subtract(Players[active_Player[connectionID].User_Name].HourlyBonus).TotalSeconds;

                        int a = 0;
                        if (HourlyBonusTime >= 3600)
                            a = 0;
                        else
                            a = (int)(3600 - HourlyBonusTime);

                        DataSender.LEAVE_ROOM(connectionID, dailyBonus, a, 2);
                    }
                    break;
                case 3:
                    if (active_Player[connectionID].IsInRoom)
                    {
                        try
                        {

                            RoomManager.TurkishPOKERRooms[active_Player[connectionID].roomID].PlayerLeftRoom(connectionID);
                            Console.WriteLine("Player Left Turkish Poker..");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Console.WriteLine("3 numaralı odada değil");
                        }

                        DateTime today = DateTime.Today;
                        int dailyBonus = 2;
                        if (today != Players[active_Player[connectionID].User_Name].DaylyBonus)
                            dailyBonus = 1;
                        else
                            dailyBonus = 0;

                        int HourlyBonusTime = 3700;
                        if (Players[active_Player[connectionID].User_Name].HourlyBonus == DateTime.MinValue)
                            HourlyBonusTime = 3700;
                        else
                            HourlyBonusTime = (int)DateTime.Now.Subtract(Players[active_Player[connectionID].User_Name].HourlyBonus).TotalSeconds;

                        int a = 0;
                        if (HourlyBonusTime >= 3600)
                            a = 0;
                        else
                            a = (int)(3600 - HourlyBonusTime);

                        DataSender.LEAVE_ROOM(connectionID, dailyBonus, a, 3);
                    }
                    break;
            }

        }

        public static void ChangePlayerPic(int _connectionID, int _newPicIndex)
        {
            active_Player[_connectionID].pictureIndex = _newPicIndex;
            Players[active_Player[_connectionID].User_Name].pictureIndex = _newPicIndex;
        }

        public static void ChangePlayerDisplayName(int _connectionID, string _newDisplayName)
        {
            active_Player[_connectionID].DisplayName = _newDisplayName;
            Players[active_Player[_connectionID].User_Name].DisplayName = _newDisplayName;
        }

        public static void CollectReedem(int _connectionID, string ReedemCode)
        {
            Console.WriteLine(active_Player[_connectionID].DisplayName + " sends code: " + ReedemCode);
            //active_Player[_connectionID]
            //Players[active_Player[_connectionID]
        }

        public static void PlayerCollectedHourlyBonus(int _connectionID)
        {
            Players[active_Player[_connectionID].User_Name].HourlyBonus = DateTime.Now;
            DataSender.CHECK_HOURLY_BONUS(_connectionID, true);
        }

        public static void PlayerCollectedDailyBonus(int _connectionID)
        {
            Players[active_Player[_connectionID].User_Name].DaylyBonus = DateTime.Today;
            
            int x = active_Player[_connectionID].Money;
            x += 500000;
            active_Player[_connectionID].Money = x;
            Players[active_Player[_connectionID].User_Name].Money = x;

            DateTime today = DateTime.Today;
            int dailyBonus = 2;
            if (today != Players[active_Player[_connectionID].User_Name].DaylyBonus)
                dailyBonus = 1;
            else
                dailyBonus = 0;

            DataSender.CHECK_DAYLY_BONUS(_connectionID, dailyBonus);
        }

        public static void PlayerMessaged(int connectionID, byte[] msg)
        {
            int playerRoom = active_Player[connectionID].RoomState;
            int playerRoomID = active_Player[connectionID].roomID;
            int playerTableID = active_Player[connectionID].roomChair;
            switch (playerRoom)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    foreach (var item in RoomManager.TurkishPOKERRooms[playerRoomID].playerIn_Room)
                    {
                        DataSender.PLAYER_MESSAGING(item.Value.ActiveID, playerTableID, msg, playerRoom);
                    }
                    break;
            }
        }

        public static void PlayerPayProduct(int _connectionID, int _product)
        {

            if(_product == 1)
            {
                Players[active_Player[_connectionID].User_Name].DaylyBonus = DateTime.Today;

                int x = active_Player[_connectionID].Money;
                x += 5000;
                active_Player[_connectionID].Money = x;
                Players[active_Player[_connectionID].User_Name].Money = x;
            }
            else if (_product == 2)
            {
                Players[active_Player[_connectionID].User_Name].DaylyBonus = DateTime.Today;

                int x = active_Player[_connectionID].Money;
                x += 10000;
                active_Player[_connectionID].Money = x;
                Players[active_Player[_connectionID].User_Name].Money = x;
            }
            else if (_product == 3)
            {
                Players[active_Player[_connectionID].User_Name].DaylyBonus = DateTime.Today;

                int x = active_Player[_connectionID].Money;
                x += 20000;
                active_Player[_connectionID].Money = x;
                Players[active_Player[_connectionID].User_Name].Money = x;
            }
            else if (_product == 4)
            {
                Players[active_Player[_connectionID].User_Name].DaylyBonus = DateTime.Today;

                int x = active_Player[_connectionID].Money;
                x += 50000;
                active_Player[_connectionID].Money = x;
                Players[active_Player[_connectionID].User_Name].Money = x;
            }
            else if (_product == 5)
            {
                Players[active_Player[_connectionID].User_Name].DaylyBonus = DateTime.Today;

                int x = active_Player[_connectionID].Money;
                x += 100000;
                active_Player[_connectionID].Money = x;
                Players[active_Player[_connectionID].User_Name].Money = x;
            }


            // DataSender.CHECK_DAYLY_BONUS(_connectionID, dailyBonus);
        }
    }

    [System.Serializable]
    public class Players
    {
        public string UserID;
        public int RoomState;
        public string User_Name;
        public string DisplayName;
        public string Password;
        public int Money;
        public string TotalLevel;
        public int ActiveID;
        public int pictureIndex;
        public int roomID;
        public int roomChair;
        public DateTime LastOnline;
        public DateTime DaylyBonus;
        public DateTime HourlyBonus;

        public bool FBUser;
        public string userFBEmail;
        public string userFBID;

        public bool IsInRoom;
        public bool guest;
    }

    public class PlayingPlayer
    {
        public int ActiveID;
        public int Hand;
        public int[] HigherCard = new int[5];
        public int[] playerCard = new int[2];
        public List<int> playerCards = new List<int>();
        public int roomChair;
        public int roomMoney;
        public int winnerCase;
        //public int totalbet;

        public bool isPlaying;
        public bool Rest;
        public bool showCard;
    }

}
