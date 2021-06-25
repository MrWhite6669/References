using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPWhite;

namespace Server
{
    static class Matchmaking
    {
        static Queue<ChessClient> matchQueue = new Queue<ChessClient>();
        static List<Match> matchsInProgress = new List<Match>();

        public static void JoinQueue(ChessClient player)
        {
            matchQueue.Enqueue(player);
        }

        public static void WaitForMatch()
        {
            if (matchQueue.Count > 1) JoinMatch(matchQueue.Dequeue(), matchQueue.Dequeue());
        }

        public static void DetectDisconnectedFromMatch()
        {
            ChessClient temp = new ChessClient();
            for (int i = 0; i < matchsInProgress.Count; i++)
            {
                if (!matchsInProgress[i].DetectConnection())
                {
                    temp = matchsInProgress[i].DetectConnected();
                    temp.GetConnection().SendCallback(new Message(ID.OpponentDisconnected));
                    matchsInProgress.Remove(matchsInProgress[i]);
                }
            }
        }

        public static void JoinMatch(ChessClient player1,ChessClient player2)
        {
            Match temp = new Match();
            temp.JoinMatch(player1);
            temp.JoinMatch(player2);
            temp.StartMatch();
            matchsInProgress.Add(temp);
        }

        public static Match FindMatchByConnection(Client connection)
        {
            ChessClient client = GameManager.dtb.FindByConnection(connection);
            foreach(Match m in matchsInProgress)
            {
                if (m.Contains(client)) return m;
            }
            return null;
        }

        public static void EndMatch(Match match)
        {
            matchsInProgress.Remove(match);
        }

        public static void Info()
        {
            foreach(Match m in matchsInProgress)
            {
                Console.WriteLine(m);
            }
        }

    }
}
