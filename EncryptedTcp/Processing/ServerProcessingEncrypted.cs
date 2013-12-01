using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;
namespace EncryptedTcp.Processing
{
    public class ServerProcessingEncrypted
    {
        private ServerProcessingEncrypted() { }
        private ServerProcessing _proc;
        private Aes256 _aes256;

        public ServerProcessingEncrypted(TcpClient client)
        {
            _proc = new ServerProcessing(client);
            _aes256 = new Aes256();
            ServerSideAuth();
        }

        public void Send(byte[]data)
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

        private void ServerSideAuth()
        {
            string clientRsaOpenKey = _proc.ReceiveText();
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
            csp.FromXmlString(clientRsaOpenKey);
            byte[] encryptedAesKey = csp.Encrypt(_aes256.Key, false);
            csp.Dispose();
            _proc.Send(encryptedAesKey);
        }
        
    }
}
