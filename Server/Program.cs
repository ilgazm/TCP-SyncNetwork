using System;
using System.Diagnostics;
using System.Text;

namespace TcpServer
{
    internal static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.Title = "Server";

            Server.Start();
        }
    }
}