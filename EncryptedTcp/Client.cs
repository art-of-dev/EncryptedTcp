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
    public class Client
    {
        private string IP { get; set; }
        private int Port { get; set; }
        private ClientProcessingEncrypted _proc;

        private Client() { }
        public Client(string ip, int port)
        {
            _proc=new ClientProcessingEncrypted(ip,port);
        }

        public void Send(byte[] data)
        {
            _proc.Send(data);
        }

        public void SendText(string text)
        {
            _proc.SendText(text);
        }

        public byte[] Receive()
        {
            return _proc.Receive();
        }

        public string ReceiveText()
        {
            return _proc.ReceiveText();
        }
    }
}
