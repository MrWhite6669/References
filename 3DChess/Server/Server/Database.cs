using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPWhite;

namespace Server
{
    public class Database
    {
        public List<ChessClient> clients = new List<ChessClient>();
        public int count;

        public bool CheckName(string name)
        {
            foreach(ChessClient client in clients)
            {
                if (client.Name == name)
                {
                    throw new Exception("Name already exists!");
                }
            }
            return true;
        }

        public void AddClient(ChessClient client)
        {
            if(client.Name != null)clients.Add(client);
        }

        public ChessClient FindByName(string name)
        {
            foreach(ChessClient c in clients)
            {
                if (name == c.Name) return c;
            }
            throw new Exception("User does not exist!");
        }

        public ChessClient FindByConnection(Client connection)
        {
            foreach (ChessClient c in clients)
            {
                if (connection == c.GetConnection()) return c;
            }
            throw new Exception("User logged out!");
        }

        public void BanClient(string name)
        {
            ChessClient temp = FindByName(name);
            if (temp != null)
            {
                foreach (string ip in temp.ipAddresses)
                {
                    Access.Ban(ip);
                }
                temp.Banned = true;
            }
        }

        public void UnbanClient(string name)
        {
            ChessClient temp = FindByName(name);
            if(temp != null)
            {
                foreach(string ip in temp.ipAddresses){
                    Access.Unban(ip);
                }
                temp.Banned = false;
            }
        }

        public void ShowClientsInfo()
        {
            foreach(ChessClient c in clients)
            {
                Console.WriteLine(c);
            }
        }

    }
}
