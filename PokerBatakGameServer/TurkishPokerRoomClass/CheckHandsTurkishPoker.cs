using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerBatakGameServer.TurkishPokerRoomClass
{

    /// <summary>
    /// 
    /// 1 club
    /// 2 spades
    /// 3 diamond
    /// 4 hearts
    /// 
    /// </summary>
    /// 


    public class CheckHandsTurkishPoker
    {
        private static int clubSum;
        private static int spadesSum;
        private static int diamondSum;
        private static int heartsSum;
        static int[] cardValue = new int[8];
        static int[] orginalCardValue = new int[8];
        public static PlayingPlayer _winnerPlayerDetails;
        public static int[] cards = new int[8];


        public static PlayingPlayer CheckCards(int[] _cards, int ActiveID, int _roomChair)
        {
            _winnerPlayerDetails = new PlayingPlayer();

            cards = _cards;
            Array.Sort(_cards);

            clubSum = 0;
            spadesSum = 0;
            diamondSum = 0;
            heartsSum = 0;
            for (int i = 0; i < 7; i++)
            {
                cardValue[i] = 0;
            }

            _winnerPlayerDetails.ActiveID = ActiveID;
            _winnerPlayerDetails.roomChair = _roomChair;



            for (int i = 0; i < 8; i++)
            {
                if (_cards[i] < 14)
                    clubSum++;
                else if (14 <= _cards[i] && _cards[i] < 27)
                    spadesSum++;
                else if (27 <= _cards[i] && _cards[i] < 40)
                    diamondSum++;
                else if (40 <= _cards[i] && _cards[i] < 53)
                    heartsSum++;



                cardValue[i] = _cards[i] % 13;

                if (cardValue[i] == 0)
                    cardValue[i] = 13;

            }

            orginalCardValue = cardValue;
            Array.Sort(cardValue);



            if (Kare())
                return _winnerPlayerDetails;
            else if (FullHouse())
                return _winnerPlayerDetails;
            else if (Flush())
                return _winnerPlayerDetails;
            else if (Kent())
                return _winnerPlayerDetails;
            else if (Set())
                return _winnerPlayerDetails;
            else if (TwoPairs())
                return _winnerPlayerDetails;
            else if (OnePair())
                return _winnerPlayerDetails;
            else if (Nothing())
                return _winnerPlayerDetails;


            //if the hand is nothing, than the player with highest card wins



            return _winnerPlayerDetails;


        }


        #region Controls

        private static bool OnePair()
        {
            if (cardValue[0] == cardValue[1])
            {
                _winnerPlayerDetails.Hand = 1;
                _winnerPlayerDetails.HigherCard[0] = cardValue[0];
                _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                _winnerPlayerDetails.HigherCard[2] = cardValue[5];
                _winnerPlayerDetails.HigherCard[3] = cardValue[4];
                return true;
            }
            else if (cardValue[1] == cardValue[2])
            {
                _winnerPlayerDetails.Hand = 1;
                _winnerPlayerDetails.HigherCard[0] = cardValue[1];
                if (cardValue[0] == 1)
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[6];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[5];
                }
                else
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[5];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[4];
                }

                return true;
            }
            else if (cardValue[2] == cardValue[3])
            {
                _winnerPlayerDetails.Hand = 1;
                _winnerPlayerDetails.HigherCard[0] = cardValue[2];
                if (cardValue[0] == 1)
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[6];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[5];
                }
                else
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[5];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[4];
                }

                return true;
            }
            else if (cardValue[3] == cardValue[4])
            {
                _winnerPlayerDetails.Hand = 1;

                _winnerPlayerDetails.HigherCard[0] = cardValue[3];
                if (cardValue[0] == 1)
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[6];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[5];
                }
                else
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[5];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[2];
                }

                return true;
            }
            else if (cardValue[4] == cardValue[5])
            {
                _winnerPlayerDetails.Hand = 1;

                _winnerPlayerDetails.HigherCard[0] = cardValue[4];
                if (cardValue[0] == 1)
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[6];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[3];
                }
                else
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[3];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[2];
                }

                return true;
            }
            else if (cardValue[5] == cardValue[6])
            {
                _winnerPlayerDetails.Hand = 1;

                _winnerPlayerDetails.HigherCard[0] = cardValue[5];
                if (cardValue[0] == 1)
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[4];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[3];
                }
                else
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[4];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[3];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[2];
                }

                return true;
            }
            else if (cardValue[6] == cardValue[7])
            {
                _winnerPlayerDetails.Hand = 1;

                _winnerPlayerDetails.HigherCard[0] = cardValue[6];
                if (cardValue[0] == 1)
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[4];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[3];
                }
                else
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[4];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[3];
                    _winnerPlayerDetails.HigherCard[3] = cardValue[2];
                }

                return true;
            }

            return false;
        }

        private static bool TwoPairs()
        {
            if (cardValue[0] == cardValue[1] && cardValue[2] == cardValue[3])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[2];
                _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                _winnerPlayerDetails.HigherCard[2] = cardValue[6];

                return true;
            }
            else if (cardValue[0] == cardValue[1] && cardValue[3] == cardValue[4])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[3];
                _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                _winnerPlayerDetails.HigherCard[2] = cardValue[6];

                return true;
            }
            else if (cardValue[0] == cardValue[1] && cardValue[4] == cardValue[5])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[4];
                _winnerPlayerDetails.HigherCard[1] = cardValue[0];

                _winnerPlayerDetails.HigherCard[2] = cardValue[6];

                return true;
            }
            else if (cardValue[0] == cardValue[1] && cardValue[5] == cardValue[6])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[5];
                _winnerPlayerDetails.HigherCard[1] = cardValue[0];

                _winnerPlayerDetails.HigherCard[2] = cardValue[4];

                return true;
            }
            else if (cardValue[0] == cardValue[1] && cardValue[6] == cardValue[7])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[6];
                _winnerPlayerDetails.HigherCard[1] = cardValue[0];

                _winnerPlayerDetails.HigherCard[2] = cardValue[5];

                return true;
            }
            else if (cardValue[1] == cardValue[2] && cardValue[3] == cardValue[4])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[3];
                _winnerPlayerDetails.HigherCard[1] = cardValue[1];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[2] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[2] = cardValue[6];

                return true;
            }
            else if (cardValue[1] == cardValue[2] && cardValue[4] == cardValue[5])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[4];
                _winnerPlayerDetails.HigherCard[1] = cardValue[1];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[2] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[2] = cardValue[6];

                return true;
            }
            else if (cardValue[1] == cardValue[2] && cardValue[5] == cardValue[6])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[5];
                _winnerPlayerDetails.HigherCard[1] = cardValue[1];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[2] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[2] = cardValue[4];

                return true;
            }
            else if (cardValue[1] == cardValue[2] && cardValue[6] == cardValue[7])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[6];
                _winnerPlayerDetails.HigherCard[1] = cardValue[1];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[2] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[2] = cardValue[5];

                return true;
            }
            else if (cardValue[2] == cardValue[3] && cardValue[4] == cardValue[5])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[4];
                _winnerPlayerDetails.HigherCard[1] = cardValue[2];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[2] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[2] = cardValue[6];

                return true;
            }
            else if (cardValue[2] == cardValue[3] && cardValue[5] == cardValue[6])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[5];
                _winnerPlayerDetails.HigherCard[1] = cardValue[2];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[2] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[2] = cardValue[4];

                return true;
            }
            else if (cardValue[2] == cardValue[3] && cardValue[6] == cardValue[7])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[6];
                _winnerPlayerDetails.HigherCard[1] = cardValue[2];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[2] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[2] = cardValue[5];

                return true;
            }
            else if (cardValue[3] == cardValue[4] && cardValue[5] == cardValue[6])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[5];
                _winnerPlayerDetails.HigherCard[1] = cardValue[3];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[2] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[2] = cardValue[2];

                return true;
            }
            else if (cardValue[3] == cardValue[4] && cardValue[6] == cardValue[7])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[6];
                _winnerPlayerDetails.HigherCard[1] = cardValue[3];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[2] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[2] = cardValue[5];

                return true;
            }
            else if (cardValue[4] == cardValue[5] && cardValue[6] == cardValue[7])
            {
                _winnerPlayerDetails.Hand = 2;

                _winnerPlayerDetails.HigherCard[0] = cardValue[6];
                _winnerPlayerDetails.HigherCard[1] = cardValue[4];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[2] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[2] = cardValue[3];

                return true;
            }


            return false;
        }

        public static bool Set()
        {
            if (cardValue[0] == cardValue[1] && cardValue[0] == cardValue[2])
            {
                _winnerPlayerDetails.Hand = 3;

                _winnerPlayerDetails.HigherCard[0] = cardValue[0];
                _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                _winnerPlayerDetails.HigherCard[2] = cardValue[5];

                return true;
            }
            else if (cardValue[1] == cardValue[2] && cardValue[1] == cardValue[3])
            {
                _winnerPlayerDetails.Hand = 3;

                _winnerPlayerDetails.HigherCard[0] = cardValue[1];
                if (cardValue[0] == 1)
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[6];
                }
                else
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[5];
                }

                return true;
            }
            else if (cardValue[2] == cardValue[3] && cardValue[2] == cardValue[4])
            {

                _winnerPlayerDetails.Hand = 3;

                _winnerPlayerDetails.HigherCard[0] = cardValue[2];
                if (cardValue[0] == 1)
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[6];
                }
                else
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[5];
                }

                return true;
            }
            else if (cardValue[3] == cardValue[4] && cardValue[3] == cardValue[5])
            {
                _winnerPlayerDetails.Hand = 3;

                _winnerPlayerDetails.HigherCard[0] = cardValue[3];
                if (cardValue[0] == 1)
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[6];
                }
                else
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[2];
                }

                return true;
            }
            else if (cardValue[4] == cardValue[5] && cardValue[4] == cardValue[6])
            {
                _winnerPlayerDetails.Hand = 3;

                _winnerPlayerDetails.HigherCard[0] = cardValue[4];
                if (cardValue[0] == 1)
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[3];
                }
                else
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[3];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[2];
                }

                return true;
            }
            else if (cardValue[7] == cardValue[5] && cardValue[5] == cardValue[6])
            {
                _winnerPlayerDetails.Hand = 3;

                _winnerPlayerDetails.HigherCard[0] = cardValue[5];
                if (cardValue[0] == 1)
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[4];
                }
                else
                {
                    _winnerPlayerDetails.HigherCard[1] = cardValue[4];
                    _winnerPlayerDetails.HigherCard[2] = cardValue[3];
                }

                return true;
            }



            return false;
        }

        public static bool Flush()
        {
            if (clubSum > 4)
            {
                for (int i = 7; i >= 0; i--)
                {
                    if(cards[orginalCardValue.ToList().IndexOf(cardValue[0])] == 1)
                    {
                        _winnerPlayerDetails.HigherCard[0] = cardValue[0];
                        break;
                    }
                    if (cards[orginalCardValue.ToList().IndexOf(cardValue[i])] <= 13)
                    {
                        _winnerPlayerDetails.HigherCard[0] = cardValue[i];
                        break;
                    }
                }

                _winnerPlayerDetails.Hand = 5;
                return true;
            }
            else if (spadesSum > 4)
            {
                for (int i = 7; i >= 0; i--)
                {
                    if (cards[orginalCardValue.ToList().IndexOf(cardValue[0])] == 14)
                    {
                        _winnerPlayerDetails.HigherCard[0] = cardValue[0];
                        break;
                    }
                    if (cards[orginalCardValue.ToList().IndexOf(cardValue[i])] > 13 && cards[orginalCardValue.ToList().IndexOf(cardValue[i])] <= 26)
                    {
                        _winnerPlayerDetails.HigherCard[0] = cardValue[i];
                        break;
                    }
                }

                _winnerPlayerDetails.Hand = 5;
                return true;
            }
            else if (diamondSum > 4)
            {

                for (int i = 7; i >= 0; i--)
                {
                    if (cards[orginalCardValue.ToList().IndexOf(cardValue[0])] == 27)
                    {
                        _winnerPlayerDetails.HigherCard[0] = cardValue[0];
                        break;
                    }
                    if (cards[orginalCardValue.ToList().IndexOf(cardValue[i])] > 26 && cards[orginalCardValue.ToList().IndexOf(cardValue[i])] <= 39)
                    {
                        _winnerPlayerDetails.HigherCard[0] = cardValue[i];
                        break;
                    }
                }


                _winnerPlayerDetails.Hand = 5;
                return true;
            }
            else if (heartsSum > 4)
            {
                for (int i = 7; i >= 0; i--)
                {
                    if (cards[orginalCardValue.ToList().IndexOf(cardValue[0])] == 40)
                    {
                        _winnerPlayerDetails.HigherCard[0] = cardValue[0];
                        break;
                    }
                    if (cards[orginalCardValue.ToList().IndexOf(cardValue[i])] > 39 && cards[orginalCardValue.ToList().IndexOf(cardValue[i])] <= 52)
                    {
                        _winnerPlayerDetails.HigherCard[0] = cardValue[i];
                        break;
                    }
                }


                _winnerPlayerDetails.Hand = 5;
                return true;
            }



            return false;
        }

        public static bool Kare()
        {
            if (cardValue[0] == cardValue[1] && cardValue[0] == cardValue[2] && cardValue[0] == cardValue[3])
            {
                _winnerPlayerDetails.Hand = 7;
                _winnerPlayerDetails.HigherCard[0] = cardValue[0];
                _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                return true;
            }
            else if (cardValue[1] == cardValue[2] && cardValue[1] == cardValue[3] && cardValue[1] == cardValue[4])
            {
                _winnerPlayerDetails.Hand = 7;

                _winnerPlayerDetails.HigherCard[0] = cardValue[1];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[1] = cardValue[6];

                return true;
            }
            else if (cardValue[2] == cardValue[3] && cardValue[2] == cardValue[4] && cardValue[2] == cardValue[5])
            {
                _winnerPlayerDetails.Hand = 7;

                _winnerPlayerDetails.HigherCard[0] = cardValue[2];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[1] = cardValue[6];

                return true;
            }
            else if (cardValue[3] == cardValue[4] && cardValue[3] == cardValue[5] && cardValue[3] == cardValue[6])
            {
                _winnerPlayerDetails.Hand = 7;

                _winnerPlayerDetails.HigherCard[0] = cardValue[3];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[1] = cardValue[2];

                return true;
            }
            else if (cardValue[4] == cardValue[5] && cardValue[4] == cardValue[6] && cardValue[4] == cardValue[7])
            {
                _winnerPlayerDetails.Hand = 7;

                _winnerPlayerDetails.HigherCard[0] = cardValue[4];
                if (cardValue[0] == 1)
                    _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                else
                    _winnerPlayerDetails.HigherCard[1] = cardValue[3];

                return true;
            }


            return false;
        }

        public static bool FullHouse()
        {
            if (cardValue[0] == cardValue[1] && (cardValue[2] == cardValue[3] && cardValue[2] == cardValue[4]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[2];
                _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                return true;
            }
            else if (cardValue[0] == cardValue[1] && (cardValue[3] == cardValue[4] && cardValue[3] == cardValue[5]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[3];
                _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                return true;
            }
            else if (cardValue[0] == cardValue[1] && (cardValue[4] == cardValue[5] && cardValue[4] == cardValue[6]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[4];
                _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                return true;
            }
            else if (cardValue[0] == cardValue[1] && (cardValue[6] == cardValue[5] && cardValue[7] == cardValue[6]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[5];
                _winnerPlayerDetails.HigherCard[1] = cardValue[0];
                return true;
            }
            else if ((cardValue[1] == cardValue[2]) && (cardValue[3] == cardValue[4] && cardValue[3] == cardValue[5]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[3];
                _winnerPlayerDetails.HigherCard[1] = cardValue[1];
                return true;
            }
            else if (cardValue[1] == cardValue[2] && (cardValue[4] == cardValue[5] && cardValue[4] == cardValue[6]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[4];
                _winnerPlayerDetails.HigherCard[1] = cardValue[1];
                return true;
            }
            else if (cardValue[1] == cardValue[2] && (cardValue[6] == cardValue[5] && cardValue[7] == cardValue[6]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[5];
                _winnerPlayerDetails.HigherCard[1] = cardValue[1];
                return true;
            }
            else if ((cardValue[2] == cardValue[3]) && (cardValue[4] == cardValue[5] && cardValue[4] == cardValue[6]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[4];
                _winnerPlayerDetails.HigherCard[1] = cardValue[2];
                return true;
            }
            else if ((cardValue[2] == cardValue[3]) && (cardValue[5] == cardValue[6] && cardValue[7] == cardValue[6]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[5];
                _winnerPlayerDetails.HigherCard[1] = cardValue[2];
                return true;
            }
            else if ((cardValue[3] == cardValue[4]) && (cardValue[0] == cardValue[1] && cardValue[0] == cardValue[2]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[0];
                _winnerPlayerDetails.HigherCard[1] = cardValue[3];
                return true;
            }
            else if ((cardValue[3] == cardValue[4]) && (cardValue[5] == cardValue[6] && cardValue[7] == cardValue[5]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[5];
                _winnerPlayerDetails.HigherCard[1] = cardValue[3];
                return true;
            }
            else if (cardValue[4] == cardValue[5] && (cardValue[0] == cardValue[1] && cardValue[0] == cardValue[2]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[0];
                _winnerPlayerDetails.HigherCard[1] = cardValue[4];
                return true;
            }
            else if (cardValue[4] == cardValue[5] && (cardValue[1] == cardValue[2] && cardValue[1] == cardValue[3]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[1];
                _winnerPlayerDetails.HigherCard[1] = cardValue[4];
                return true;
            }
            else if (cardValue[5] == cardValue[6] && (cardValue[0] == cardValue[1] && cardValue[0] == cardValue[2]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[0];
                _winnerPlayerDetails.HigherCard[1] = cardValue[5];
                return true;
            }
            else if ((cardValue[5] == cardValue[6]) && (cardValue[1] == cardValue[2] && cardValue[1] == cardValue[3]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[1];
                _winnerPlayerDetails.HigherCard[1] = cardValue[5];
                return true;
            }
            else if ((cardValue[5] == cardValue[6]) && (cardValue[2] == cardValue[3] && cardValue[2] == cardValue[4]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[2];
                _winnerPlayerDetails.HigherCard[1] = cardValue[5];
                return true;
            }
            else if ((cardValue[6] == cardValue[7]) && (cardValue[0] == cardValue[1] && cardValue[2] == cardValue[0]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[0];
                _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                return true;
            }
            else if ((cardValue[6] == cardValue[7]) && (cardValue[2] == cardValue[1] && cardValue[2] == cardValue[3]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[2];
                _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                return true;
            }
            else if ((cardValue[6] == cardValue[7]) && (cardValue[2] == cardValue[3] && cardValue[2] == cardValue[4]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[2];
                _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                return true;
            }
            else if ((cardValue[6] == cardValue[7]) && (cardValue[3] == cardValue[4] && cardValue[5] == cardValue[3]))
            {
                _winnerPlayerDetails.Hand = 6;

                _winnerPlayerDetails.HigherCard[0] = cardValue[3];
                _winnerPlayerDetails.HigherCard[1] = cardValue[6];
                return true;
            }


            return false;
        }

        public static bool Kent()
        {

            if (cardValue[0] == cardValue[1] - 1 && cardValue[0] == cardValue[2] - 2 && cardValue[0] == cardValue[3] - 3 && cardValue[0] == cardValue[4] - 4)
            {
                _winnerPlayerDetails.Hand = 4;
                _winnerPlayerDetails.HigherCard[0] = cardValue[4];
                return true;
            }
            else if (cardValue[1] == cardValue[2] - 1 && cardValue[1] == cardValue[3] - 2 && cardValue[1] == cardValue[4] - 3 && cardValue[1] == cardValue[5] - 4)
            {
                _winnerPlayerDetails.Hand = 4;
                _winnerPlayerDetails.HigherCard[0] = cardValue[5];
                return true;
            }
            else if (cardValue[2] == cardValue[3] - 1 && cardValue[2] == cardValue[4] - 2 && cardValue[2] == cardValue[5] - 3 && cardValue[2] == cardValue[6] - 4)
            {
                _winnerPlayerDetails.Hand = 4;
                _winnerPlayerDetails.HigherCard[0] = cardValue[6];
                return true;
            }
            else if (cardValue[3] == cardValue[4] - 1 && cardValue[3] == cardValue[5] - 2 && cardValue[3] == cardValue[6] - 3 && cardValue[3] == cardValue[7] - 4)
            {
                _winnerPlayerDetails.Hand = 4;
                _winnerPlayerDetails.HigherCard[0] = cardValue[7];
                return true;
            }
            else if (cardValue[4] == cardValue[5] - 1 && cardValue[4] == cardValue[6] - 2 && cardValue[4] == cardValue[7] - 3 && cardValue[7] == cardValue[0] + 12)
            {
                _winnerPlayerDetails.Hand = 4;
                _winnerPlayerDetails.HigherCard[0] = cardValue[7];
                return true;
            }

            return false;
        }

        public static bool LineFlush()
        {
            if (Flush())
                if (Kent())
                    return true;

            return false;
        }

        public static bool Nothing()
        {
            _winnerPlayerDetails.HigherCard[0] = cardValue[6];
            _winnerPlayerDetails.HigherCard[1] = cardValue[5];
            _winnerPlayerDetails.HigherCard[2] = cardValue[4];
            _winnerPlayerDetails.HigherCard[3] = cardValue[3];
            _winnerPlayerDetails.HigherCard[3] = cardValue[2];
            return true;
        }
        #endregion


        public static Dictionary<int, int> SeperateMoney(Dictionary<int, int> _playerbet, Dictionary<int, PlayingPlayer> _PlayerInGame)
        {
            Dictionary<int, int> _playerBet;
            Dictionary<int, PlayingPlayer> _playerInGame;
            List<List<PlayingPlayer>> _WinnersList = new List<List<PlayingPlayer>>();

            List<PlayingPlayer> _firstWinners = new List<PlayingPlayer>();
            List<PlayingPlayer> _secondWinners = new List<PlayingPlayer>();
            List<PlayingPlayer> _thirdWinners = new List<PlayingPlayer>();
            List<PlayingPlayer> _fourthWinners = new List<PlayingPlayer>();
            List<PlayingPlayer> _fifthWinners = new List<PlayingPlayer>();
            List<PlayingPlayer> _sixthWinners = new List<PlayingPlayer>();
            List<PlayingPlayer> _seventhWinners = new List<PlayingPlayer>();
            List<PlayingPlayer> _eighthWinners = new List<PlayingPlayer>();
            List<PlayingPlayer> _ninethWinners = new List<PlayingPlayer>();
            _WinnersList.Add(_firstWinners);
            _WinnersList.Add(_secondWinners);
            _WinnersList.Add(_thirdWinners);
            _WinnersList.Add(_fourthWinners);
            _WinnersList.Add(_fifthWinners);
            _WinnersList.Add(_sixthWinners);
            _WinnersList.Add(_seventhWinners);
            _WinnersList.Add(_eighthWinners);
            _WinnersList.Add(_ninethWinners);


            _playerInGame = _PlayerInGame;
            _playerBet = _playerbet;



            #region setWiners
            List<PlayingPlayer> check = new List<PlayingPlayer>();

            for (int j = 0; j < 9; j++)
            {
                int _WinnerHand = -1;
                int[] _HigherCard = new int[5];

                for (int k = 1; k <= _playerInGame.Count; k++)
                {
                    if (_playerInGame.ContainsKey(k))
                    {
                        if (!check.Contains(_playerInGame[k]))
                        {
                            if (_playerInGame[k].Hand > _WinnerHand)
                            {
                                _WinnerHand = _playerInGame[k].Hand;
                                _HigherCard = _playerInGame[k].HigherCard;

                                _WinnersList[j].Clear();
                                _WinnersList[j].Add(_playerInGame[k]);


                            }
                            else if (_playerInGame[k].Hand == _WinnerHand)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    if (_playerInGame[k].HigherCard[i] > _HigherCard[i])
                                    {
                                        _WinnerHand = _playerInGame[k].Hand;
                                        _HigherCard = _playerInGame[k].HigherCard;
                                        _WinnersList[j].Clear();
                                        _WinnersList[j].Add(_playerInGame[k]);
                                        break;
                                    }
                                    else if (_playerInGame[k].HigherCard[i] < _HigherCard[i])
                                    {
                                        break;
                                    }
                                    else if (_playerInGame[k].HigherCard[i] == _HigherCard[i])
                                    {

                                        if (i == 4)
                                        {
                                            _WinnersList[j].Add(_playerInGame[k]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //foreach (var item in _WinnersList[j])
                //    Console.WriteLine(item.Hand + "  " + item.ActiveID + "  " + item.roomChair + "  " + j + " " + _playerBet[item.roomChair]);


                foreach (var item in _WinnersList[j])
                {
                    check.Add(item);
                }


            }

            #endregion

            Dictionary<int, int> playerPrices = new Dictionary<int, int>();

            for (int i = 0; i < 9; i++)
            {
                while (_WinnersList[i].Count > 0)
                {
                    int winnerBet = 0;
                    int totalWinning = 0;
                    foreach (var winner in _WinnersList[i])
                    {
                        if (winnerBet == 0 || _playerBet[winner.roomChair] < winnerBet)
                            winnerBet = _playerBet[winner.roomChair];
                    }
                    foreach (var item in _playerInGame)
                    {
                        if (_playerBet.ContainsKey(item.Key))
                        {
                            if (_playerBet[item.Key] > winnerBet)
                            {
                                totalWinning += winnerBet;
                                int x = _playerBet[item.Key] - winnerBet;
                                _playerBet.Remove(item.Key);
                                _playerBet.Add(item.Key, x);
                            }
                            else
                            {
                                totalWinning += _playerBet[item.Key];
                                int x = _playerBet[item.Key] - winnerBet;
                                _playerBet.Remove(item.Key);
                                _playerBet.Add(item.Key, 0);
                            }
                        }
                    }
                    int shareMoney = totalWinning / _WinnersList[i].Count;
                    foreach (var item in _WinnersList[i])
                    {
                        if (!playerPrices.ContainsKey(item.roomChair))
                            playerPrices.Add(item.roomChair, shareMoney);
                        else
                        {
                            int x = playerPrices[item.roomChair] + shareMoney;
                            playerPrices.Remove(item.roomChair);
                            playerPrices.Add(item.roomChair, x);
                        }
                    }
                    foreach (var item in _playerInGame)
                        if (_playerBet[item.Key] <= 0)
                        {
                            //_playerBet.Remove(item.Key);
                            foreach (var win in _WinnersList)
                                if (win.Contains(_playerInGame[item.Key]))
                                {
                                    win.Remove(_playerInGame[item.Key]);
                                    break;
                                }

                        }

                }

            }



            return playerPrices;
        }

    }

}
