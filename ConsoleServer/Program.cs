using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptedTcp;
using System.Net.Sockets;
using EncryptedTcp.Processing;
using System.Threading;
namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            MyServer srv = new MyServer();
            srv.Port = 10001;
            srv.Start();
        }
    }
    class MyServer:Server
    {
        public override void ClientThread(object clientSocket)
        {
            Socket client = (Socket)clientSocket;
            ServerProcessingEncrypted clientProc = new ServerProcessingEncrypted(client);
            string text = clientProc.ReceiveText();
            Console.WriteLine("Client: {0}", text);
        }
    }
}
