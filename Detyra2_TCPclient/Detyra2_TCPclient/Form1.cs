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

        string ConnectionString = "Server=localhost;Database=sigdb;Uid=root;Pwd=;";
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
            MySqlConnection connection = new MySqlConnection(ConnectionString);

            try
            {
                connection.Open();

                string Salt = new Random().Next(100000, 1000000).ToString();
                string Password = txtPassword.Text;

                string SaltedPassword = Salt + Password;
                string SaltedHashPassword = CalculateHash(SaltedPassword);

                string strCommand = "insert into users(name, surname, email, password, salt) values (" +
                    "'" + txtName.Text + "','" + txtSurname.Text + "','" + txtEmail.Text + "','" + SaltedHashPassword + "','" + Salt + "')";

                MySqlCommand sqlCommand = new MySqlCommand(strCommand, connection);
                int retValue = sqlCommand.ExecuteNonQuery();

                if(retValue > 0)
                {
                    MessageBox.Show("Te dhenat u ruajten me sukses!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Ruajtja deshtoi!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Lidhja deshtoi!\n " + ex.Message);
            }

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
