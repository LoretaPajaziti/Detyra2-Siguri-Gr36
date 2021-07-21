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

namespace Detyra2_TCPserver
{
    public partial class Form1 : Form
    {
        TcpListener server;
        private void ClientConnection(object obj)
        {
            TcpClient client = (TcpClient)obj;
            MessageBox.Show("Client connected with IP " + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
        }
        public void StartListening() 
        {
           
            try
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(ClientConnection, client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }


        public Form1()
        {
            InitializeComponent();

            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            server = new TcpListener(localAddr, port);
            server.Start();
            Thread thread = new Thread(new ThreadStart(StartListening));
            thread.IsBackground = true;
            thread.Start();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

  
}
