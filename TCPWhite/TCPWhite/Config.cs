using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TCPWhite
{
    public static class Config
    {
        public static Dictionary<string, string> data = new Dictionary<string, string>(100);
        public static bool error = false;

        public static void LoadConfig()
        {
            try
            {
                error = false;
                using (XmlReader xr = XmlReader.Create(@"config.xml"))
                {
                    string element = "";
                    while (xr.Read())
                    {
                        if (xr.NodeType == XmlNodeType.Element) element = xr.Name;
                        else if (xr.NodeType == XmlNodeType.Text)
                        {
                            data.Add(element, xr.Value);
                        }
                    }
                }
            }
            catch
            {
                error = true;
                CreateConfig();
            }
        }

        static void CreateConfig()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(@"config.xml", settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("Config");
                xw.WriteStartElement("Server");
                xw.WriteStartElement("IPAddress");
                xw.WriteValue("127.0.0.1");
                xw.WriteEndElement();
                xw.WriteStartElement("Port");
                xw.WriteValue("6666");
                xw.WriteEndElement();

                xw.WriteEndElement();
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }

        public static string DebugShowData()
        {
            string debugData = "";
            foreach (string s in data.Keys)
            {
                debugData += s + "|" + data[s] + "\n";
            }
            return debugData;
        }

    }
}

