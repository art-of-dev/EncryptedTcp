using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptedTcp;
using System.Net.Sockets;
using EncryptedTcp.Processing;
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
            TcpClient client = (TcpClient)clientSocket;
            ServerProcessingEncrypted clientProc = new ServerProcessingEncrypted(client);
            Console.WriteLine("Client: {0}", clientProc.ReceiveText());
        }
    }
}
