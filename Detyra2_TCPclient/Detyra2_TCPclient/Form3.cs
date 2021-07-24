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
        public Form3()
        {
            InitializeComponent();
        }
      
       
        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string form = "LOGIN";
            string email = textEmail.Text;
            string password = textPass.Text;

            string msg = form + "?" + email + "?" + password + "?";

            clientConnection.Instance.sendData(msg);
            clientConnection.Instance.readData();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



    }
}


