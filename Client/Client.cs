using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleClient
{
    internal static class Client
    {
        //ID:
        private static string name;

        //Network:
        public static TcpClient _tcpClient = new TcpClient();
        public static readonly string ArgumentPrefix = "::0c";
        
        public static NetworkStream networkStream;
        private const int port = 9924;
        private static readonly int byteLimit = 1024;

        //Threads:
        private static readonly Thread ReceiveThread = new Thread(ReceiveData)
        {
            Priority = ThreadPriority.Normal
        };

        private static readonly Thread InputThread = new Thread(ReadInput)
        {
            Priority = ThreadPriority.Normal
        };

        public static void Connect()
        {
            Console.Write("Please define a username: ");
            var nameInput = Console.ReadLine();

            _tcpClient.Connect("127.0.0.1", port);
            networkStream = _tcpClient.GetStream();

            ReceiveThread.Start();
            InputThread.Start();

            SendData(ArgumentPrefix +  "0+".ToLower() + nameInput);
        }

        private static void ReceiveData()
        {
            while (true)
            {
                var data = new byte[byteLimit];

                var i = networkStream.Read(data, 0, data.Length);

                var input = Encoding.UTF8.GetString(data, 0, i);

                Console.WriteLine(input);
                networkStream.Flush();
            }
        }

        private static void ReadInput()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input) && input.StartsWith("::0c"))
                {
                    Console.WriteLine("Please provide a proper input!");
                }
                else
                {
                    SendData(input);
                }
            }
        }


        //TODO: IMPLEMENT SenDataAsync

        public static void SendData(string dataToSend)
        {
            Task.Run(() =>
            {
                if (string.IsNullOrEmpty(dataToSend))
                {
                    Console.WriteLine("Please provide a proper input!");
                }
                else
                {
                    var data = Encoding.UTF8.GetBytes(dataToSend);
                    networkStream.Write(data, 0, data.Length);
                    networkStream.Flush();
                    
                }
            });
        }
    }
}