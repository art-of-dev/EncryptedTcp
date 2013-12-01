using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptedTcp;
namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("127.0.0.1", 10001);
            client.SendText("Hello!");

            //client.Connect();
            //string serverMessage = client.ReceiveText();
            //Console.WriteLine(serverMessage);
            //client.SendText("I'am your client!");
            Console.WriteLine("Ok!");
            Console.ReadKey();
        }
    }
}
