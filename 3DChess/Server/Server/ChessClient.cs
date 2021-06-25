using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPWhite;

namespace Server
{
    public class ChessClient
    {
        //Authentication Info
        public string Name { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }

        public List<string> ipAddresses = new List<string>();
        public bool Banned { get; set; }

        //Game Info
        public int GamesWon = 0;
        public int GamesLost = 0;

        private Client connection;

        public static int count = 0;

        //Password requiements
        private int maxCharacters = 10;
        private int minCharacters = 4;
        private string allowedChars = "qwertzuiopasdfghjklyxcvbnm123456789";

        public ChessClient(string name,string password)
        {
            if (!GameManager.dtb.CheckName(name)) return;
            CheckPassword(password);
            CheckLenght(name);
            Name = name;
            Password = password;
            Id = count++;
        }

        public void AddIp(string address)
        {
            if (!ipAddresses.Contains(address)) ipAddresses.Add(address);
        }

        public ChessClient()
        {
            Name = "";
            Password = "";
            Id = -1;
        }

        public void SetConnection(Client connection)
        {
            this.connection = connection;
            AddIp(connection.ip);
        }

        public Client GetConnection()
        {
            return connection;
        }

        void CheckPassword(string password)
        {
            string temp = password.ToLower();
            if (temp.Length < minCharacters || temp.Length > maxCharacters || !CheckChars(temp))
            {
                throw new Exception(string.Format("Password must be longer than {0} characters and shorter than {1} characters and must contain only ({2}).", minCharacters, maxCharacters, allowedChars));
            }
        }

        bool CheckChars(string text)
        {
            foreach(char c in text)
            {
                if (!allowedChars.Contains(c)) return false;
            }
            return true;
        }

        void CheckLenght(string name)
        {
            if (name.Length < 4) throw new Exception("Name must be more than 4 characters long!");
        }

        public override string ToString()
        {
            return string.Format("Client {0}: {1} , password: {2}", Id, Name, Password);
        }
    }
}
