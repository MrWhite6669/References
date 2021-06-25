using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskKiller
{
    static class TaskManager
    {
        public static MainForm current;

        public static List<string> GetTasks()
        {
            Process[] processes = Process.GetProcesses();
            List<string> tasks = new List<string>();
            foreach (var proc in processes)
            {
                if (!string.IsNullOrEmpty(proc.ProcessName))
                    tasks.Add(proc.ProcessName);
            }
            tasks.Sort();
            return tasks;
        }

        public static void KillTasks(string file)
        {
            try
            {
                using (StreamReader sr = new StreamReader("KillList.tk"))
                {
                    string line;
                    while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                    {
                        try
                        {

                            Process[] proc = Process.GetProcessesByName(line);
                            foreach (Process p in proc) p.Kill();
                        }
                        catch { }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
