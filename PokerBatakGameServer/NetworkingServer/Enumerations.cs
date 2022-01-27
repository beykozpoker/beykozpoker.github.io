using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkServer
{
    class Enumerations
    {
        public enum ServerPackets
        {
            ALERT_MESSAGES = 199,
            SIGN_IN = 1,
            LOG_IN = 2,
            PLAYER_ENTERED_POKER_ROOM = 3,
            PLAYER_LEFT_POKER_ROOM = 4,

            POKER_ROOM_ANSWER = 6,
            TURN_PLAYER = 9,
            START_GAME = 10,
            END_GAME = 11,
            LOG_OUT = 12,
            SEND_FIRST_THREE_CARDS = 13,
            SEND_FOURTH_CARD = 14,
            SEND_FIFTH_CARD = 15,
            CHECK_HOURLY_BONUS = 16,
            CHECK_DAYLY_BONUS = 17,
            LEAVE_ROOM = 18,

            REFRESH_MONEY = 20,

            BJT_ROOM_ANSWER = 21,
            PLAYER_ENTERED_BJT_ROOM = 22,
            START_BJT_GAME = 23,
            TURN_BJT_PLAYER = 24,
            BJ_TOURNAMENT_TAKE_CARD = 25,
            END_BJT_GAME = 27,
            PLAYER_LEFT_BJT_ROOM = 28,

            TP_ROOM_ANSWER = 29,
            PLAYER_ENTERED_TP_ROOM = 30,
            PLAYER_LEFT_TP_ROOM = 31,
            ASK_CARD_TO_PLAYER = 32,
            TURN_BET_PLAYER = 33,
            PLAYER_ACCEPT_CARD = 34,

            OPEN_FLOP_CARDS_BPR = 35,
            OPEN_TURN_CARD_BPR = 36,
            OPEN_RIVER_CARD_BPR = 37,
            TPROOM_END_GAME = 38,
            SHOW_BPR_WINNER = 39,
            PLAYER_BETED_BPR = 40,
            START_BPGAME = 41,
            PAY_BACK = 43,
            SHOW_TOTAL_BET = 44,

            PLAYER_MESSAGING = 42,
            SHOW_PLAYER_CARDS = 45,
            CHECK_CONNECTION = 46,
        }

        public enum ClientPackets
        {
            ALERT_MESSAGES = 199,

            LOG_IN = 1,
            POKER_ROOM_REQUEST = 2,
            BET_PASS = 4,
            BET_OK = 5,
            BET_UP = 6,
            CREATE_NEW_ACCOUNT = 7,
            LOG_OUT = 8,
            CHANGE_IMG = 9,
            
            CHANGE_NAME = 22,
            COLLECT_REEDEM = 23,

            CHECK_HOURLY_BONUS = 10,
            COLLECT_HOURLY_BONUS = 11,
            COLLECT_DAILY_BONUS = 12,
            LEAVE_ROOM = 13,

            BJ_TOURNAMENT_ROOM_REQUEST = 14,
            BJ_TOURNAMENT_TAKE_CARD = 15,
            BJ_TOURNAMENT_BETDOUBLE = 16,
            BJ_TOURNAMENT_OK = 17,


            TURKISH_POKER_ROOM_REQUEST = 18,
            RECEIVE_CARD_ANSWER = 19,
            RECEIVE_BET = 20,


            RECEIVE_PLAYER_MESSAGE = 21,
            GIVE_REDEEM = 24,
        }

    }
}
