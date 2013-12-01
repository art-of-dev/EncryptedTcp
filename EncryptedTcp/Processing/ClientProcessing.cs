using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
namespace EncryptedTcp.Processing
{
    public class ClientProcessing
    {
        private Socket _connectionFromServerForReceive;
        private Socket _connectionToServerForSend;

        private string IP { get; set; }
        private int Port { get; set; }

        public ClientProcessing() { }
        public ClientProcessing(string ip, int port)
        {
            IP = ip;
            Port = port;
        }


        public bool Connect()
        {
            _connectionToServerForSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _connectionToServerForSend.Connect(IP, Port);
            IPEndPoint localIpEndPoint = _connectionToServerForSend.LocalEndPoint as IPEndPoint;
            Socket _listenerForServerConnectionForReceive = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listenerForServerConnectionForReceive.Bind(localIpEndPoint);
            _listenerForServerConnectionForReceive.Listen(2);
            _connectionFromServerForReceive=_listenerForServerConnectionForReceive.Accept();
            return true;
        }


        public void Send(byte[] data)
        {
            while (_connectionToServerForSend.Available > 0)
                Thread.Sleep(10);
            _connectionToServerForSend.Send(data);
        }


        public void SendText(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            Send(data);
        }


        public byte[] Receive()
        {
            while (_connectionFromServerForReceive.Available == 0)
            {
                Thread.Sleep(100);
            }

            using (MemoryStream result = new MemoryStream())
            {
                byte[] buffer = new byte[8192];
                while (_connectionFromServerForReceive.Available > 0)
                {
                    int readedData = _connectionFromServerForReceive.Receive(buffer);
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
