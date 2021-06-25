using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCPWhite
{
    public static class Access
    {
        static List<string> banned = new List<string>();

        public static void Ban(string ip)
        {
            if (!banned.Contains(ip)) banned.Add(ip);
            else Console.WriteLine("Already banned.");
        }

        public static void Unban(string ip)
        {
            if (banned.Contains(ip)) banned.Remove(ip);
            else Console.WriteLine("This ip is not banned.");
        }

        public static bool IsBanned(string ip)
        {
            foreach(string i in banned)
            {
                if (i == ip) return true;
            }
            return false;
        }

        public static void ShowBanned()
        {
            Console.WriteLine("Banned IPs:");
            foreach(string s in banned)
            {
                Console.WriteLine(s);
            }
        }

        public static void Load()
        {
            try
            {
                using (BinaryReader br = new BinaryReader(File.Open("Banned.bin", FileMode.Open)))
                {
                    int length = br.ReadInt32();
                    for(int i = 0; i < length; i++)
                    {
                        banned.Add(br.ReadString());
                    }
                }
            }
            catch { Save(); }
        }

        public static void Save()
        {
            using(BinaryWriter bw = new BinaryWriter(File.Open("Banned.bin", FileMode.Create)))
            {
                bw.Write(banned.Count);
                foreach(string i in banned)
                {
                    bw.Write(i);
                }
            }
        }

    }
}
