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
        private Socket _connectionToServerForReceive;
        private Socket _connectionFromServerForSend;

        private ServerProcessing() { }
        public ServerProcessing(Socket client)
        {
            _connectionToServerForReceive = client;
            IPEndPoint _clientEndPoint = client.RemoteEndPoint as IPEndPoint;
            _connectionFromServerForSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _connectionFromServerForSend.Connect(_clientEndPoint);
        }

        public void Send(byte[] data)
        {
            while (_connectionFromServerForSend.Available > 0)
                Thread.Sleep(10);
            _connectionFromServerForSend.Send(data);
        }

        public void SendText(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            Send(data);
        }

        public byte[] Receive()
        {
            while (_connectionToServerForReceive.Available == 0)
            {
                Thread.Sleep(100);
            }

            using (MemoryStream result = new MemoryStream())
            {
                byte[] buffer = new byte[8192];
                while (_connectionToServerForReceive.Available>0)
                {
                    int readedData = _connectionToServerForReceive.Receive(buffer);
                    result.Write(buffer, 0, readedData);
                }
                return result.ToArray();
            }
        }

        public string ReceiveText()
        {
            byte[] textBytes = Receive();
            return Encoding.UTF8.GetString(textBytes);
        }
    }
}
