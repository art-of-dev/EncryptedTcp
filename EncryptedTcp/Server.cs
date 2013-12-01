using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using EncryptedTcp.Processing;
namespace EncryptedTcp
{
    public class Server
    {
        Socket _listener;
        //private TcpListener _listener;//Класс прослушки Tcp
        private int _maxClients;//Максимальное количество клиентов

        private bool StopFlag { get; set; }
        public int MaxClients { get; set; }
        public int Port { get; set; }

        public void Start()
        {
            if (MaxClients < 2)
                MaxClients = 16;
            if (Port == 0)
                Port = 10000;
            ThreadPool.SetMaxThreads(MaxClients, MaxClients);
            ThreadPool.SetMinThreads(MaxClients, MaxClients);
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(GetLocalIP(), Port));
            _listener.Listen(MaxClients);
            while (!StopFlag)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), _listener.Accept());
            }
        }

        private IPAddress GetLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            }
            return IPAddress.Any;
        }

        public virtual void ClientThread(Object clientSocket)
        {
            Socket client = (Socket)clientSocket;
            ServerProcessingEncrypted clientProc = new ServerProcessingEncrypted(client);
            
        }
    }
}
