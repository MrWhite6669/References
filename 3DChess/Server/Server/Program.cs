using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCPWhite;

namespace Server
{
    class Program
    {
        static MessageHandler handler;

        static void Main(string[] args)
        {
            try
            {
                GameManager.LoadDatabase();
                //string ip = GetLocalIPAddress();
                Config.LoadConfig();
                Access.Load();
                TCPServer.StartListen(Config.data["IPAddress"], int.Parse(Config.data["Port"]));
                handler = new MessageHandler();
                Thread listen = new Thread(Listen);
                listen.Start();
                string command = "";
                while (command != "exit")
                {
                    command = Console.ReadLine();
                    if(!Commands.ExecuteComand(command))Console.WriteLine("Unknown command!");
                } 
                Access.Save();
                GameManager.SaveDatabase();
                Environment.Exit(0);
            }
            catch(Exception e)
            {
                foreach(Client c in TCPServer.clients)
                {
                    Message msg = new Message(1, "Server shutdown", "server");
                    msg.data.Add("sender", "Server");
                    byte[] array = msg.Serialize();
                    c.stream.Write(array,0,array.Length);
                    c.connection.Close();
                }
                Console.WriteLine(e);
                Console.ReadKey();
            }
           
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static void Listen()
        {
            while (true)
            {
                Matchmaking.WaitForMatch();
                Matchmaking.DetectDisconnectedFromMatch();
                Message temp;
                try
                {
                    temp = TCPServer.GetMessageFromQueue();
                    handler.Sort(temp);
                }
                catch { }
            }
        }
    }
}
