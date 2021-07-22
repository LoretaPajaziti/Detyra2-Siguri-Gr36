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
            MessageBox.Show("Serveri filloi te degjimin ne portin 1400");
            

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
                        MessageBox.Show(data);
                    
                        // data = decrypt(data);

                        string[] list = data.Split('.');

                        string controlFlow = list[list.Length - 1].Substring(0, 1);

             
                    }
                    catch
                    {
                        MessageBox.Show("Connection lost");
                        Application.Exit();
                    }
                }
            }).Start();
        
    }

      

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

  
}
