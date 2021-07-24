using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySqlConnector;


namespace Detyra2_TCPclient
{
    public partial class Form4 : Form
    {

        string ConnectionString = "Server=localhost;Database=sigdb;Uid=root;Pwd=;";
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(ConnectionString);

                connection.Open();

                string strCommand = "Select * from faturat";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(strCommand, connection);

                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                dataGrid.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lidhja deshtoi!!\n " + ex.Message);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 frm = new Form5();
            frm.ShowDialog();
        }
    }
}
