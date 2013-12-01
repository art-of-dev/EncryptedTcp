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
        private TcpClient _connectionFromServerForReceive;
        private TcpClient _connectionToServerForSend;
        private TcpListener _listenerForServerConnectionForReceive;

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
            _connectionToServerForSend = new TcpClient();
            _connectionToServerForSend.Connect(IP, Port);
            IPEndPoint localIpEndPoint = _connectionToServerForSend.Client.LocalEndPoint as IPEndPoint;
            _listenerForServerConnectionForReceive = new TcpListener(localIpEndPoint);
            _listenerForServerConnectionForReceive.Start(1);
            _connectionFromServerForReceive = _listenerForServerConnectionForReceive.AcceptTcpClient();
            return true;
        }


        public void Send(byte[] data)
        {
            NetworkStream networkStream = _connectionToServerForSend.GetStream();
            networkStream.Write(data, 0, data.Length);
        }


        public void SendText(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            Send(data);
        }


        public byte[] Receive()
        {
            NetworkStream networkStream = _connectionFromServerForReceive.GetStream();
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
