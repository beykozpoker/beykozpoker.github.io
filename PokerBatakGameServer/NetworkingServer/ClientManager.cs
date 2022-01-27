using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkServer
{
    class ClientManager
    {
        public static Dictionary<int, Client> client = new Dictionary<int, Client>();
        //public static Dictionary<int, int> clientDictinary = new Dictionary<int, int>();


        public static void CreateNewConnection(TcpClient tempClient)
        {
            Client newClient = new Client();
            newClient.socket = tempClient;
            newClient.connectionID = ((IPEndPoint)tempClient.Client.RemoteEndPoint).Port;
            //newClient.clientDictinaryID = client.Count;

            newClient.Start();
            client.Add(newClient.connectionID, newClient);
            DataSender.CHECK_CONNECTION(newClient.connectionID);
        }

        public static void SendDataTo(int connectionID, byte[] data)
        {
            if(client.ContainsKey(connectionID))
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
                buffer.WriteBytes(data);
                client[connectionID].stream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
                buffer.Dispose();
            }
            else
            {
                Console.WriteLine("bot");
            }
        }

        public static void SendDataToAll(int connectionID, byte[] data)
        {
            Dictionary<int, Client>.KeyCollection ClientList = client.Keys;

            foreach (int _connectionID in ClientList)
            {
                SendDataTo(connectionID, data);
            }

        }

    }
}
