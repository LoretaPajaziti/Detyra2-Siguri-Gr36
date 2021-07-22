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
    public partial class Form3 : Form
    {

        RSACryptoServiceProvider objRSA = new RSACryptoServiceProvider();
        DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
        Socket klienti;
        byte[] ClientKey;
        byte[] ClientInitialVector;

        Socket socket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public Form3()
        {
            InitializeComponent();
            klienti = socket();
            connect();

        }
        private void connect()
        {
            string ipaddress = "127.0.0.1";
            int portNumber = 1400;

            try
            {
               klienti.Connect(new IPEndPoint(IPAddress.Parse(ipaddress), portNumber));

                new Thread(() =>
                {
                    read();
                }).Start();
            }
            catch
            {
                MessageBox.Show("Lidhja deshtoi");
            }
        }

        void read()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[2048];
                    int rec = klienti.Receive(buffer, 0, buffer.Length, 0);

                    MessageBox.Show("Klient: " + Encoding.Default.GetString(buffer));

                    if (rec <= 0)
                    {
                        throw new SocketException();
                    }

                    Array.Resize(ref buffer, rec);

                    string mesazhi = Encoding.Default.GetString(buffer);
                    mesazhi = decrypt(mesazhi);

                    MessageBox.Show(mesazhi);

                    string[] list = mesazhi.Split('.');
                    string controlFlow = list[list.Length - 1].Substring(0, 1);

                }
                catch
                {
                    MessageBox.Show("Disconnected");
                    Application.Exit();
                }
            }
        }

        private void send()
        {
            string username = textEmail.Text;
            string password = textPass.Text;
            string login = "1";

            string msg = username + "." + password + "." + login;

            msg = encrypt(msg);
            byte[] data = Encoding.Default.GetBytes(msg);
            klienti.Send(data, 0, data.Length, 0);
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

            byte[] bytePlaintext = Encoding.UTF8.GetBytes(plaintext);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, objDES.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(bytePlaintext, 0, bytePlaintext.Length);
            cs.Close();

            byte[] byteCiphertext = ms.ToArray();

            string iv = Convert.ToBase64String(ClientInitialVector);
            string key = Convert.ToBase64String(ClientKey);
            string ciphertxt = Convert.ToBase64String(byteCiphertext);

            return iv + "." + key + "." + ciphertxt;


        }

        //DEKRIPTIMI
     private string decrypt(string ciphertext)
        {
           

            string[] info = ciphertext.Split('.');
            MessageBox.Show(info.Length.ToString());

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

   




        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            send();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



    }
}


