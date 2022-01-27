using PokerBatakGameServer;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace NetworkServer
{
    class Client
    {
        public int connectionID;
        public TcpClient socket;
        public NetworkStream stream;
        private byte[] recBuffer;
        public ByteBuffer buffer;

        public void Start()
        {
            try
            {
                socket.SendBufferSize = 4096;
                socket.ReceiveBufferSize = 4096;
                stream = socket.GetStream();
                recBuffer = new byte[4096];
                stream.BeginRead(recBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);
                Console.WriteLine("Incoming connection from '{0}'.", socket.Client.RemoteEndPoint.ToString());
                
            }
            catch
            {
                CloseConnection();
            }
        }

        private void OnReceiveData(IAsyncResult result)
        {
            try
            {
                int length = stream.EndRead(result);
                if (length <= 0)
                {
                    CloseConnection();
                    return;
                }

                byte[] newBytes = new byte[length];
                Array.Copy(recBuffer, newBytes, length);
                ServerHandleData.HandleData(connectionID, newBytes);
                stream.BeginRead(recBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);
            }
            catch (Exception)
            {

                CloseConnection();
                return;
            }
        }

        private void CloseConnection()
        {
            try
            {
                Console.WriteLine("Connection from '{0}' has been terminated.", socket.Client.RemoteEndPoint.ToString());

                if (PlayerManager.active_Player.ContainsKey(connectionID))
                    PlayerManager.LogoutPlayer(connectionID);

                if (ClientManager.client.ContainsKey(connectionID))
                    ClientManager.client.Remove(connectionID);


                socket.Close();
            }
            catch
            {
                Console.WriteLine("Undefined Connection has been terminated.");                
            }
        }
    }
}