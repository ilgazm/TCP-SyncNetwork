using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpServer
{
    public class Connections
    {
        public string name;
        
        //Network:
        public TcpClient Connection;

        public NetworkStream Stream;
        public readonly EndPoint Address;
        public bool _keepConnection;

        //Threads:
        public Thread ReadThread;
        public Thread ConnectionHandlerThread;



        public Connections(TcpClient client, EndPoint ip, NetworkStream stream)
        {
            this.Connection = client;
            this.Address = ip;
            this.Connection.Client.ReceiveBufferSize = Server.ByteLimit;
            this.Stream = stream;
            _keepConnection = true;
        }

        public void NetworkRead()
        {
            try
            {
                while (_keepConnection)
                {
                    var buffer = new byte[Server.ByteLimit];
                    var bytesRead = Stream.Read(buffer, 0, buffer.Length);
                    var dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Stream.Flush();

                    Console.WriteLine(Connection.Client.RemoteEndPoint + ": " + dataReceived);

                    if (dataReceived.StartsWith("::0c"))
                    {
                        Argument.ArgumentHandler.Execute(dataReceived, this.Address);
                    }
                    else
                    {
                        var writeThread = new Thread(() => NetworkWrite(dataReceived))
                            {Priority = ThreadPriority.Lowest};

                        writeThread.Start();
                    }
                }
            }
            catch (Exception e)
            {
                _keepConnection = false;
                Stream.Dispose();
                Connection.Close();
                Server.Clients.Remove(this);
                Console.WriteLine(e);
            }

        }

        private void NetworkWrite(string output)
        {
            var buffer = Encoding.UTF8.GetBytes(name + ": " + output);

            foreach (var client in Server.Clients)
            {
                if (client.Address == this.Address) continue;

                client.Stream.Write(buffer, 0, buffer.Length);
                client.Stream.Flush();
            }
        }


    }
}