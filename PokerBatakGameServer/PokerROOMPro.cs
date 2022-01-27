using NetworkServer;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PokerBatakGameServer
{
    public class PokerRoomPro
    {
        #region Variables

        public int roomId;
        public List<int> TableCards = new List<int>();


        public Dictionary<int, PlayingPlayer> playerIn_Room = new Dictionary<int, PlayingPlayer>();
        public Dictionary<int, PlayingPlayer> playerIn_Game = new Dictionary<int, PlayingPlayer>();

        public Dictionary<int, int> playersBet = new Dictionary<int, int>();
        public Dictionary<int, int> playerstotalBet = new Dictionary<int, int>();
        bool[] betControl = new bool[10];



        public bool isPlaying;
        public int firstBeter = 0;
        public int StartingBet;
        public int roundBet;
        public int turnPlayer;
        public int gameRound;
        public int[] beters = new int[3];


        public Timer update;
        public int tg = 0;
        public int te = 0;
        public int winners = 1;

        #endregion

        void Update(object obj)
        {
            if (isPlaying && playerIn_Game.Count >= 2)
            {
                tg++;
                if (tg >= 9 && isPlaying)
                {
                    GetBet(turnPlayer, 0);
                }
            }
            else
            {
                te++;
                if (te > 3 * winners && !isPlaying)
                {
                    te = 0;
                    StartGame();
                }
            }
        }

        public void EnteredPlayer(int _playerConnection)
        {
            PlayerManager.active_Player[_playerConnection].roomID = roomId;

            for (int i = 1; i <= 9; i++)
            {
                PlayerManager.active_Player[_playerConnection].roomChair = -1;

                if (!playerIn_Room.ContainsKey(i))
                {
                    PlayingPlayer newplayer = new PlayingPlayer
                    {
                        ActiveID = _playerConnection,
                        roomMoney = 10000,
                        isPlaying = true,
                        roomChair = i,
                        Rest = false,
                    };

                    PlayerManager.active_Player[_playerConnection].roomID = roomId;
                    PlayerManager.active_Player[_playerConnection].roomChair = i;
                    PlayerManager.active_Player[_playerConnection].IsInRoom = true;
                    int x = PlayerManager.active_Player[_playerConnection].Money - newplayer.roomMoney;
                    PlayerManager.active_Player[_playerConnection].Money = x;
                    PlayerManager.active_Player[_playerConnection].RoomState = 1;


                    playerIn_Room.Add(i, newplayer);

                    break;
                }
            }

            if (PlayerManager.active_Player[_playerConnection].roomChair == -1)
                return;

            /////                                     /////
            ///                                         ///
            DataSender.POKER_ROOM_ANSWER(_playerConnection);
            ///                                         ///
            /////                                     /////


            foreach (var player in playerIn_Room)
                if (_playerConnection != player.Value.ActiveID)
                    DataSender.PLAYER_ENTERED_POKER_ROOM(_playerConnection, player.Value.ActiveID);

            foreach (var player in playerIn_Room)
                if (_playerConnection != player.Value.ActiveID)
                    DataSender.PLAYER_ENTERED_POKER_ROOM(player.Value.ActiveID, _playerConnection);


            if (playerIn_Room.Count == 2 && !isPlaying)
                StartGame();

        }
        
        public void LeftPlayer(int ConnectionID, int serverChair)
        {
            if (playerIn_Game.ContainsKey(serverChair) && playerIn_Room.ContainsKey(serverChair))
            {
                playerIn_Room[serverChair].roomMoney = playerIn_Game[serverChair].roomMoney;

                int x = PlayerManager.active_Player[ConnectionID].Money + playerIn_Room[serverChair].roomMoney;
                PlayerManager.active_Player[ConnectionID].Money = x;

                playerIn_Game.Remove(serverChair);
                playerIn_Room.Remove(serverChair);
                if (playerIn_Game.Count < 2)
                    EndGame();
                else if (turnPlayer == serverChair)
                    TurnPlayer(turnPlayer, roundBet, true);

                Console.WriteLine("player left room");
            }else if (playerIn_Room.ContainsKey(serverChair))
            {
                int x = PlayerManager.active_Player[ConnectionID].Money + playerIn_Room[serverChair].roomMoney;
                PlayerManager.active_Player[ConnectionID].Money = x;

                playerIn_Room.Remove(serverChair);

                Console.WriteLine("player left room");
            }

            foreach (var item in playerIn_Room)
                DataSender.PLAYER_LEFT_POKER_ROOM(item.Value.ActiveID, serverChair);

            if (playerIn_Game.Count <= 0)
                RoomManager.DisposePOKERRoom(roomId);

            PlayerManager.active_Player[ConnectionID].roomID = 0;
            PlayerManager.active_Player[ConnectionID].IsInRoom = false;
            PlayerManager.active_Player[ConnectionID].RoomState = 0;
        }

        public void StartGame()
        {
            if (isPlaying || playerIn_Room.Count < 2)
                return;

            gameRound = 0;
            roundBet = StartingBet;
            isPlaying = true;

            playerIn_Game.Clear();
            TableCards.Clear();
            playersBet.Clear();
            playerstotalBet.Clear();

            for (int i = 1; i <= 9; i++)
                betControl[i] = false;

            if (update != null)
                update.Dispose();
            update = new Timer(Update, null, 0, 500);

            Thread.Sleep(1500);

            List<int> Cards = ShareCards();
            if (playerIn_Game.Count < 2)
            {
                return;
            }

            beters = SetfirstBeter();
            turnPlayer = beters[2];
            
            foreach (var player in playerIn_Room)
            {
                DataSender.START_GAME(player.Value.ActiveID, beters[0], beters[1], beters[2], roundBet,
                    playerIn_Game[player.Key].playerCard[0],
                    playerIn_Game[player.Key].playerCard[1], playerIn_Game);
            }

            #region FIRST BETERS
            int refreshMoney = 0;
            if (playerIn_Game[beters[0]].roomMoney <= roundBet / 2)
            {
                playersBet.Add(beters[0], playerIn_Game[beters[0]].roomMoney);
                refreshMoney = 0;
                playerIn_Game[beters[0]].roomMoney = refreshMoney;
                DataSender.REFRESH_MONEY(refreshMoney, roomId, beters[0], playerIn_Game[beters[0]].roomMoney);
                playerIn_Game[beters[0]].Rest = true;
            }
            else
            {
                playersBet.Add(beters[0], roundBet / 2);
                refreshMoney = playerIn_Game[beters[0]].roomMoney - (roundBet / 2);
                playerIn_Game[beters[0]].roomMoney = refreshMoney;
                DataSender.REFRESH_MONEY(refreshMoney, roomId, beters[0], (roundBet / 2));
            }

            if (playerIn_Game[beters[1]].roomMoney <= roundBet)
            {
                playersBet.Add(beters[1], playerIn_Game[beters[1]].roomMoney);
                refreshMoney = 0;
                playerIn_Game[beters[1]].roomMoney = refreshMoney;
                DataSender.REFRESH_MONEY(refreshMoney, roomId, beters[1], playerIn_Game[beters[1]].roomMoney);
                playerIn_Game[beters[1]].Rest = true;
            }
            else
            {
                playersBet.Add(beters[1], roundBet);
                refreshMoney = playerIn_Game[beters[1]].roomMoney - roundBet;
                playerIn_Game[beters[1]].roomMoney = refreshMoney;
                DataSender.REFRESH_MONEY(refreshMoney, roomId, beters[1], roundBet);
            }

            TurnPlayer(turnPlayer, roundBet, false);
            #endregion


        }

        public void TurnPlayer(int x, int _minbet, bool change)
        {
            if (!isPlaying)
                return;

            tg = 0;
            roundBet = _minbet;

            if (change)
                x++;

            if (x > 9)
            {
                x = 1;
            }

            if (playerIn_Game.ContainsKey(x) && !playerIn_Game[x].Rest)
            {
                turnPlayer = x;
            }
            else
            {
                TurnPlayer(x, _minbet, true);
                return;
            }

            int maxBet = playerIn_Game[turnPlayer].roomMoney;
            foreach(var item in playerIn_Game)
                if (item.Value.roomMoney < maxBet)
                    maxBet = item.Value.roomMoney;

           
            if(playerIn_Game[turnPlayer].roomMoney >= roundBet)
                DataSender.TURN_PLAYER(turnPlayer, roomId, playerIn_Game[turnPlayer].roomMoney, roundBet, maxBet);
            else
                DataSender.TURN_PLAYER(turnPlayer, roomId, playerIn_Game[turnPlayer].roomMoney, playerIn_Game[turnPlayer].roomMoney, maxBet);

        }

        public List<int> ShareCards()
        {
            Random newRandom = new Random();
            List<int> cards = new List<int>();

            while (cards.Count < 52)
            {
                int newcard = newRandom.Next(1, 53);

                if (!cards.Contains(newcard))
                    cards.Add(newcard);
            }

            int x = 0;
            foreach (var item in playerIn_Room)
            {
                if (playerIn_Room[item.Key].roomMoney > 0)
                {
                    item.Value.playerCard[0] = cards[x];
                    x++;
                    item.Value.playerCard[1] = cards[x];
                    x++;
                    // Console.WriteLine("player aded game");
                    playerIn_Game.Add(item.Key, item.Value);
                }
            }
            foreach (var item in playerIn_Game)
                item.Value.Rest = false;

            TableCards.Add(cards[x]);
            x++;
            TableCards.Add(cards[x]);
            x++;
            TableCards.Add(cards[x]);
            x++;
            TableCards.Add(cards[x]);
            x++;
            TableCards.Add(cards[x]);

            return cards;
        }

        public int[] SetfirstBeter()
        {
            firstBeter++;
            if (firstBeter > 9)
            {
                firstBeter = 1;
            }

            if (!playerIn_Game.ContainsKey(firstBeter) || playerIn_Game[firstBeter].Rest)
                return SetfirstBeter();

            int y = SetsecondBeter(firstBeter);
            int z = SetthirdBeter(y);

            int[] c = { firstBeter, y, z };

            return c;
        }

        public int SetsecondBeter(int x)
        {
            x++;
            if (x > 9)
                x = 1;

            if (!playerIn_Game.ContainsKey(x) || playerIn_Game[x].Rest)
                return SetsecondBeter(x);
            else
                return x;
        }

        public int SetthirdBeter(int x)
        {
            x++;
            if (x > 9)
                x = 1;

            if (!playerIn_Game.ContainsKey(x) || playerIn_Game[x].Rest)
                return SetthirdBeter(x);
            else
                return x;
        }

        public void GetBet(int _playerChair, int Bet)
        {
            if (!isPlaying)
                return;

         
            int playerConnection = playerIn_Game[_playerChair].ActiveID;
            if (playersBet.ContainsKey(_playerChair))
            {
                if (Bet >= (playerIn_Game[_playerChair].roomMoney + playersBet[_playerChair]))
                {
                    Bet = playerIn_Game[_playerChair].roomMoney;
                    playerIn_Game[_playerChair].Rest = true;
                }
            }
            else
            {
                if (Bet >= (playerIn_Game[_playerChair].roomMoney))
                {
                    Bet = playerIn_Game[_playerChair].roomMoney;
                    playerIn_Game[_playerChair].Rest = true;
                }
            }

            if (Bet < roundBet && !playerIn_Game[_playerChair].Rest)
                Bet = -1;
            if (Bet > roundBet)
                roundBet = Bet;
            

            int refreshMoney;
            if (Bet == -1)
            {
                Console.WriteLine("player Left tour");
                //if (!betControl[_playerChair] && _playerChair == beters[0])
                //    playersBet.Add(beters[0], roundBet / 2);
                //if (!playersBet.ContainsKey(_playerChair) && _playerChair == beters[1])
                //    playersBet.Add(beters[0], roundBet);

                //playerIn_Game[_playerChair].roomMoney -= playersBet[_playerChair];
                if(playerIn_Room.ContainsKey(_playerChair) && playerIn_Game.ContainsKey(_playerChair))
                {
                    playerIn_Room[_playerChair].roomMoney = playerIn_Game[_playerChair].roomMoney;
                    playerIn_Game.Remove(_playerChair);
                }
            }
            else
            {
                if (playersBet.ContainsKey(_playerChair))
                {
                    refreshMoney = (playerIn_Game[_playerChair].roomMoney - Bet) + playersBet[_playerChair];

                    if (refreshMoney <= 0)
                    {
                        playerIn_Game[_playerChair].Rest = true;
                        refreshMoney = 0;
                    }
                        

                    playerIn_Game[_playerChair].roomMoney = refreshMoney;
                    
                    playersBet.Remove(_playerChair);
                    playersBet.Add(_playerChair, Bet);
                    betControl[_playerChair] = true;


                }
                else
                {
                    refreshMoney = playerIn_Game[_playerChair].roomMoney - Bet;
                    if (refreshMoney < 0)
                        refreshMoney = 0;

                    playerIn_Game[_playerChair].roomMoney = refreshMoney;

                    playersBet.Add(_playerChair, Bet);
                    betControl[_playerChair] = true;
                }
            }


            DataSender.REFRESH_MONEY(playerIn_Room[_playerChair].roomMoney, roomId, _playerChair, Bet);

            if (playerIn_Game.Count < 2)
            {
                EndGame();
                return;
            }


            if (CheckRound())
            {
                SetRoundBets();

                int restPlayer = 0;
                foreach (var item in playerIn_Game)
                    if (!item.Value.Rest)
                        restPlayer++;

                if (restPlayer < 2 || playerIn_Game.Count < 2)
                {
                    EndGame();
                    return;
                }

                SetGameRound();
            }
            else
            {
                TurnPlayer(turnPlayer, roundBet, true);
            }
        }

        public void SetRoundBets()
        {
            foreach (var item in playersBet)
            {
                if (playerstotalBet.ContainsKey(item.Key))
                {
                    int x = playerstotalBet[item.Key] + item.Value;
                    playerstotalBet.Remove(item.Key);
                    playerstotalBet.Add(item.Key, x);
                }
                else
                    playerstotalBet.Add(item.Key, item.Value);
            }
            playersBet.Clear();
        }
        
        public int TotalBetsInTable()
        {
            int total = 0;
            foreach (var item in playerstotalBet)
                total += item.Value;

            return total;
        }

        public void SetGameRound()
        {
            playersBet.Clear();

            switch (gameRound)
            {
                case 0:
                    foreach (var item in playerIn_Room)
                        DataSender.SEND_FIRST_THREE_CARDS(item.Value.ActiveID, TableCards, TotalBetsInTable());
                    roundBet = 0;
                    for (int i = 1; i <= 9; i++)
                        betControl[i] = false;
                    break;
                case 1:
                    foreach (var item in playerIn_Room)
                        DataSender.SEND_FOURTH_CARD(item.Value.ActiveID, TableCards, TotalBetsInTable());
                    roundBet = 0;
                    for (int i = 1; i <= 9; i++)
                        betControl[i] = false;
                    break;
                case 2:
                    foreach (var item in playerIn_Room)
                        DataSender.SEND_FIFTH_CARD(item.Value.ActiveID, TableCards, TotalBetsInTable());
                    roundBet = 0;
                    for (int i = 1; i <= 9; i++)
                        betControl[i] = false;
                    break;
                case 3:
                    EndGame();
                    return;
            }
            gameRound++;
            turnPlayer = beters[0];
            TurnPlayer(turnPlayer, roundBet, false);
        }

        public bool CheckRound()
        {
            foreach (var item in playerIn_Game)
            {
                if(!item.Value.Rest)
                    if (!playersBet.ContainsKey(item.Key)
                        || playersBet[item.Key] != roundBet
                        || !betControl[item.Key])
                        return false;
            }

            return true;
        }

        public void EndGame()
        {
            isPlaying = false;
            tg = 0;
            SetRoundBets();
            CheckWinner();
        }

        public void CheckWinner()
        {
            int total = 0;

            foreach (var item in playerstotalBet)
                total += item.Value;

            if (playerIn_Game.Count < 2)
            {
                foreach (var item in playerIn_Game)
                {
                    int refreshMoney = playerIn_Game[item.Key].roomMoney + total;
                    playerIn_Game[item.Key].roomMoney = refreshMoney;
                    playerIn_Room[item.Key].roomMoney = playerIn_Game[item.Key].roomMoney;

                    foreach (var item1 in playerIn_Room)
                        DataSender.END_GAME(item1.Value.ActiveID, item.Key, total, total);
                }

                
                return;
            }

            Dictionary<int, PlayingPlayer> playerInCheck = new Dictionary<int, PlayingPlayer>();
            int[] cardsValues = new int[7];

            for(int i = 1; i <= 9; i++)
            {
                if (playerIn_Game.ContainsKey(i))
                {
                    cardsValues[0] = playerIn_Game[i].playerCard[0];
                    cardsValues[1] = playerIn_Game[i].playerCard[1];
                    cardsValues[2] = TableCards[0];
                    cardsValues[3] = TableCards[1];
                    cardsValues[4] = TableCards[2];
                    cardsValues[5] = TableCards[3];
                    cardsValues[6] = TableCards[4];

                    playerInCheck.Add(i, CheckHand.CheckCards(cardsValues, playerIn_Game[i].ActiveID, i));
                }
            }

            Dictionary<int, int> playerPrize = CheckHand.seperateMoney(playerstotalBet, playerInCheck);

            foreach(var item in playerPrize)
            {
                int refreshMoney = playerIn_Game[item.Key].roomMoney + item.Value;
                playerIn_Game[item.Key].roomMoney = refreshMoney;


                foreach (var item1 in playerIn_Room)
                    DataSender.END_GAME(item1.Value.ActiveID, item.Key, item.Value, total);
            }

            foreach (var item in playerIn_Game)
                playerIn_Room[item.Key].roomMoney = item.Value.roomMoney;
        }
    }
}
