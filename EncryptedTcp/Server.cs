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
        private TcpListener _listener;//Класс прослушки Tcp
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
            _listener = new TcpListener(IPAddress.Any, Port);
            _listener.Start();
            while (!StopFlag)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), _listener.AcceptTcpClient());
            }
        }

        public virtual void ClientThread(Object clientSocket)
        {
            TcpClient client = (TcpClient)clientSocket;
            ServerProcessingEncrypted clientProc = new ServerProcessingEncrypted(client);
            
        }
    }
}
