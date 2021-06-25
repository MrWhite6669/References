using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading;

namespace TCPWhite
{
    public static class TCPServer
    {
        static TcpListener listen;
        public static List<Client> clients = new List<Client>();
        public static List<Message> queue = new List<Message>();
        
        public static void StartListen(string ip, int port)
        {
            listen = new TcpListener(IPAddress.Parse(ip), port);
            listen.Start();
            Console.WriteLine("Listening on {1} with port {0}...", port, ip);
            WaitForClientConnect();
        }

        private static async void WaitForClientConnect()
        {
            TcpClient client = await listen.AcceptTcpClientAsync();
            OnClientConnect(client);
        }

        private static void OnClientConnect(TcpClient client)
        {
            Client newClient = new Client(client);
            Console.WriteLine("Client connected on {0}", newClient.ip);
            if (!Access.IsBanned(newClient.ip))
            {
                clients.Add(newClient);
            }
            else
            {
                Console.WriteLine("Banned ip tried to connect: {0}", newClient.ip);
                newClient.SendCallback(new Message(-1, "You are banned", "server"));
                newClient.Disconnect();
            }

            WaitForClientConnect();
        }

        public static void BroadcastCallback(Message callback)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].SendCallback(callback);
            }
        }

        public static void AddToQueue(Message msg)
        {
            queue.Add(msg);
        }

        public static Message GetMessageFromQueue()
        {
            Message temp;
            try
            {
                temp = queue.Last();
                queue.Remove(queue.Last());
                return temp;
            }
            catch
            {
                return null;
            }
        }
    }
}
