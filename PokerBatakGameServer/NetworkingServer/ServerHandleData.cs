using System.Collections.Generic;

namespace NetworkServer
{
    static class ServerHandleData
    {
        public delegate void Packet(int connectionID, byte[] data);
        public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

        public static void InitializePackets()
        {
            packets.Add((int)Enumerations.ClientPackets.ALERT_MESSAGES, DataReceiver.ALERT_MESSAGES);
            packets.Add((int)Enumerations.ClientPackets.CREATE_NEW_ACCOUNT, DataReceiver.CREATE_NEW_ACCOUNT);
            packets.Add((int)Enumerations.ClientPackets.LOG_IN, DataReceiver.LOG_IN);
            packets.Add((int)Enumerations.ClientPackets.LOG_OUT, DataReceiver.LOG_OUT);
            packets.Add((int)Enumerations.ClientPackets.POKER_ROOM_REQUEST, DataReceiver.POKER_ROOM_REQUEST);
            packets.Add((int)Enumerations.ClientPackets.BET_PASS, DataReceiver.BET_PASS);
            packets.Add((int)Enumerations.ClientPackets.BET_OK, DataReceiver.BET_OK);
            packets.Add((int)Enumerations.ClientPackets.BET_UP, DataReceiver.BET_UP);
            packets.Add((int)Enumerations.ClientPackets.CHANGE_IMG, DataReceiver.CHANGE_IMG);
            packets.Add((int)Enumerations.ClientPackets.CHECK_HOURLY_BONUS, DataReceiver.CHECK_HOURLY_BONUS);
            packets.Add((int)Enumerations.ClientPackets.COLLECT_HOURLY_BONUS, DataReceiver.COLLECT_HOURLY_BONUS);
            packets.Add((int)Enumerations.ClientPackets.COLLECT_DAILY_BONUS, DataReceiver.COLLECT_DAILY_BONUS);
            packets.Add((int)Enumerations.ClientPackets.LEAVE_ROOM, DataReceiver.LEAVE_ROOM);

            packets.Add((int)Enumerations.ClientPackets.BJ_TOURNAMENT_ROOM_REQUEST, DataReceiver.BJ_TOURNAMENT_ROOM_REQUEST);
            packets.Add((int)Enumerations.ClientPackets.BJ_TOURNAMENT_TAKE_CARD, DataReceiver.BJ_TOURNAMENT_TAKE_CARD);
            packets.Add((int)Enumerations.ClientPackets.BJ_TOURNAMENT_BETDOUBLE, DataReceiver.BJ_TOURNAMENT_BETDOUBLE);
            packets.Add((int)Enumerations.ClientPackets.BJ_TOURNAMENT_OK, DataReceiver.BJ_TOURNAMENT_OK);


            packets.Add((int)Enumerations.ClientPackets.TURKISH_POKER_ROOM_REQUEST, DataReceiver.TURKISH_POKER_ROOM_REQUEST);
            packets.Add((int)Enumerations.ClientPackets.RECEIVE_CARD_ANSWER, DataReceiver.RECEIVE_CARD_ANSWERR);
            packets.Add((int)Enumerations.ClientPackets.RECEIVE_BET, DataReceiver.RECEIVE_BET);


            packets.Add((int)Enumerations.ClientPackets.RECEIVE_PLAYER_MESSAGE, DataReceiver.RECEIVE_PLAYER_MESSAGE);
            packets.Add((int)Enumerations.ClientPackets.CHANGE_NAME, DataReceiver.CHANGE_NAME);
            packets.Add((int)Enumerations.ClientPackets.COLLECT_REEDEM, DataReceiver.COLLECT_REEDEM);
            packets.Add((int)Enumerations.ClientPackets.GIVE_REDEEM, DataReceiver.GIVE_REEDEM);
        }

        public static void HandleData(int connectionID, byte[] data)
        {
            byte[] buffer = (byte[])data.Clone();
            int pLength = 0;

            if (ClientManager.client[connectionID].buffer == null) ClientManager.client[connectionID].buffer = new ByteBuffer();

            ClientManager.client[connectionID].buffer.WriteBytes(buffer);
            if (ClientManager.client[connectionID].buffer.Count() == 0)
            {
                ClientManager.client[connectionID].buffer.Clear();
                return;
            }

            if (ClientManager.client[connectionID].buffer.Length() >= 4)
            {
                pLength = ClientManager.client[connectionID].buffer.ReadInteger(false);
                if (pLength <= 0)
                {
                    ClientManager.client[connectionID].buffer.Clear();
                    return;
                }
            }

            while (pLength > 0 & pLength <= ClientManager.client[connectionID].buffer.Length() - 4)
            {
                if (pLength <= ClientManager.client[connectionID].buffer.Length() - 4)
                {
                    ClientManager.client[connectionID].buffer.ReadInteger();
                    data = ClientManager.client[connectionID].buffer.ReadBytes(pLength);
                    HandleDataPackets(connectionID, data);
                }

                pLength = 0;
                if (ClientManager.client[connectionID].buffer.Length() >= 4)
                {
                    pLength = ClientManager.client[connectionID].buffer.ReadInteger(false);
                    if (pLength <= 0)
                    {
                        ClientManager.client[connectionID].buffer.Clear();
                        return;
                    }
                }
            }

            if (pLength <= 1)
            {
                ClientManager.client[connectionID].buffer.Clear();
            }
        }

        private static void HandleDataPackets(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            buffer.Dispose();
            if (packets.TryGetValue(packetID, out Packet packet))
            {
                packet.Invoke(connectionID, data);
            }
        }
    }
}
