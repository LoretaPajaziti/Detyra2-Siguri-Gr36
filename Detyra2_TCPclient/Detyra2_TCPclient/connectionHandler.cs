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

namespace Detyra2_TCPclient
{

    public class clientConnection
    {
        Socket client;

           string Certificate = "C:\\Users\\loret\\Desktop\\CertifikataX509.pfx"; /*  
              static X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(Certificate), "12345678", X509KeyStorageFlags.MachineKeySet);
              public static byte[] celesipublik = cert.GetPublicKey();*/

       






        //string result = System.Convert.ToBase64String(celesipublik)    
        // return cert.GetPublicKey();



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

            // X509Certificate2 cert = new X509Certificate2("C:\\Users\\loret\\Desktop\\server.crt");
            // cert.Import("C:\\Users\\loret\\Desktop\\server.crt");

         //  var myDocuments = Environment.GetEnvironmentVariable(Environment.SpecialFolder.MyDocuments);

         //  var keyPath = Path.Combine(myDocuments, "keys", "server.pem");

            var cert = new X509Certificate2(System.IO.File.ReadAllBytes("C:\\Users\\loret\\Desktop\\server.crt"));

            RSA rsa = (RSA)cert.PublicKey.Key;
        

            byte[] byteKey = rsa.Encrypt(ClientKey, RSAEncryptionPadding.Pkcs1);
            //string encryptedkey = Convert.ToBase64String(byteKey);

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
            /*byte[] data = Encoding.Default.GetBytes(msg);*/




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
                    MessageBox.Show(data);

                    if (data.Equals("Login completed"))
                    {
                        Form2 frm = new Form2();
                        frm.ShowDialog();
                    }
                    else if (data.Equals("Registering completed"))
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
