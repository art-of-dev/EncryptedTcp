EncryptedTcp
============

This is my first work. I'm learning C# for a few weeks and I'm want to share my work for everyone.
This is VS2013 solution contains the library with examples how to use it.

This library makes secure tcp-connection between server and any number of clients.

You need download sources to your PC, compile it and add as reference to your soloution to use it.

How to use:
1) Include it in your server-part and override the Server class when you define
what you will do with your clients.
2) Include it in your client-part and just call any methods, which available for Client class to Recieve and Send any data
like bytes or plain text

How it works:
1) Server is running for listening all incoming connections on defined port.
2) Client connect to server
3) Server places client to ThreadPool and waiting for a new clients.
4) In ThreadPool client places in Callback, which you can override, but tou must create instance of the class ServerProcessingEncrypted
which includes methods for interaction with client.
5) When you create ServerProcessingEncrypted instance client will create RSA2048 key pair and sends public key to server
6) Server recieved client's RSA2048 public key, generates AES256 key, encrypt in by using client's RSA public key and send it back to client.
7) Client decrypts AES256 key by using private key.
8) After that any data tdansfer will be encrypted by using AES256.

P.S. - sorry for my worst English and worst code) I want to make some good things for poeple like me. I will be happy if I will helped to someone.
