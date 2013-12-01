using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedTcp.Processing
{
    class Aes256
    {
        public byte[] Key { get; set; }
        private byte[] IV { get; set; }
        private RijndaelManaged aes;


        public Aes256(bool GenerateKeys = true)
        {
            aes = new RijndaelManaged();
            {
                aes.KeySize = 256;
                aes.IV = new byte[16];
                if (GenerateKeys)
                {
                    aes.GenerateKey();
                    Key = aes.Key;
                }
                IV = aes.IV;
            }
        }


        public byte[] Encrypt(byte[] data)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            sa.KeySize = 256;
            sa.BlockSize = 128;
            ICryptoTransform ct = sa.CreateEncryptor(Key, IV);
            byte[] encryptedData;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                }
                encryptedData = ms.ToArray();
            }
            return encryptedData;
        }


        public string EncryptString(string data)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(data)));
        }


        public byte[] Decrypt(byte[] data)
        {
            BinaryReader br = new BinaryReader(InternalDecrypt(data));
            return br.ReadBytes(data.Length);
        }

        public string DecryptString(string data)
        {
            CryptoStream cs = InternalDecrypt(Convert.FromBase64String(data));
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }


        CryptoStream InternalDecrypt(byte[] data)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            sa.KeySize = 256;
            sa.BlockSize = 128;
            ICryptoTransform ct = sa.CreateDecryptor(Key, IV);
            MemoryStream ms = new MemoryStream(data);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }
    }
}