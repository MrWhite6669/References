using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPWhite;
using WindowsInput;

namespace Server
{
    public class MessageHandler
    {

        public void Sort(Message msg)
        {
            switch (msg.id)
            {
                default:
                    {
                        Console.WriteLine(msg);
                    }
                    break;
                case ID.RegisterRequest:
                    {
                        GameManager.Register(msg.data["name"], msg.data["password"],msg.sender);
                    }
                    break;
                case ID.LoginRequest:
                    {
                        GameManager.Login(msg.data["name"], msg.data["password"], msg.sender);
                    }
                    break;
                case ID.LogoutRequest:
                    {
                        GameManager.Logout(msg.sender);
                    }
                    break;
                case ID.InfoRequest:
                    {
                        Message temp = new Message(ID.InfoAnswer);
                        temp.data.Add("wins", GameManager.GetWins(msg.sender).ToString());
                        temp.data.Add("losts", GameManager.GetLosts(msg.sender).ToString());
                        temp.data.Add("name", GameManager.GetName(msg.sender));
                        msg.sender.SendCallback(temp);
                    }
                    break;
                case ID.MatchRequest:
                    {
                        Matchmaking.JoinQueue(GameManager.dtb.FindByConnection(msg.sender));
                    }
                    break;
                case ID.Move:
                    {
                        Match match = Matchmaking.FindMatchByConnection(msg.sender);
                        ChessClient sender = GameManager.dtb.FindByConnection(msg.sender);
                        if (match.CurrentTurn == match.ReturnColor(sender))
                        {
                            match.Move(msg.data["position"]);
                        }
                        else
                        {
                            Message temp = new Message(ID.Move);
                            temp.data.Add("status", "error");
                            msg.sender.SendCallback(temp);
                        }
                    }
                    break;
                case ID.LeaveMatch:
                    {
                        Matchmaking.FindMatchByConnection(msg.sender).Leave(GameManager.dtb.FindByConnection(msg.sender));
                    }
                    break;
                case ID.ChatMessage:
                    {
                        Match match = Matchmaking.FindMatchByConnection(msg.sender);
                        match.SendMessage(msg.data["text"], GameManager.dtb.FindByConnection(msg.sender));
                    }
                    break;
                case ID.Check:
                    {
                        Match match = Matchmaking.FindMatchByConnection(msg.sender);
                        match.Check(GameManager.dtb.FindByConnection(msg.sender),msg.data["attacker"]);
                    }
                    break;
                case ID.Mate:
                    {
                        Match match = Matchmaking.FindMatchByConnection(msg.sender);
                        match.Finish(GameManager.dtb.FindByConnection(msg.sender));
                    }
                    break;
                case ID.ThreatenNode:
                    {
                        ChessClient client = GameManager.dtb.FindByConnection(msg.sender);
                        Match match = Matchmaking.FindMatchByConnection(msg.sender);

                        match.ThreatenNode(msg.data["node"], client);

                    }
                    break;
            }
        }
    }
}
