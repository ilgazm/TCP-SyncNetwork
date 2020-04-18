using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClient
{
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Client";
            var process = Process.GetCurrentProcess();
            process.EnableRaisingEvents = true;

            //AppDomain.CurrentDomain.ProcessExit += ApplicationExit;

            Client.Connect();
        }

        //private static async void ApplicationExit(object sender, EventArgs e)
        //{
        //    await Client.networkStream.DisposeAsync();
        //    Client.networkStream.Close();
        //    Client._tcpClient.Close();
        //}
    }
}