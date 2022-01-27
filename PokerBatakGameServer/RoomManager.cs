using NetworkServer;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Text;
using PokerBatakGameServer.TurkishPokerRoomClass;
using System.Xml.Schema;

namespace PokerBatakGameServer
{
    public static class RoomManager
    {
        public static Dictionary<int, BJTROOM> BJTRooms = new Dictionary<int, BJTROOM>();
        public static Dictionary<int, PokerRoomPro> POKERRooms = new Dictionary<int, PokerRoomPro>();
        public static Dictionary<int, TurkishPokerRoom> TurkishPOKERRooms = new Dictionary<int, TurkishPokerRoom>();


        #region TURKISHPOKERROOM
        public static void AddPlayerTo_TP_Room(int _playerConnection, int _minbet)
        {
            if (TurkishPOKERRooms.Count > 0)
            {
                foreach (var room in TurkishPOKERRooms)
                {
                    if (room.Value.playerIn_Room.Count <= 5 && _minbet == room.Value.roomBet)
                    {
                        room.Value.PlayerJoinedRoom(_playerConnection);
                        Console.WriteLine("ROOM Manager/ADDPlayerTO_TP_ROom");
                        return;
                    }
                }

                CreateNewTPRoom(_playerConnection, _minbet);
            }
            else
            {
                CreateNewTPRoom(_playerConnection, _minbet);
            }
        }
        public static void Add_BOT_TP_Room(int _playerConnection, int roomID)
        {
            TurkishPOKERRooms[roomID].PlayerJoinedRoom(_playerConnection);
        }
        public static void CreateNewTPRoom(int _playerConnection, int _minbet)
        {
            if (_minbet == 0)
                _minbet = 2;

            TurkishPokerRoom newRoom = new TurkishPokerRoom
            {
                roomID = SetTPRoomID(BJTRooms.Count + 1040),
                roomBet = _minbet
            };

            TurkishPOKERRooms.Add(newRoom.roomID, newRoom);
            AddPlayerTo_TP_Room(_playerConnection, _minbet);
        }
        public static int SetTPRoomID(int id)
        {
            foreach (var room in TurkishPOKERRooms)
                if (id == room.Value.roomID)
                {
                    return SetTPRoomID(id + 1);
                }


            return id;
        }
        public static void DisposeBPRoom(int roomID)
        {
            if (TurkishPOKERRooms.ContainsKey(roomID) && TurkishPOKERRooms[roomID].playerIn_Room.Count < 1)
                TurkishPOKERRooms.Remove(roomID);

            Console.WriteLine("total Turkish POKER Rooms room: " + TurkishPOKERRooms.Count);
        }
        #endregion

        #region BJTROOM
        public static void AddPlayerToBJTRoom(int _playerConnection, int _minbet)
        {
            if (BJTRooms.Count > 0)
            {
                foreach (var room in BJTRooms)
                {
                    if (room.Value.RoomPlayers.Count == 1 && _minbet == room.Value.roomBet)
                    {
                        room.Value.EnteredPlayer(_playerConnection);
                        return;
                    }
                }
                foreach (var room in BJTRooms)
                {
                    if (room.Value.RoomPlayers.Count < 2 && _minbet == room.Value.roomBet)
                    {
                        room.Value.EnteredPlayer(_playerConnection);
                        //break;
                        return;
                    }
                }

                CreateNewBJTRoom(_playerConnection, _minbet);
            }
            else
            {
                CreateNewBJTRoom(_playerConnection, _minbet);
            }
        }
        public static void CreateNewBJTRoom(int _playerConnection, int _minbet)
        {
            if (_minbet == 0)
                _minbet = 2;

            BJTROOM newRoom = new BJTROOM
            {
                roomID = SetBJTRoomID(BJTRooms.Count + 1040),
                roomBet = _minbet
            };

            BJTRooms.Add(newRoom.roomID, newRoom);
            AddPlayerToBJTRoom(_playerConnection, _minbet);
        }
        public static int SetBJTRoomID(int id)
        {
            foreach (var room in BJTRooms)
                if (id == room.Value.roomID)
                {
                    return SetBJTRoomID(id + 1);
                }



            return id;
        }
        public static void DisposeBJTRoom(int roomID)
        {
            if (BJTRooms.ContainsKey(roomID) && BJTRooms[roomID].RoomPlayers.Count <= 0)
                BJTRooms.Remove(roomID);
        }
        #endregion

        #region POKERROOM
        public static void AddPlayerToPOKERRoom(int _playerConnection, int _minbet)
        {
            if (POKERRooms.Count > 0)
            {
                foreach (var room in POKERRooms)
                {
                    if (room.Value.playerIn_Room.Count == 1 && _minbet == room.Value.StartingBet)
                    {
                        room.Value.EnteredPlayer(_playerConnection);
                        //break;
                        return;
                    }
                }
                foreach (var room in POKERRooms)
                {
                    if (room.Value.playerIn_Room.Count < 9 && _minbet == room.Value.StartingBet)
                    {
                        room.Value.EnteredPlayer(_playerConnection);
                        //break;
                        return;
                    }
                }

                CreateNewPOKERRoom(_playerConnection, _minbet);
            }
            else
            {
                CreateNewPOKERRoom(_playerConnection, _minbet);
            }
        }
        public static void CreateNewPOKERRoom(int _playerConnection, int _minbet)
        {
            if (_minbet == 0)
                _minbet = 2;

            PokerRoomPro newRoom = new PokerRoomPro
            {
                roomId = SetPOKERRoomID(POKERRooms.Count + 1040),
                StartingBet = _minbet
            };

            POKERRooms.Add(newRoom.roomId, newRoom);
            AddPlayerToPOKERRoom(_playerConnection, _minbet);
        }
        public static int SetPOKERRoomID(int id)
        {
            foreach (var room in POKERRooms)
                if (id == room.Value.roomId)
                {
                    return SetPOKERRoomID(id + 1);
                }


            return id;
        }
        public static void DisposePOKERRoom(int roomID)
        {
            if (POKERRooms.ContainsKey(roomID) && POKERRooms[roomID].playerIn_Room.Count <= 0)
                POKERRooms.Remove(roomID);
        }
        #endregion

    }

    public class BJTROOM
    {
        public int roomID;
        public int roomBet;
        public bool isPlaying = false; 
        public int turnPlayer = 0;
        public int FirstPlayer = 0;
        public Dictionary<int, PlayingPlayer> RoomPlayers = new Dictionary<int, PlayingPlayer>();
        //public List<int> Player1Cards = new List<int>();
        //public List<int> Player2Cards = new List<int>();
        public List<int> Cards = new List<int>();
        public int[] PlayersBet = new int[3];


        public Timer update;
        public int gt = 0;
        public int et = 0;

        void Update(object obj)
        {
            if (isPlaying && RoomPlayers.Count == 2)
            {
                gt++;
                if (gt >= 20 && isPlaying && RoomPlayers.Count == 2)
                {
                    Console.WriteLine("PlayerTurned...");
                    PlayerOK(RoomPlayers[turnPlayer].ActiveID);
                    gt = 0;
                }
            }

            if (!isPlaying && RoomPlayers.Count == 2)
            {
                et++;
                Console.WriteLine("bekle...");

                if (et >= 8)
                {
                    if(!isPlaying && RoomPlayers.Count == 2)
                    {
                        StartGame();
                        et = 0;
                    }
                    else
                    {
                        et = 0;
                    }
                }
            }
        }

        public void EnteredPlayer(int _playerConnection)
        {

            for (int i = 1; i <= 2; i++)
            {
                PlayerManager.active_Player[_playerConnection].roomChair = -1;

                if (!RoomPlayers.ContainsKey(i))
                {
                    PlayingPlayer newplayer = new PlayingPlayer
                    {
                        ActiveID = _playerConnection,
                        roomMoney = 10000,
                        isPlaying = true,
                        roomChair = i,
                        Rest = false,
                    };

                    PlayerManager.active_Player[_playerConnection].roomID = roomID;
                    PlayerManager.active_Player[_playerConnection].roomChair = i;
                    PlayerManager.active_Player[_playerConnection].IsInRoom = true;
                    int x = PlayerManager.active_Player[_playerConnection].Money - newplayer.roomMoney;
                    PlayerManager.active_Player[_playerConnection].Money = x;
                    PlayerManager.active_Player[_playerConnection].RoomState = 2;

                    RoomPlayers.Add(i, newplayer);

                    break;
                }
            }

            if (PlayerManager.active_Player[_playerConnection].roomChair == -1)
                return;

            /////                                     /////
            ///                                         ///
            DataSender.BJT_ROOM_ANSWER(_playerConnection);
            ///                                         ///
            /////                                     /////

            foreach (var player in RoomPlayers)
                if (_playerConnection != player.Value.ActiveID)
                    DataSender.PLAYER_ENTERED_BJT_ROOM(_playerConnection, player.Value.ActiveID);

            foreach (var player in RoomPlayers)
                if (_playerConnection != player.Value.ActiveID)
                    DataSender.PLAYER_ENTERED_BJT_ROOM(player.Value.ActiveID, _playerConnection);


            if (update != null)
                update.Dispose();
            update = new Timer(Update, null, 0, 500);

            Thread.Sleep(500);


            //if (RoomPlayers.Count == 2 && !isPlaying)
            //    StartGame();
            //Console.WriteLine("BJTRoom: " + roomID + " starts game");
            //StartGame();
        }

        public void LeftRoom(int _playerConnection)
        {
            Console.WriteLine("Player Left Room");
            if (isPlaying)
            {
                RoomPlayers[PlayerManager.active_Player[_playerConnection].roomChair].playerCards.Clear();
                EndGame();
            }

            int x = PlayerManager.active_Player[_playerConnection].Money;
            x += RoomPlayers[PlayerManager.active_Player[_playerConnection].roomChair].roomMoney;
            PlayerManager.active_Player[_playerConnection].Money = x;

            RoomPlayers.Remove(PlayerManager.active_Player[_playerConnection].roomChair);

            PlayerManager.active_Player[_playerConnection].roomID = -1;
            PlayerManager.active_Player[_playerConnection].IsInRoom = false;
            PlayerManager.active_Player[_playerConnection].roomChair = -1;
            PlayerManager.active_Player[_playerConnection].RoomState = 0;

            foreach(var item in RoomPlayers)
                DataSender.PLAYER_LEFT_BJT_ROOM(item.Value.ActiveID);

        }

        public void StartGame()
        {
            if (isPlaying || RoomPlayers.Count < 2)
                return;

            Console.WriteLine("game starts With " + RoomPlayers.Count + " player");

            isPlaying = true;
            Cards.Clear();
            RoomPlayers[1].Rest = false;
            RoomPlayers[2].Rest = false;
            et = 0;

            if (FirstPlayer == 0 || FirstPlayer == 2)
                FirstPlayer = 1;
            else if (FirstPlayer == 1)
                FirstPlayer = 2;

            turnPlayer = FirstPlayer;
            

            RoomPlayers[1].playerCards.Clear();
            RoomPlayers[2].playerCards.Clear();


            if (update != null)
                update.Dispose();
            update = new Timer(Update, null, 0, 500);

            Thread.Sleep(500);



            PlayersBet[1] = roomBet;
            PlayersBet[2] = roomBet;

            int x = PlayerManager.active_Player[RoomPlayers[1].ActiveID].Money;
            x -= roomBet;
            PlayerManager.active_Player[RoomPlayers[1].ActiveID].Money = x;
            PlayerManager.Players[PlayerManager.active_Player[RoomPlayers[1].ActiveID].User_Name].Money = x;


            x = PlayerManager.active_Player[RoomPlayers[2].ActiveID].Money;
            x -= roomBet;
            PlayerManager.active_Player[RoomPlayers[2].ActiveID].Money = x;
            PlayerManager.Players[PlayerManager.active_Player[RoomPlayers[2].ActiveID].User_Name].Money = x;



            Cards = ShareCards();

            RoomPlayers[1].playerCards.Add(Cards[0]);
            Cards.Remove(Cards[0]);

            RoomPlayers[1].playerCards.Add(Cards[0]);
            Cards.Remove(Cards[0]);

            RoomPlayers[2].playerCards.Add(Cards[0]);
            Cards.Remove(Cards[0]);

            RoomPlayers[2].playerCards.Add(Cards[0]);
            Cards.Remove(Cards[0]);

            DataSender.START_BJT_GAME(RoomPlayers[1].ActiveID, roomID, RoomPlayers[1].playerCards, RoomPlayers[2].playerCards);
            DataSender.START_BJT_GAME(RoomPlayers[2].ActiveID, roomID, RoomPlayers[2].playerCards, RoomPlayers[1].playerCards);

            TurnPlayer(turnPlayer, false);
        }

        public void TurnPlayer(int _turnPlayer, bool change)
        {
            if (!isPlaying)
                return;
            gt = 0;

            if (change)
                if ((_turnPlayer == 0 || _turnPlayer == 2) && !RoomPlayers[1].Rest)
                    turnPlayer = 1;
                else if (!RoomPlayers[2].Rest)
                    turnPlayer = 2;
                else if (RoomPlayers[2].Rest && RoomPlayers[1].Rest)
                {
                    EndGame();
                    return;
                }


            DataSender.TURN_BJT_PLAYER(turnPlayer, roomID);
        }

        public void EndGame()
        {
            Console.WriteLine("Game Ending");
            isPlaying = false;
            int winner = 0;
            int price = 0;
            
            int x = CheckCards(RoomPlayers[1].playerCards);
            int y = CheckCards(RoomPlayers[2].playerCards);

            


            if (x > 21)
            {
                winner = 2;
                price = PlayersBet[winner] * 2;
                RoomPlayers[winner].roomMoney += price;
                int j = 1;
                if (PlayersBet[2] > PlayersBet[1])
                    j = 2;
                else if(PlayersBet[2] < PlayersBet[1])
                    j = 3;
                DataSender.END_BJT_GAME(winner, price, roomID, j);
                return;
            }
            else if (y > 21)
            {
                winner = 1;
                price = PlayersBet[winner] * 2;
                RoomPlayers[winner].roomMoney += price;
                int j = 1;
                if (PlayersBet[2] > PlayersBet[1])
                    j = 3;
                else if (PlayersBet[2] < PlayersBet[1])
                    j = 2;
                DataSender.END_BJT_GAME(winner, price, roomID, j);
                return;
            }
            else
            {
                if (x > y)
                {
                    winner = 1;
                    price = PlayersBet[winner] * 2;
                    RoomPlayers[winner].roomMoney += price;
                    int j = 1;
                    if (PlayersBet[2] > PlayersBet[1])
                        j = 3;
                    else if (PlayersBet[2] < PlayersBet[1])
                        j = 2;
                    DataSender.END_BJT_GAME(winner, price, roomID, j);
                }
                else if (y > x)
                {
                    winner = 2;
                    price = PlayersBet[winner] * 2;
                    RoomPlayers[winner].roomMoney += price;
                    int j = 1;
                    if (PlayersBet[2] > PlayersBet[1])
                        j = 2;
                    else if (PlayersBet[2] < PlayersBet[1])
                        j = 3;
                    DataSender.END_BJT_GAME(winner, price, roomID, j);
                }
                else
                {
                    if (x == 21 || y == 21)
                    {
                        int p1 = 0;
                        int p2 = 0;

                        if (RoomPlayers[1].playerCards.Count == 2)
                        {
                            if ((RoomPlayers[1].playerCards[0] % 13 == 11 || RoomPlayers[1].playerCards[0] % 13 == 12 || RoomPlayers[1].playerCards[0] % 13 == 0 || RoomPlayers[1].playerCards[0] == 1)
                                && (RoomPlayers[1].playerCards[1] % 13 == 11 || RoomPlayers[1].playerCards[1] % 13 == 12 || RoomPlayers[1].playerCards[1] % 13 == 0 || RoomPlayers[1].playerCards[1] == 1))
                            {
                                p1 = 1;
                            }
                        }
                        if (RoomPlayers[2].playerCards.Count == 2)
                        {
                            if ((RoomPlayers[2].playerCards[0] % 13 == 11 || RoomPlayers[2].playerCards[0] % 13 == 12 || RoomPlayers[2].playerCards[0] % 13 == 0 || RoomPlayers[2].playerCards[0] == 1)
                                && (RoomPlayers[2].playerCards[1] % 13 == 11 || RoomPlayers[2].playerCards[1] % 13 == 12 || RoomPlayers[2].playerCards[1] % 13 == 0 || RoomPlayers[2].playerCards[1] == 1))
                            {
                                p2 = 1;
                            }
                        }
                        if (p1 == 1 && p2 != 1)
                        {
                            winner = 1;
                            price = PlayersBet[winner] * 2;
                            RoomPlayers[winner].roomMoney += price;
                            int j = 1;
                            if (PlayersBet[2] > PlayersBet[1])
                                j = 3;
                            else if (PlayersBet[2] < PlayersBet[1])
                                j = 2;
                            DataSender.END_BJT_GAME(winner, price, roomID, j);
                            return;
                        }
                        else if (p2 == 1 && p1 != 1)
                        {
                            winner = 2;
                            price = PlayersBet[winner] * 2;
                            RoomPlayers[winner].roomMoney += price;
                            int j = 1;
                            if (PlayersBet[2] > PlayersBet[1])
                                j = 2;
                            else if (PlayersBet[2] < PlayersBet[1])
                                j = 3;
                            DataSender.END_BJT_GAME(winner, price, roomID, j);
                            return;
                        }
                    }

                    winner = -1;
                    RoomPlayers[1].roomMoney += PlayersBet[1];
                    RoomPlayers[2].roomMoney += PlayersBet[2];
                    DataSender.END_BJT_GAME(winner, price, roomID, 0);
                    Console.WriteLine("No Winner, winner price: " + price);
                    return;
                }

                Console.WriteLine("Winner is: " + PlayerManager.active_Player[RoomPlayers[winner].ActiveID].User_Name +
                    "   winner price: " + price);
            }
        }

        public void PlayerTakeCard(int connectionID)
        {
            Console.WriteLine("/RoomManager/PlayerTakeCard");
            if(connectionID == RoomPlayers[1].ActiveID)
            {
                RoomPlayers[1].playerCards.Add(Cards[0]);
                Cards.Remove(Cards[0]);
                DataSender.BJ_TOURNAMENT_TAKE_CARD(1, RoomPlayers[1].playerCards[RoomPlayers[1].playerCards.Count - 1], roomID, 0);
                if (CheckCards(RoomPlayers[1].playerCards) > 21)
                {
                    EndGame();
                    return;
                }
            }
            else if(connectionID == RoomPlayers[2].ActiveID)
            {
                RoomPlayers[2].playerCards.Add(Cards[0]);
                Cards.Remove(Cards[0]);
                DataSender.BJ_TOURNAMENT_TAKE_CARD(2, RoomPlayers[2].playerCards[RoomPlayers[2].playerCards.Count - 1], roomID, 0);
                if (CheckCards(RoomPlayers[2].playerCards) > 21)
                {
                    EndGame();
                    return;
                }
            }
            TurnPlayer(turnPlayer, true);
        }

        public void PlayerBetingDouble(int connectionID)
        {
            if (connectionID == RoomPlayers[1].ActiveID)
            {
                RoomPlayers[1].playerCards.Add(Cards[0]);
                Cards.Remove(Cards[0]);
                DataSender.BJ_TOURNAMENT_TAKE_CARD(1, RoomPlayers[1].playerCards[RoomPlayers[1].playerCards.Count - 1], roomID, 2);
                PlayersBet[1] = PlayersBet[1] * 2;
                RoomPlayers[1].roomMoney -= PlayersBet[1] / 2;
                RoomPlayers[1].Rest = true;
                if (CheckCards(RoomPlayers[1].playerCards) > 21)
                {
                    EndGame();
                    return;
                }
            }
            else if (connectionID == RoomPlayers[2].ActiveID)
            {
                RoomPlayers[2].playerCards.Add(Cards[0]);
                Cards.Remove(Cards[0]);
                DataSender.BJ_TOURNAMENT_TAKE_CARD(2, RoomPlayers[2].playerCards[RoomPlayers[2].playerCards.Count - 1], roomID, 2);
                PlayersBet[2] = PlayersBet[2] * 2;
                RoomPlayers[2].roomMoney -= PlayersBet[2] / 2;
                RoomPlayers[2].Rest = true;
                if (CheckCards(RoomPlayers[2].playerCards) > 21)
                {
                    EndGame();
                    return;
                }
            }
            
            TurnPlayer(turnPlayer, true);
        }

        public void PlayerBetingTrible(int connectionID)
        {
            if (connectionID == RoomPlayers[1].ActiveID)
            {
                RoomPlayers[1].playerCards.Add(Cards[0]);
                Cards.Remove(Cards[0]);
                DataSender.BJ_TOURNAMENT_TAKE_CARD(1, RoomPlayers[1].playerCards[RoomPlayers[1].playerCards.Count - 1], roomID, 3);
                PlayersBet[1] = PlayersBet[1] * 3;
                RoomPlayers[1].roomMoney -= (PlayersBet[1] /3)* 2;
                RoomPlayers[1].Rest = true;
                if (CheckCards(RoomPlayers[1].playerCards) > 21)
                {
                    EndGame();
                    return;
                }
            }
            else if (connectionID == RoomPlayers[2].ActiveID)
            {
                RoomPlayers[2].playerCards.Add(Cards[0]);
                Cards.Remove(Cards[0]);
                DataSender.BJ_TOURNAMENT_TAKE_CARD(2, RoomPlayers[2].playerCards[RoomPlayers[2].playerCards.Count - 1], roomID, 3);
                PlayersBet[2] = PlayersBet[2] * 3;
                RoomPlayers[2].roomMoney -= (PlayersBet[2]/3) * 2;
                RoomPlayers[2].Rest = true;
                if (CheckCards(RoomPlayers[2].playerCards) > 21)
                {
                    EndGame();
                    return;
                }
            }

            TurnPlayer(turnPlayer, true);
        }

        public void PlayerOK(int connectionID)
        {
            if (connectionID == RoomPlayers[1].ActiveID)
            {
                RoomPlayers[1].Rest = true;
            }
            else if (connectionID == RoomPlayers[2].ActiveID)
            {
                RoomPlayers[2].Rest = true;
            }

            TurnPlayer(turnPlayer, true);
        }

        public List<int> ShareCards()
        {
            List<int> _Cards = new List<int>();
            Random newRandom = new Random();

            while (_Cards.Count < 52)
            {
                int newcard = newRandom.Next(1, 53);

                if (!_Cards.Contains(newcard))
                    _Cards.Add(newcard);
            }

            return _Cards;
        }

        public int CheckCards(List<int> _playerCards)
        {
            if (_playerCards.Count < 1)
                return 0;

            List<int> cardsValue = new List<int>();
            int[] cardArray = new int[_playerCards.Count];


            foreach (var item in _playerCards)
            {
                int k = item % 13;
                if (k == 11 || k == 12 || k == 0)
                    k = 10;

                cardsValue.Add(k);
                cardArray[cardsValue.Count - 1] = k;
            }


            Array.Sort(cardArray);
            int t = 0;
            for (int i = cardArray.Length - 1; i >= 0; i--)
            {
                if (cardArray[i] == 1)
                {
                    if (t + 11 <= 21)
                    {
                        t += 11;
                    }
                    else
                    {
                        t += cardArray[i];
                    }
                }
                else
                {
                    t += cardArray[i];
                }
            }
            return t;
        }
    }
}
