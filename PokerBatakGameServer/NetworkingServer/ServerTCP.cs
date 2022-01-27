using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetworkServer
{
    static class ServerTCP
    {
        static TcpListener serverSocket = new TcpListener(IPAddress.Any, 7777);

        public static void InitializeNetwork()
        {
            Console.WriteLine("Initializing Packets...");
            ServerHandleData.InitializePackets();
            serverSocket.Start();
            serverSocket.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
            Console.WriteLine("Server is running...");
        }

        private static void OnClientConnect(IAsyncResult result)
        {
            TcpClient client = serverSocket.EndAcceptTcpClient(result);
            if (client.Client.RemoteEndPoint.ToString() == "")
                return;
            serverSocket.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
            ClientManager.CreateNewConnection(client);
        }

    }
}
