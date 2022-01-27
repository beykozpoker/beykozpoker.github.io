using NetworkServer;
using PokerBatakGameServer.Bots;
using PokerBatakGameServer.SaveLoad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

namespace PokerBatakGameServer.TurkishPokerRoomClass
{
    public class TurkishPokerRoom
    {
        
        #region Variables
        
        public Dictionary<int, PlayingPlayer> playerIn_Room = new Dictionary<int, PlayingPlayer>();
        public Dictionary<int, PlayingPlayer> playerIn_Game = new Dictionary<int, PlayingPlayer>();

        public Dictionary<int, int> playersRoundBet = new Dictionary<int, int>();
        public Dictionary<int, int> playersTotalBet = new Dictionary<int, int>();
        bool[] betControl = new bool[10];


        public List<int> TableCards = new List<int>();


        public int roomID;
        public int roomBet;
        public int roundBet;
        public int maxRoundBet;


        public List<int> Cards = new List<int>();
        public bool isPlaying;
        public int firstBeter = 0;
        public int turnPlayer;
        public int gameRound;
        public int[] beters = new int[3];


        public Timer update;
        public int gameTime = 0;
        public int endTime = 0;
        public int botTime = 0;
        public int DisposeBot = 0;
        int totalWinner = 1;


        public string newBotUserID;
        public string newBotUserName;
        public int newBotActiveID;
        public bool botActive;


        public int gamecounter = 0;
        #endregion

        void Update(object obj)
        {
            if (isPlaying && playerIn_Room.Count >= 2)
            {
                gameTime++;
                if (gameTime >= 10 && isPlaying)
                {
                    gameTime = 0;
                    if (gameRound == 0 || gameRound == 2)
                    {
                        ReceiveCardAnswers(playerIn_Game[turnPlayer].ActiveID, true);
                    }
                    else
                    {
                        if (roundBet > 0)
                            ReceiveBets(playerIn_Game[turnPlayer].ActiveID, -2);
                        else
                            ReceiveBets(playerIn_Game[turnPlayer].ActiveID, 0);
                    }
                }
            }
            else
            {
                gameTime = 0;
            }



            if (!isPlaying && playerIn_Room.Count >= 2)
            {
                endTime++;

                if (endTime > 2 + (2 * totalWinner))
                {
                    if (!isPlaying && playerIn_Room.Count >= 2)
                    {
                        StartGame();
                        endTime = 0;
                    }
                }
            }
            else
            {
                totalWinner = 1;
                endTime = 0;
            }


            if (!isPlaying && playerIn_Room.Count == 1 && !botActive)
            {
                botTime++;

                if (botTime > 4)
                {
                    botTime = 0;
                    CreateBotPlayer();
                }
            }
            if (!isPlaying && playerIn_Room.Count == 1 && botActive)
            {
                DisposeBot++;
                if (DisposeBot == 2)
                {
                    DisposeBot = 0;
                    PlayerLeftRoom(newBotActiveID);
                }
            }
        }

        public void CreateBotPlayer()
        {
            BotPlayerManager.CreateBotPlayer("mustafa", "123123123", roomID, roomBet);
        }

        public void SetBotINFO(int _activeID, string _UserID, string _newBotUserName)
        {
            botActive = true;
            newBotActiveID = _activeID;
            newBotUserID = _UserID;
            newBotUserName = _newBotUserName;
        }

        public void PlayerJoinedRoom(int _playerConnection)
        {
            PlayerManager.active_Player[_playerConnection].roomChair = -1;

            for (int i = 1; i <= 5; i++)
            {
                if (!playerIn_Room.ContainsKey(i))
                {
                    PlayingPlayer newplayer = new PlayingPlayer
                    {
                        ActiveID = _playerConnection,
                        roomMoney = PlayerManager.active_Player[_playerConnection].Money,
                        isPlaying = true,
                        roomChair = i,
                        Rest = false,
                    };

                    PlayerManager.active_Player[_playerConnection].roomID = roomID;
                    PlayerManager.active_Player[_playerConnection].roomChair = i;
                    PlayerManager.active_Player[_playerConnection].IsInRoom = true;
                    int x = PlayerManager.active_Player[_playerConnection].Money - newplayer.roomMoney;
                    PlayerManager.active_Player[_playerConnection].Money = x;
                    PlayerManager.active_Player[_playerConnection].RoomState = 3;

                    playerIn_Room.Add(i, newplayer);

                    break;
                }
            }

            if (PlayerManager.active_Player[_playerConnection].roomChair == -1)
                return;

            /////                                     /////
            ///                                         ///
            DataSender.TP_ROOM_ANSWER(_playerConnection);
            ///                                         ///
            /////                                     /////

            foreach (var player in playerIn_Room)
                if (_playerConnection != player.Value.ActiveID)
                    DataSender.PLAYER_ENTERED_TP_ROOM(_playerConnection, player.Value.ActiveID);

            foreach (var player in playerIn_Room)
                if (_playerConnection != player.Value.ActiveID)
                    DataSender.PLAYER_ENTERED_TP_ROOM(player.Value.ActiveID, _playerConnection);


            if (update != null)
                update.Dispose();
            update = new Timer(Update, null, 0, 1000);

        }

        public void PlayerLeftRoom(int _playerConnection)
        {
            int player = PlayerManager.active_Player[_playerConnection].roomChair;


            if (playerIn_Game.ContainsKey(player))
            {
                playerIn_Room[player].roomMoney = playerIn_Game[player].roomMoney;

                playerIn_Game.Remove(player);
            }


            int x = PlayerManager.active_Player[_playerConnection].Money
                + playerIn_Room[player].roomMoney;
            PlayerManager.active_Player[_playerConnection].Money = x;


            playerIn_Room.Remove(player);


            DataSender.PLAYER_LEFT_TP_ROOM(player, roomID);


            PlayerManager.active_Player[_playerConnection].RoomState = 0000;
            PlayerManager.active_Player[_playerConnection].roomID = 0000;
            PlayerManager.active_Player[_playerConnection].roomChair = -1;
            PlayerManager.active_Player[_playerConnection].IsInRoom = false;

            if(_playerConnection < 0)
            {
                PlayerManager.active_Player.Remove(_playerConnection);
                BotPlayerManager.Bots.Remove(newBotActiveID);


                botActive = false;
            }




            if (turnPlayer == player)
            {
                if (playerIn_Game.Count < 2)
                {
                    EndGame();
                }
                SetTurnBetPlayer(player, roundBet, true);
            }
            else
            {
                if (playerIn_Game.Count < 2)
                {
                    EndGame();
                }
            }



            if (playerIn_Room.Count == 0 )
            {
                RoomManager.DisposeBPRoom(roomID);
            }
        }

        public void StartGame()
        {
            gamecounter++;
            playerIn_Game.Clear();
            TableCards.Clear();
            playersTotalBet.Clear();
            playersRoundBet.Clear();
            Cards.Clear();

            roundBet = roomBet;
            gameRound = 0;
            forcegameend = false;


            if (botActive)
                BotPlayerManager.StartGame(newBotActiveID, playerIn_Room[PlayerManager.active_Player[newBotActiveID].roomChair].roomMoney);

            if (isPlaying || playerIn_Room.Count < 2)
                return;
            else
                isPlaying = true;



            foreach (var item in playerIn_Room)
            {
                if(item.Value.roomMoney > 0)
                {
                    playerIn_Game.Add(item.Key, item.Value);
                    playerIn_Game[PlayerManager.active_Player[item.Value.ActiveID].roomChair].playerCards.Clear();
                    playerIn_Game[item.Key].Rest = false;
                    playersRoundBet.Add(item.Key, 0);
                    betControl[item.Key] = false;
                }
            }

            if(playerIn_Game.Count < 2)
            {
                isPlaying = false;
                return;
            }


            Cards = LoadCards();
            beters = SetfirstBeter();
            turnPlayer = beters[2];

            if(playerIn_Game[beters[0]].roomMoney <= roomBet / 2)
            {
                playersRoundBet.Remove(beters[0]);
                playersRoundBet.Add(beters[0], playerIn_Game[beters[0]].roomMoney);
                playerIn_Game[beters[0]].Rest = true;
                playerIn_Game[beters[0]].roomMoney = 0;
            }
            else
            {
                playersRoundBet.Remove(beters[0]);
                playersRoundBet.Add(beters[0], roomBet / 2);
                playerIn_Game[beters[0]].roomMoney -= roomBet / 2;
            }


            if (playerIn_Game[beters[1]].roomMoney <= roomBet)
            {
                playersRoundBet.Remove(beters[1]);
                playersRoundBet.Add(beters[1], playerIn_Game[beters[1]].roomMoney);
                playerIn_Game[beters[1]].Rest = true;
                playerIn_Game[beters[1]].roomMoney = 0;
            }
            else
            {
                playersRoundBet.Remove(beters[1]);
                playersRoundBet.Add(beters[1], roomBet);
                playerIn_Game[beters[1]].roomMoney -= roomBet;
            }


            DataSender.START_BPGAME(roomID, beters[0], beters[1],
                playerIn_Game[beters[0]].roomMoney, playerIn_Game[beters[1]].roomMoney);

            //if (playerIn_Room.Count < 5)
            //{
            //    CreateBotPlayer();
            //}
            AskCardToPlayer(turnPlayer, false);
        }

        public void EndGame()
        {
            isPlaying = false;
            endTime = 0;

            foreach (var item in playersRoundBet)
            {
                if (playersTotalBet.ContainsKey(item.Key))
                {
                    int z = playersTotalBet[item.Key] + item.Value;
                    playersTotalBet.Remove(item.Key);
                    playersTotalBet.Add(item.Key, z);
                }
                else
                {
                    playersTotalBet.Add(item.Key, item.Value);
                }
            }



            foreach (var item in playerIn_Room)
                DataSender.TPROOM_END_GAME(item.Value.ActiveID, roomID);


            CheckWinner();
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

        public List<int> LoadCards()
        {
            Random newRandom = new Random();
            List<int> cards = new List<int>();

            while (cards.Count < 52)
            {
                int newcard = newRandom.Next(1, 53);

                if (!cards.Contains(newcard))
                    cards.Add(newcard);
            }

            return cards;
        }

        public bool CheckALLPlayersHasCard()
        {

            if (gameRound == 0)
            {
                foreach (var item in playerIn_Game)
                {
                    if (item.Value.playerCards.Count != 2)
                    {
                        return false;
                    }
                }
            }
            else if(gameRound == 2)
            {
                foreach (var item in playerIn_Game)
                {
                    if (item.Value.playerCards.Count != 3)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public int ChekALLPlayersHasBet()
        {
            int playingPlayer = 0;
            int lastPlayer = -1;
            foreach (var item in playerIn_Game)
            {
                if (!item.Value.Rest)
                {
                    playingPlayer++;
                    lastPlayer = item.Key;
                }
            }



            ///////////////////
            ///

            

            foreach (var item in playerIn_Game)
            {
                if (!item.Value.Rest)
                {
                    if (playersRoundBet[item.Key] != roundBet || !betControl[item.Key])
                    {
                        return 3;
                    }
                }
            }


            if ((playingPlayer < 2))
            {
                PayBackBET();

                return 2;
            }


            ////////////
            ///



            return 1;
        }

        public void PayBackBET()
        {
            int maxBetPlayer = -1;
            int maxBetPrice = 0;
            int maxBetPrice2 = 0;
            bool check = false;
            int payback = 0;

            foreach (var bet in playersRoundBet)
            {
                if (bet.Value > maxBetPrice)
                {
                    maxBetPrice = bet.Value;
                    maxBetPlayer = bet.Key;

                    maxBetPrice2 = maxBetPrice;
                    check = false;
                }
                else if(bet.Value < maxBetPrice)
                {
                    if (bet.Value > maxBetPrice2)
                    {
                        maxBetPrice2 = bet.Value;
                    }
                }
                else
                {
                    check = true;
                }
            }

            if (!check)
            {
                payback = maxBetPrice - maxBetPrice2;
                playersRoundBet.Remove(maxBetPlayer);
                playersRoundBet.Add(maxBetPlayer, maxBetPrice2);
                playerIn_Game[maxBetPlayer].roomMoney += payback;
                DataSender.PAYBACK(maxBetPlayer, payback, playerIn_Game[maxBetPlayer].roomMoney, roomID);
            }
        }

        public void SetTurnBetPlayer(int player, int _roundBet, bool change)
        {
            if (!isPlaying)
                return;

            gameTime = 0;

            if (change)
            {
                player++;

                if (player > 5)
                {
                    player = 1;
                }
            }

            roundBet = _roundBet;
            turnPlayer = player;
            if (playerIn_Game.ContainsKey(player) && !playerIn_Game[player].Rest)
            {
                int maxbet = 0;
                foreach(var item in playerIn_Game)
                {
                    if (item.Value.roomMoney + playersRoundBet[item.Key] > maxbet && item.Key != turnPlayer)
                    {
                        maxbet = item.Value.roomMoney + playersRoundBet[item.Key];
                    }

                    if (maxbet >= playerIn_Game[player].roomMoney + playersRoundBet[player])
                    {
                        maxbet = playerIn_Game[player].roomMoney + playersRoundBet[player];
                        break;
                    }
                }

                maxRoundBet = maxbet;

                if (roundBet >= playerIn_Game[player].roomMoney)
                    DataSender.TURN_BET_PLAYER(turnPlayer, roomID, -1, maxbet);
                else
                    DataSender.TURN_BET_PLAYER(turnPlayer, roomID, roundBet, maxbet);

            }
            else
            {
                SetTurnBetPlayer(player, roundBet, true);
                return;
            }

        }

        public void AskCardToPlayer(int player, bool changePlayer)
        {
            if (!isPlaying)
                return;

            gameTime = 0;

            if (changePlayer)
            {
                player++;

                if (player > 5)
                {
                    player = 1;
                }
            }


            if (playerIn_Game.ContainsKey(player))
            {
                turnPlayer = player;
                DataSender.ASK_CARD_TO_PLAYER(player, roomID, Cards[0], gameRound);
            }
            else
            {
                AskCardToPlayer(player, true);
                return;
            }
        }

        public void ReceiveCardAnswers(int connectionID, bool answer)
        {
            gameTime = 0;
            int player = -1;
            if (PlayerManager.active_Player.ContainsKey(connectionID))
                player = PlayerManager.active_Player[connectionID].roomChair;
            else
                return;


            if (answer)
            {
                playerIn_Game[PlayerManager.active_Player[connectionID].roomChair].playerCards.Add(Cards[0]);
                DataSender.PLAYER_ACCEPT_CARD(player, Cards[0], 1, roomID);

                Cards.Remove(Cards[0]);
            }
            else if (!answer)
            {
                Cards.Remove(Cards[0]);
                playerIn_Game[PlayerManager.active_Player[connectionID].roomChair].playerCards.Add(Cards[0]);
                DataSender.PLAYER_ACCEPT_CARD(player, Cards[0], 2, roomID);
                
                Cards.Remove(Cards[0]);

                Thread.Sleep(800);
            }

            Thread.Sleep(1000);

            if (CheckALLPlayersHasCard())
            {
                turnPlayer = beters[2];
                if (gameRound == 0)
                    roundBet = roomBet;
                else
                    roundBet = 0;

                gameRound++;
                gameTime = 0;



                if (forcegameend)
                    ForceGameToEnd(gameRound);
                else
                    SetTurnBetPlayer(turnPlayer, roundBet, false);


            }
            else
            {
                gameTime = 0;
                AskCardToPlayer(turnPlayer, true);
            }
        }

        public void ReceiveBets(int connectionID, int Bet)
        {
            //bet = -2 player left game
            //bet = -1 player rest
            //bet = 0 player see round bet
            
            
            gameTime = 0;

            int player = PlayerManager.active_Player[connectionID].roomChair;
            int playerMoney = playerIn_Game[player].roomMoney;

            if (player != turnPlayer)
            {
                return;
            }


            if (Bet >= playerIn_Game[player].roomMoney || Bet >= maxRoundBet)
            {
                Bet = -1;
            }

            int betSound = -1;




            if (Bet == -2)
            {
                PlayerLeftGame(connectionID, player);

                Bet = 0;
                betSound = 3;
                //Player Left Game
            }
            else if (Bet == -1)
            {
                //Player Rest
                Bet = maxRoundBet;
                playerIn_Game[player].Rest = true;
                betSound = 2;
            }
            else if (Bet == 0)
            {
                Bet = roundBet;
            }

            

            if (Bet > roundBet)
            {
                roundBet = Bet;
            }



            if (playerIn_Game.ContainsKey(player))
            {
                if (playerIn_Game[player].Rest)
                {
                    betSound = 2;
                }
                else if (Bet > playersRoundBet[player])
                {
                    betSound = 1;
                }
                else if (betSound != 3)
                {
                    betSound = 0;
                }


                if (Bet >= playerIn_Game[player].roomMoney + playersRoundBet[player])
                {
                    Bet = playerIn_Game[player].roomMoney + playersRoundBet[player];
                    playerIn_Game[player].Rest = true;
                    betSound = 2;
                }

                if (betControl[player])
                {
                    if (playerIn_Game[player].Rest)
                    {
                        playerIn_Game[player].roomMoney += playersRoundBet[player];

                        playersRoundBet.Remove(player);
                        playersRoundBet.Add(player, Bet);
                    }
                    else
                    {
                        playerIn_Game[player].roomMoney += playersRoundBet[player];
                        //int x = playersRoundBet[player] + Bet;
                        playersRoundBet.Remove(player);
                        playersRoundBet.Add(player, Bet);
                    }
                }
                else
                {
                    playerIn_Game[player].roomMoney += playersRoundBet[player];
                    playersRoundBet.Remove(player);
                    playersRoundBet.Add(player, Bet);
                }

                playerIn_Game[player].roomMoney -= Bet;
                playerMoney = playerIn_Game[player].roomMoney;
            }
            else
            {
                if (playerIn_Room.ContainsKey(player))
                    playerMoney = playerIn_Room[player].roomMoney;
                else
                    playerMoney = 0;
            }

            
            betControl[player] = true;





            DataSender.PLAYER_BETED_BPR(player, playersRoundBet[player], playerMoney, roomID, betSound);




            if (playerIn_Game.Count < 2)
            {
                EndGame();
                return;
            }

            Thread.Sleep(500);

            int CPB;
            if (!forcegameend)
                CPB = ChekALLPlayersHasBet();
            else
                CPB = 2;

            if (CPB == 1)
            {
                gameTime = 0;

                foreach (var item in playersRoundBet)
                {
                    if (playersTotalBet.ContainsKey(item.Key))
                    {
                        int z = playersTotalBet[item.Key] + item.Value;
                        playersTotalBet.Remove(item.Key);
                        playersTotalBet.Add(item.Key, z);
                    }
                    else
                    {
                        playersTotalBet.Add(item.Key, item.Value);
                    }

                    
                    betControl[item.Key] = false;
                }

                foreach(var item in playerIn_Game)
                {
                    playersRoundBet.Remove(item.Key);
                    playersRoundBet.Add(item.Key, 0);
                }

                int totalBet = 0;
                foreach (var item in playersTotalBet)
                {
                    totalBet += item.Value;
                }

                DataSender.SHOW_TOTAL_BET(roomID, totalBet);


                roundBet = 0;

                switch (gameRound)
                {
                    case 1:
                        //Show 3 card
                        ShowFlopCards(false);
                        break;
                    case 2:
                        //ask second time question
                        turnPlayer = beters[2];
                        AskCardToPlayer(beters[2], false);
                        break;
                    case 3:
                        //Show 4th card
                        ShowTurnCards(false);
                        break;
                    case 4:
                        //Show 5th card
                        ShowRiverCards(false);
                        break;
                    case 5:
                        //end game
                        EndGame();
                        break;
                }
            }
            else if (CPB == 2)
            {

                forcegameend = true;
                ForceGameToEnd(gameRound);
            }
            else if (CPB == 3)
            {
                gameTime = 0;
                SetTurnBetPlayer(turnPlayer, roundBet, true);
            }
        }
        
        bool forcegameend = false;

        public void ForceGameToEnd(int _gameRound)
        {
            gameTime = 0;

            foreach (var item in playersRoundBet)
            {
                if (playersTotalBet.ContainsKey(item.Key))
                {
                    int z = playersTotalBet[item.Key] + item.Value;
                    playersTotalBet.Remove(item.Key);
                    playersTotalBet.Add(item.Key, z);
                }
                else
                {
                    playersTotalBet.Add(item.Key, item.Value);
                }


                betControl[item.Key] = false;
            }

            foreach (var item in playerIn_Game)
            {
                playersRoundBet.Remove(item.Key);
                playersRoundBet.Add(item.Key, 0);
            }

            int totalBet = 0;
            foreach (var item in playersTotalBet)
            {
                totalBet += item.Value;
            }

            Console.WriteLine("totalbet: " + totalBet);
            DataSender.SHOW_TOTAL_BET(roomID, totalBet);

            switch (gameRound)
            {
                case 1:
                    //Show 3 card
                    ShowFlopCards(true);
                    break;
                case 2:
                    //ask second time question

                    AskCardToPlayer(beters[0], false);
                    break;
                case 3:
                    //Show 4th card
                    ShowTurnCards(true);
                    break;
                case 4:
                    //Show 5th card
                    ShowRiverCards(true);
                    break;
                case 5:
                    //end game
                    EndGame();
                    break;
            }
        }

        public void PlayerLeftGame(int connectionID, int player)
        {
            playerIn_Room[player].roomMoney = playerIn_Game[player].roomMoney;

            playerIn_Game.Remove(player);
        }

        public void ShowFlopCards(bool forceEnd)
        {
            gameRound++;

            TableCards.Add(Cards[0]);
            Cards.Remove(Cards[0]);

            TableCards.Add(Cards[0]);
            Cards.Remove(Cards[0]);

            TableCards.Add(Cards[0]);
            Cards.Remove(Cards[0]);


            DataSender.OPEN_FLOP_CARDS_BPR(roomID, TableCards[0], TableCards[1], TableCards[2]);


            Thread.Sleep(650);

            if(!forceEnd)
                SetTurnBetPlayer(beters[2], roundBet, false);
            else
            {
                ForceGameToEnd(gameRound);
            }

        }

        public void ShowTurnCards(bool forceEnd)
        {
            gameRound++;

            TableCards.Add(Cards[0]);
            Cards.Remove(Cards[0]);

            DataSender.OPEN_TURN_CARD_BPR(roomID, TableCards[3]);


            Thread.Sleep(650);

            if (!forceEnd) 
                SetTurnBetPlayer(beters[2], roundBet, false);
            else
            {
                ForceGameToEnd(gameRound);
            }
            
        }

        public void ShowRiverCards(bool forceEnd)
        {
            gameRound++;

            TableCards.Add(Cards[0]);
            Cards.Remove(Cards[0]);

            DataSender.OPEN_RIVER_CARD_BPR(roomID, TableCards[4]);


            Thread.Sleep(650);

            if (!forceEnd)
                SetTurnBetPlayer(beters[2], roundBet, false);
            else
            {
                ForceGameToEnd(gameRound);
            }
            
        }

        public void CheckWinner()
        {
            endTime = 0;
            gameTime = 0;


            int total = 0;
            foreach (var item in playersTotalBet)
                total += item.Value;


            if (playerIn_Game.Count < 2)
            {
                foreach (var item in playerIn_Game)
                {
                    int refreshMoney = playerIn_Game[item.Key].roomMoney + total;
                    playerIn_Game[item.Key].roomMoney = refreshMoney;
                    playerIn_Room[item.Key].roomMoney = playerIn_Game[item.Key].roomMoney;

                    Thread.Sleep(1000);

                    DataSender.SHOW_BPR_WINNER(roomID, item.Key, total, item.Value.roomMoney);
                }

                return;
            }

            Dictionary<int, PlayingPlayer> playerInCheck = new Dictionary<int, PlayingPlayer>();
            int[] cardsValues = new int[8];

            for (int i = 1; i <= 9; i++)
            {
                if (playerIn_Game.ContainsKey(i))
                {
                    cardsValues[0] = playerIn_Game[i].playerCards[0];
                    cardsValues[1] = playerIn_Game[i].playerCards[1];
                    cardsValues[2] = playerIn_Game[i].playerCards[2];
                    cardsValues[3] = TableCards[0];
                    cardsValues[4] = TableCards[1];
                    cardsValues[5] = TableCards[2];
                    cardsValues[6] = TableCards[3];
                    cardsValues[7] = TableCards[4];

                    playerInCheck.Add(i, CheckHandsTurkishPoker.CheckCards(cardsValues, playerIn_Game[i].ActiveID, i));
                }
            }

            Dictionary<int, int> playerPrize = CheckHandsTurkishPoker.SeperateMoney(playersTotalBet, playerInCheck);


            ///// Show Players Cards
            ///


            Thread.Sleep(1000);


            ShowPlayerCards();



            Thread.Sleep(2000);

            foreach (var item in playerPrize)
            {
                int refreshMoney = playerIn_Game[item.Key].roomMoney + item.Value;
                playerIn_Game[item.Key].roomMoney = refreshMoney;


                Thread.Sleep(1000);
                endTime = 0;

                DataSender.SHOW_BPR_WINNER(roomID, item.Key, item.Value, playerIn_Game[item.Key].roomMoney);
                
            }

            totalWinner = playerPrize.Count;

            foreach (var item in playerIn_Game)
                playerIn_Room[item.Key].roomMoney = item.Value.roomMoney;
        }

        public void ShowPlayerCards()
        {
            foreach (var player in playerIn_Game)
                    DataSender.SHOW_PLAYER_CARDS(player.Value.roomChair, player.Value.ActiveID, player.Value.playerCards[0], player.Value.playerCards[1], player.Value.playerCards[2], roomID);
        }
    }
}
