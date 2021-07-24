using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.IO;
using JWT.Algorithms;
using JWT;
using JWT.Serializers;

namespace Detyra2_TCPclient
{

    public class clientConnection
    {
        Socket client;
        X509Certificate2 certifikata = new X509Certificate2();
        RSACryptoServiceProvider objRSA = new RSACryptoServiceProvider();
        DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
        byte[] ClientKey;
        byte[] ClientInitialVector;

        private static clientConnection instance;
        public clientConnection()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string ipaddress = "127.0.0.1";
            int portNumber = 250;
            client.Connect(new IPEndPoint(IPAddress.Parse(ipaddress), portNumber));

        }

        public static System.Security.Cryptography.RSAEncryptionPadding Pkcs1 { get; }


        public static clientConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new clientConnection();
                }
                return instance;
            }
        }

        //ENKRIPTIMI
        private string encrypt(string plaintext)
        {
            objDES.GenerateKey();
            objDES.GenerateIV();
            ClientKey = objDES.Key;
            ClientInitialVector = objDES.IV;

            objDES.Mode = CipherMode.CBC;
            objDES.Padding = PaddingMode.PKCS7;

            var cert = new X509Certificate2(System.IO.File.ReadAllBytes("C:\\Users\\lumdu\\Desktop\\server.crt"));
            RSA rsa = (RSA)cert.PublicKey.Key;
            byte[] byteKey = rsa.Encrypt(ClientKey, RSAEncryptionPadding.Pkcs1);

            byte[] bytePlaintext = Encoding.UTF8.GetBytes(plaintext);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, objDES.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(bytePlaintext, 0, bytePlaintext.Length);
            cs.Close();

            byte[] byteCiphertext = ms.ToArray();

            string iv = Convert.ToBase64String(ClientInitialVector);
            string key = Convert.ToBase64String(byteKey);
            string ciphertxt = Convert.ToBase64String(byteCiphertext);

            return iv + "." + key + "." + ciphertxt;

        }

        //DEKRIPTIMI
        private string decrypt(string ciphertext)
        {

            string[] info = ciphertext.Split('.');

            ClientKey = Convert.FromBase64String(info[1]);
            ClientInitialVector = Convert.FromBase64String(info[0]);

            objDES.Key = ClientKey;
            objDES.IV = ClientInitialVector;

            objDES.Padding = PaddingMode.PKCS7;
            objDES.Mode = CipherMode.CBC;

            byte[] byteCiphertexti = Convert.FromBase64String(info[2]);
            MemoryStream ms = new MemoryStream(byteCiphertexti);
            CryptoStream cs = new CryptoStream(ms, objDES.CreateDecryptor(), CryptoStreamMode.Read);

            byte[] byteTextiDekriptuar = new byte[ms.Length];
            cs.Read(byteTextiDekriptuar, 0, byteTextiDekriptuar.Length);
            cs.Close();

            string decryptedText = Encoding.UTF8.GetString(byteTextiDekriptuar);
            return decryptedText;

        }


        public void sendData(String msg)
        {
            byte[] data = Encoding.Default.GetBytes(encrypt(msg));

            client.Send(data, 0, data.Length, 0);
        }
        public void readData()
        {
            byte[] buffer = new byte[2048];
            int rec = client.Receive(buffer, 0, buffer.Length, 0);
            if (rec > 0)
            {
                try
                {
                    Array.Resize(ref buffer, rec);
                    if (buffer.Length > 900)
                    {
                        certifikata.Import(buffer);
                    }
                    String data = Encoding.Default.GetString((buffer));
                    data = decrypt(data);
                    String[] response = data.Split('?');

                    string token = "";

                    MessageBox.Show(response[0]);

                    if (response[0].Equals("Login completed"))
                    {
                        token = response[1];
                        IJwtAlgorithm algorithm = new JWT.Algorithms.HMACSHA256Algorithm();
                        IJsonSerializer serializer = new JsonNetSerializer();
                        IBase64UrlEncoder base64UrlEncoder = new JwtBase64UrlEncoder();
                        IJwtValidator jwtValidator = new JwtValidator(serializer, new UtcDateTimeProvider());
                        IJwtDecoder jwtDecoder = new JwtDecoder(serializer, jwtValidator, base64UrlEncoder, algorithm);

                        const string secret = "As8DhbTY6Hb6jhFJK76GH76hvg&";

                        var json = jwtDecoder.Decode(token, secret, true);

                        MessageBox.Show(json);

                        Form4 frm = new Form4();
                        frm.ShowDialog();
                    }
                    else if (response[0].Equals("Registering completed"))
                    {
                        Form3 frm = new Form3();
                        frm.ShowDialog();
                    }
                }
                catch
                {
                    MessageBox.Show("Disconnected");
                    Application.Exit();
                }
            }
        }
    }
}
