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
       
        Socket klienti;

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

           // msg = encrypt(msg);
            byte[] data = Encoding.Default.GetBytes(msg);
            klienti.Send(data, 0, data.Length, 0);
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


