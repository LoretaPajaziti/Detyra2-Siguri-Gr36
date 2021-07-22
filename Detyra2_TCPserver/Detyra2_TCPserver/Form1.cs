using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.IO;

namespace Detyra2_TCPserver
{
    public partial class Form1 : Form
    {

        
        Socket serveri;
        Socket accept;
       
        RSACryptoServiceProvider objRSA = new RSACryptoServiceProvider();
        DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
        byte[] ServerKey;
        byte[] ServerInitialVector;

        Socket socket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public Form1()
        {
            InitializeComponent();

            serveri = socket();
            serveri.Bind(new IPEndPoint(0, 1400));
            serveri.Listen(0);
            MessageBox.Show("Serveri filloi degjimin ne portin 1400");
            

            new Thread(() =>
            {
                accept = serveri.Accept();
                serveri.Close();

                while (true)
                {
                    try
                    {

                        byte[] buffer = new byte[2048];
                        int rec = accept.Receive(buffer, 0, buffer.Length, 0);


                        if (rec <= 0)
                        {
                            throw new SocketException();
                        }
                   
                        Array.Resize(ref buffer, rec);

                        string data = Encoding.Default.GetString(buffer);

                        data = decrypt(data);
                        MessageBox.Show(data);

                        string[] list = data.Split('.');
                        string controlFlow = list[list.Length - 1].Substring(0, 1);

                        send();

                    }
                    catch
                    {
                        MessageBox.Show("Connection lost");
                        Application.Exit();
                    }
                }
            }).Start();
        
    }

        private void send()
        {

            string msg;
            msg = encrypt("Pershendetje");
            MessageBox.Show(msg);
            byte[] data = Encoding.Default.GetBytes(msg);
            accept.Send(data, 0, data.Length, 0);

        }


        //ENKRIPTIMI
        private string encrypt(string plaintext)
        {

            DESCryptoServiceProvider ServerDes = new DESCryptoServiceProvider();

            ServerDes.GenerateIV();

            ServerDes.Key = ServerKey;
            byte[] iv = ServerDes.IV;  

            ServerDes.Padding = PaddingMode.PKCS7;
            ServerDes.Mode = CipherMode.CBC;

            byte[] bytePlaintexti = Encoding.UTF8.GetBytes(plaintext);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ServerDes.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(bytePlaintexti, 0, bytePlaintexti.Length);
            cs.Close();

            byte[] byteCiphertexti = ms.ToArray();

            string answer_iv = Convert.ToBase64String(iv);
            string answer_key = Convert.ToBase64String(ServerKey);
            string answer_ciphertext = Convert.ToBase64String(byteCiphertexti);

            return answer_iv + "." + answer_key + "." + answer_ciphertext;

        }


        //DEKRIPTIMI
        private string decrypt(string ciphertext)
        {
            string[] info = ciphertext.Split('.');
            ServerKey = Convert.FromBase64String(info[1]);
            ServerInitialVector = Convert.FromBase64String(info[0]);

            objDES.Key = ServerKey;
            objDES.IV = ServerInitialVector;

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



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

  
}
