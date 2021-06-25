using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPWhite;

namespace Server
{
    class Match
    {

        public enum Turn
        {
            White,
            Black
        }

        ChessClient black;
        ChessClient white;

        public int Id;
        public static int count = 0;
        private Turn turn = Turn.White;

        public Match()
        {
            Id = count++;
        }

        public void JoinMatch(ChessClient client)
        {
            if (black != null) white = client;
            else black = client;
        }

        public void StartMatch()
        {
            SendMessages();
        }

        public bool DetectConnection()
        {
            if (!black.GetConnection().connection.Connected || !white.GetConnection().connection.Connected) return false;
            return true;
        }

        public ChessClient DetectConnected()
        {
            if (!black.GetConnection().connection.Connected) return white;
            if (!white.GetConnection().connection.Connected) return black;
            return null;
        }

        void SendMessages()
        {
            Message msg = new Message(ID.MatchAnswer);
            msg.data.Add("black", black.Name);
            msg.data.Add("white", white.Name);
            if (black.GetConnection() != null) black.GetConnection().SendCallback(msg);
            if (white.GetConnection() != null) white.GetConnection().SendCallback(msg);
            if (white.GetConnection() != null) white.GetConnection().SendCallback(new Message(ID.SwitchTurn));
        }

        public Turn CurrentTurn
        {
            get
            {
                return turn;
            }
            set
            {
                turn = value;
            }
        }

        public Turn ReturnColor(ChessClient client)
        {
            if (black == client) return Turn.Black;
            else if (white == client) return Turn.White;
            else throw new Exception("Player is not in match! (This should never happen!)");
        }

        public void Move(string posChange)
        {
            Message temp = new Message(ID.Move);
            temp.data.Add("status", "ok");
            temp.data.Add("position", posChange);
            black.GetConnection().SendCallback(temp);
            white.GetConnection().SendCallback(temp);
            SwitchTurn();
        }

        public void SwitchTurn()
        {
            if (CurrentTurn == Turn.Black)
            {
                CurrentTurn = Turn.White;
            }
            else
            {
                CurrentTurn = Turn.Black;
            }
            if (black.GetConnection() != null) black.GetConnection().SendCallback(new Message(ID.SwitchTurn));
            if (white.GetConnection() != null) white.GetConnection().SendCallback(new Message(ID.SwitchTurn));
        }

        public bool Contains(ChessClient client)
        {
            if (white == client || black == client) return true;
            return false;
        }

        public void Leave(ChessClient client)
        {
            if (white == client) black.GetConnection().SendCallback(new Message(ID.OpponentDisconnected));
            else white.GetConnection().SendCallback(new Message(ID.OpponentDisconnected));
            client.GetConnection().SendCallback(new Message(ID.LeaveMatch));
            EndMatch();
        }

        public void EndMatch()
        {
            Matchmaking.EndMatch(this);
        }

        public void Finish(ChessClient loser)
        {
            Message win = new Message(ID.EndMatch);
            Message lost = new Message(ID.EndMatch);

            win.data.Add("status", "win");
            lost.data.Add("status", "lost");

            if(black == loser)
            {
                white.GamesWon++;
                black.GamesLost++;
                white.GetConnection().SendCallback(win);
                black.GetConnection().SendCallback(lost);
            }
            else
            {
                black.GamesWon++;
                white.GamesLost++;
                black.GetConnection().SendCallback(win);
                white.GetConnection().SendCallback(lost);
            }

            EndMatch();
        }

        public override string ToString()
        {
            return string.Format("Match {0}: {1} aggainst {2}", Id, white.Name, black.Name);
        }

        public void SendMessage(string text, ChessClient sender)
        {
            Message msg = new Message(ID.ChatMessage);
            msg.data.Add("sender", sender.Name);
            msg.data.Add("text", text);
            if (black.GetConnection() != null) black.GetConnection().SendCallback(msg);
            if (white.GetConnection() != null) white.GetConnection().SendCallback(msg);
        }

        public void Check(ChessClient sender,string attacker)
        {
            Message msg = new Message(ID.Check);
            msg.data.Add("attacker", attacker);
            if (white == sender) black.GetConnection().SendCallback(msg);
            else white.GetConnection().SendCallback(msg);
        }

        public void ThreatenNode(string node , ChessClient sender)
        {
            Message msg = new Message(ID.ThreatenNode);
            msg.data.Add("node", node);

            if (sender == white) black.GetConnection().SendCallback(msg);
            else white.GetConnection().SendCallback(msg);
        }

    }
}
