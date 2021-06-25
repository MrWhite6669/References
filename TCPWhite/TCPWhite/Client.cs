using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TCPWhite
{
    public class Client
    {
        public TcpClient connection;
        public NetworkStream stream;
        public string ip;

        public Client(TcpClient connection)
        {
            this.connection = connection;
            stream = connection.GetStream();
            ip = ((IPEndPoint)connection.Client.RemoteEndPoint).Address.ToString();

            WaitForHeadder();
        }

        private async void WaitForHeadder()
        {
            byte[] headder = new byte[5];
            int bytesRead = 0;
            if (connection.Connected)
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
                Disconnect();
            }
            WaitForData(Converter.ByteArrayToInt(headder));
        }

        private async void WaitForData(int lenght)
        {
            if (connection.Connected)
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
                

                XmlSerializer xs = new XmlSerializer(typeof(Message));
                using(MemoryStream m = new MemoryStream(buffer))
                {
                    Message msg = (Message)xs.Deserialize(m);
                    msg.AddSender(this);
                    msg.DeserializeData();
                    TCPServer.AddToQueue(msg);
                }
            }
            else Disconnect();
            WaitForHeadder();

        }

        public async void SendCallback(Message message)
        {
            byte[] msg = message.Serialize();
            byte[] lenght = Converter.IntToByteArray(msg.Length);
            byte[] buffer = new byte[lenght.Length + msg.Length + 1];

            lenght.CopyTo(buffer, 0);
            msg.CopyTo(buffer, 5);

            try
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            Console.WriteLine("Client {0} disconnected...", ip);
            stream.Close();
            connection.Close();
            TCPServer.clients.Remove(this);
        }

        ~Client()
        {
            Disconnect();
        }

    }
}
