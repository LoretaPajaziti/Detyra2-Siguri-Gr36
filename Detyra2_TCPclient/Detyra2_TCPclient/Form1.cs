using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;
using System.Threading;



namespace Detyra2_TCPclient
{
    public partial class Form1 : Form
    {

        string form = "REGISTER";
        string name = "";
        string surname = "";
        string email = "";
        string password = "";
        string salt = "";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
 
                string Salt = new Random().Next(100000, 1000000).ToString();
                salt = Salt;
                string Password = txtPassword.Text;

                string SaltedPassword = Salt + Password;
                string SaltedHashPassword = CalculateHash(SaltedPassword);
                password = SaltedHashPassword;

                string form = "REGISTER";
                string name = txtName.Text;
                string surname = txtSurname.Text;
                string email = txtEmail.Text;

                string msg = form + "?" + name + "?" + surname + "?" + email + "?" + password + "?" + salt + "?";

                clientConnection.Instance.sendData(msg);
                clientConnection.Instance.readData();

        }

        private string CalculateHash(string SaltedPassword)
        {
            byte[] byteSaltedPassword = Encoding.UTF8.GetBytes(SaltedPassword);
            SHA1CryptoServiceProvider objHash = new SHA1CryptoServiceProvider();
            byte[] byteSaltedHashPassword = objHash.ComputeHash(byteSaltedPassword);

            return Convert.ToBase64String(byteSaltedHashPassword);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
