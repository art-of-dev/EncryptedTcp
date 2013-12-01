using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptedTcp;
using System.Net;
using System.Net.Sockets;
namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client(GetLocalIP().ToString(), 10001);
            client.SendText("Hello!");
            Console.WriteLine("Ok!");
            Console.ReadKey();
        }

        private static IPAddress GetLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            }
            return IPAddress.Any;
        }
    }
}
