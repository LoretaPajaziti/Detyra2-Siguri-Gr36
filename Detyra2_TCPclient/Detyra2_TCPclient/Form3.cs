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


namespace Detyra2_TCPclient
{
    public partial class Form3 : Form
    {

        public Form3()
        {
            InitializeComponent();
            
        }

        public static Socket server;
        static string receivedData;
        private bool Connected;
       
        private void SendDataToServer(string data)
        {
            server.Send(Encoding.ASCII.GetBytes(data));
        }

        private string ReceiveDataFromServer()
        {
            byte[] data = new byte[512];
            int recv_data = server.Receive(data);
            string stringData = Encoding.ASCII.GetString(data, 0, recv_data);
            receivedData = stringData;
            return stringData;
        }
        private void SendRequestToSrv(string teksti)
        {
            try
            {
                server.Send(Encoding.ASCII.GetBytes(teksti));
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message.ToString());
            }
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
            
            byte[] data = new byte[512];

            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            IPEndPoint ipep = new IPEndPoint(localAddr, port);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ipep);
                Connected = true;
                MessageBox.Show("Jeni lidhur me serverin!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(SocketException ex)
            {
                MessageBox.Show("Lidhja me Server ka deshtuar!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Connected = false;
                return;
            }

            if (Connected)
            {
                Form2 frm = new Form2();
                frm.ShowDialog();
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
    }
}
