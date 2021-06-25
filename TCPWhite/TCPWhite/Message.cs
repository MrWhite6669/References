using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TCPWhite
{
    [Serializable]
    public class Message
    {

        public int id;

        [XmlIgnore]
        public Dictionary<string, string> data = new Dictionary<string, string>();

        public List<string> keys = new List<string>();
        public List<string> values = new List<string>();

        public byte[] byteData;
        public bool inBytes = false;
        public string from;

        [XmlIgnore]
        public Client sender;


        public Message(int id, string data, string from)
        {
            this.id = id;
            this.data.Add("text", data);
            this.from = from;
        }

        public Message(int id)
        {
            this.id = id;
        }

        public Message()
        {
            
        }

        public void DeserializeData()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if(!data.ContainsKey(keys[i]))data.Add(keys[i], values[i]);
            }
        }

        public void SerializeData()
        {
            foreach(string s in data.Keys)
            {
                keys.Add(s);
                values.Add(data[s]);
            }
            data.Clear();
        }

        public void AddSender(Client c)
        {
            sender = c;
        }
        
        public string Code()
        {
            return string.Join(";", id, data);
        }


        XmlSerializer xs = new XmlSerializer(typeof(Message));

        public byte[] Serialize()
        {
            using(MemoryStream m = new MemoryStream())
            {
                SerializeData();
                xs.Serialize(m, this);
                return m.ToArray();
            }
        }

        public override string ToString()
        {
            string sd = "";
            foreach (string s in data.Keys)
            {
                sd += string.Format("{0} = {1}\n", s, data[s]);
            }
            return string.Format("Message: \n ID: {0} Sender:{1} \n Data:\n{2}", id, sender.ip, sd);
        }
    }
}
