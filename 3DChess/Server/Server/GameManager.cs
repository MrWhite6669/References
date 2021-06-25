using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using TCPWhite;

namespace Server
{
    static class GameManager
    {
        public static Database dtb = new Database();
        public static List<ChessClient> online = new List<ChessClient>();
        static XmlSerializer xs = new XmlSerializer(typeof(Database));

        public static void Register(string name,string password,Client connection)
        {
            try
            {
                ChessClient temp = new ChessClient(name, password);
                temp.SetConnection(connection);
                dtb.AddClient(temp);
                SaveDatabase();
                SendOkMessage(ID.RegisterAnswer, connection);
            }
            catch(Exception e)
            {
                SendErrorMessage(ID.RegisterAnswer, e.Message, connection);
            }
        }

        public static void SendOkMessage(int id,Client connection)
        {
            Message msg = new Message(id);
            msg.data.Add("status", "ok");
            connection.SendCallback(msg);
        }

        public static int GetWins(Client connection)
        {
            return dtb.FindByConnection(connection).GamesWon;
        }

        public static int GetLosts(Client connection)
        {
            return dtb.FindByConnection(connection).GamesLost;
        }

        public static string GetName(Client connection)
        {
            return dtb.FindByConnection(connection).Name;
        }

        public static void SendErrorMessage(int id , string reason, Client connection)
        {
            Message msg = new Message(id);
            msg.data.Add("status", "error");
            msg.data.Add("reason", reason);
            connection.SendCallback(msg);
        }

        public static void Login(string name,string password , Client connection)
        {
            try
            {
                ChessClient temp;
                if ((temp = dtb.FindByName(name)).Password != password) throw new Exception("Wrong password!");
                if (online.Contains(temp)) throw new Exception("User already online!");
                if (temp.Banned) throw new Exception("User Banned.");
                temp.SetConnection(connection);
                SendOkMessage(ID.LoginAnswer, connection);
                online.Add(temp);
            }
            catch (Exception e)
            {
                SendErrorMessage(ID.LoginAnswer, e.Message, connection);
            }
        }

        public static void Logout(Client connection)
        {
            ChessClient c = dtb.FindByConnection(connection);
            online.Remove(c);
            SendOkMessage(ID.LogoutAnswer, connection);
            c.GetConnection().Disconnect();
        }

        public static void SaveDatabase()
        {
            dtb.count = ChessClient.count;
            xs.Serialize(File.Open("Database.xml", FileMode.OpenOrCreate), dtb);
        }

        public static void GetOnlineInfo()
        {
            for (int i = 0; i < online.Count; i++)
            {
                Console.WriteLine(online[i]);
            }
        }

        public static void LoadDatabase()
        {
            if (File.Exists("Database.xml")) { dtb = (Database)xs.Deserialize(File.Open("Database.xml", FileMode.Open)); ChessClient.count = dtb.count; }
        }
    }
}
