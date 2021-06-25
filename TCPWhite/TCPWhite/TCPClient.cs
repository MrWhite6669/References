using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Xml.Serialization;

namespace TCPWhite
{
    public class TCPClient
    {
        TcpClient c;
        NetworkStream stream;
        List<Message> queue = new List<Message>();
        public int maxLength = 100000000;

        public TCPClient(string ip, int port)
        {
            c = new TcpClient(ip, port);
            stream = c.GetStream();
            GetCallBackHeadder();
        }

        public async void SendMessage(Message message)
        {
            byte[] msg = message.Serialize();
            byte[] lenght = Converter.IntToByteArray(msg.Length);
            byte[] buffer = new byte[lenght.Length + msg.Length + 1];

            lenght.CopyTo(buffer,0);
            msg.CopyTo(buffer, 5);

            try
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch
            {
                c.Close();
                stream.Close();
                return;
            }
        }

        public bool IsConnected()
        {
            return c.Connected;
        }

        private async void GetCallBackHeadder()
        {
            byte[] headder = new byte[5];
            int bytesRead = 0;
            if (c.Connected)
            {
                while (bytesRead < 5)
                {
                    try
                    {
                        bytesRead += await stream.ReadAsync(headder, bytesRead, headder.Length - bytesRead);
                    }
                    catch
                    {
                        return;
                    }
                }
            }
            else
            {
                c.Close();
                stream.Close();
                return;
            }
            GetCallBack(Converter.ByteArrayToInt(headder));
        }

        private async void GetCallBack(int lenght)
        {
            if (c.Connected)
            {
                byte[] buffer = new byte[lenght];
                int bytesRead = 0;

                while (bytesRead < lenght)
                {
                    try
                    {
                        bytesRead += await stream.ReadAsync(buffer, bytesRead, buffer.Length - bytesRead);
                    }
                    catch
                    {
                        return;
                    }
                }

                //Message msg = new Message(Converter.BytesToString(CleanBuffer(buffer)));
                XmlSerializer xs = new XmlSerializer(typeof(Message));
                using (MemoryStream m = new MemoryStream(buffer))
                {
                    Message msg = (Message)xs.Deserialize(m);
                    msg.DeserializeData();
                    AddToQueue(msg);
                }
            }
            else
            {
                c.Close();
                stream.Close();
                return;
            }
            GetCallBackHeadder();

        }

        private void AddToQueue(Message msg)
        {
            queue.Add(msg);
        }

        public Message GetMessageFromQueue()
        {
            Message temp;
            if ((temp = queue.Last()) != null)
            {
                queue.Remove(temp);
                return temp;
            }
            return null;
        }

        public void Close()
        {
            c.Close();
        }

    }
}
