using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
namespace EncryptedTcp
{
    class ServerProcessing
    {
        private TcpClient _connectionToServerForReceive;
        private TcpClient _connectionFromServerForSend;

        private ServerProcessing() { }
        public ServerProcessing (TcpClient client)
        {
            _connectionToServerForReceive = client;
            IPEndPoint _clientEndPoint = _connectionToServerForReceive.Client.RemoteEndPoint as IPEndPoint;
            _connectionFromServerForSend = new TcpClient();
            _connectionFromServerForSend.Connect(_clientEndPoint);
        }

        public void Send(byte[] data)
        {
            NetworkStream networkStream = _connectionFromServerForSend.GetStream();
            networkStream.Write(data, 0, data.Length);
        }

        public void SendText(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            Send(data);
        }

        public byte[] Receive()
        {
            NetworkStream networkStream = _connectionToServerForReceive.GetStream();
            if (networkStream.CanRead)
            {
                while (!networkStream.DataAvailable)
                {
                    Thread.Sleep(100);
                }
                using (MemoryStream result = new MemoryStream())
                {
                    byte[] buffer = new byte[8192];
                    while (networkStream.DataAvailable)
                    {
                        int readedData = networkStream.Read(buffer, 0, buffer.Length);
                        result.Write(buffer, 0, readedData);
                    }
                    return result.ToArray();
                }
            }
            return new byte[] { };
        }

        public string ReceiveText()
        {
            byte[] textBytes = Receive();
            return Encoding.UTF8.GetString(textBytes);
        }
    }
}
