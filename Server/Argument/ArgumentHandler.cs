using System;
using System.Net;

namespace TcpServer.Argument
{
    internal static class ArgumentHandler
    {
        private const string prefix = "::0c";

        public static void Execute(string argument, EndPoint address)
        {
            var arguments = ArgumentSeparator(argument);
            var client = Server.Clients.Find(i => i.Address == address);

            switch (arguments[0])
            {
                case prefix + "0":
                    {
                        client.name = arguments[1];
                        break;
                    }
                case prefix + "1":
                    {
                        Console.WriteLine(client.Address + ": Ending connection...");
                        break;
                    }
            }
        }


        private static string[] ArgumentSeparator(string argument)
        {
            if (argument.Contains("+"))
            {
                return argument.Split("+", 2);
            }

            var argumentToReturn = new string[1];
            argumentToReturn[0] = argument;
            return argumentToReturn;
        }
    }
}