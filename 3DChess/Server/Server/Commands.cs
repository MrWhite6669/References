using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPWhite;

namespace Server
{
    class Commands
    {
        public static bool ExecuteComand(string command)
        {

            string[] data = command.Split(' ');
            switch (data[0])
            {
                default:
                    {
                        return false;
                    }
                case "Kick":
                    {
                        Console.WriteLine("{0} was kicked! (debug)", data[1]); // Change in future!
                    }
                    break;
                case "exit": { }
                    break;
                case "Database":
                    {
                        GameManager.dtb.ShowClientsInfo();
                    }
                    break;
                case "AddClient":
                    {
                        if (data[1] != null || data[2] != null)
                        {
                            GameManager.dtb.AddClient(new ChessClient(data[1], data[2]));
                        }
                        else Console.WriteLine("Invalid syntax!");
                    }
                    break;
                case "MatchsInfo":
                    {
                        Matchmaking.Info();
                    }
                    break;
                case "AddClientToQueue":
                    {
                        Matchmaking.JoinQueue(GameManager.dtb.FindByName(data[1]));
                    }
                    break;
                case "Start":
                    {
                        TCPServer.BroadcastCallback(new Message(-20));
                    }
                    break;
                case "Online":
                    {
                        GameManager.GetOnlineInfo();
                    }
                    break;
                case "Ban":
                    {
                        GameManager.dtb.BanClient(data[1]);
                    }
                    break;
                case "Unban":
                    {
                        GameManager.dtb.UnbanClient(data[1]);
                    }
                    break;
            }
            return true;
        }

    }
}
