using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
namespace EncryptedTcp.Processing
{
    class ClientProcessingEncrypted
    {
        private ClientProcessing _proc;
        private Aes256 _aes256;

        private ClientProcessingEncrypted() { }
        public ClientProcessingEncrypted(string ip, int port)
        {
            _aes256 = new Aes256(false);
            _proc = new ClientProcessing(ip, port);
            _proc.Connect();
            ClientSideAuth();
        }

        public void Send(byte[] data)
        {
            byte[] encryptedData = _aes256.Encrypt(data);
            _proc.Send(encryptedData);
        }

        public void SendText(string text)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            Send(textBytes);
        }

        public byte[] Receive()
        {
            byte[] encryptedData = _proc.Receive();
            return _aes256.Decrypt(encryptedData);
        }

        public string ReceiveText()
        {
            byte[] decryptedBytes = Receive();
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        private void ClientSideAuth()
        {
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
            string openClientRsaKey = csp.ToXmlString(false);
            _proc.SendText(openClientRsaKey);
            byte[] encryptedServersAesKey = _proc.Receive();
            _aes256.Key = csp.Decrypt(encryptedServersAesKey, false);
        }
    }
}
