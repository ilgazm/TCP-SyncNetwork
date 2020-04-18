using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpServer
{
    internal static class Server
    {
        //Network variables
        private const int port = 9924;

        private static TcpListener _tcpListener;
        public static List<Connections> Clients = new List<Connections>();
        public static readonly int ByteLimit = 1024;

        //Threads:
        private static readonly Thread _listenThread = new Thread(ListenConnections);

        public static void Start()
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _tcpListener.Start();

            _listenThread.Start();
        }

        //Multi Thread: _listenThread
        private static void ListenConnections()
        {
            while (true)
            {
                //Storing the variable prior to being added into the Clients list.
                var tempTcpClient = _tcpListener.AcceptTcpClient();

                Clients.Add(new Connections(client: tempTcpClient,
                    ip: tempTcpClient.Client.RemoteEndPoint,
                    stream: tempTcpClient.GetStream()));

                var client = Clients.Find(i => i.Address == tempTcpClient.Client.RemoteEndPoint);
                Console.WriteLine("Received connection from: " + client.Address);

                client.ReadThread = new Thread(client.NetworkRead)
                {
                    Priority = ThreadPriority.Lowest
                };
                client.ReadThread.Start();

            }
        }
    }
}