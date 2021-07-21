using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Detyra2_TCPclient
{
    public partial class Form2 : Form
    {

        string ConnectionString = "Server=localhost;Database=sigdb;Uid=root;Pwd=;";

        public Form2()
        {
            InitializeComponent();
        }

        private void txtEmri_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMbiemri_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtlloji_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtVlera_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAdresa_TextChanged(object sender, EventArgs e)
        {

        }

        private void ndryshoButton_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);

            try
            {

                connection.Open();


                string strCommand = "UPDATE faturat SET lloji ='" + txtlloji.Text + "' , data=" + txtData.Text + ",vlera=" + txtVlera.Text + ",adresa ='" + txtAdresa.Text + "' WHERE id IN( SELECT id FROM users WHERE name = '" + txtEmri.Text + "' and surname = '" + txtMbiemri.Text + "')";

                MySqlCommand sqlCommand = new MySqlCommand(strCommand, connection);
                int retValue = sqlCommand.ExecuteNonQuery();
                
                if (retValue > 0)
                {
                    MessageBox.Show("Te dhenat u ruajten me sukses!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Ruajtja deshtoi!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lidhja deshtoi!\n " + ex.Message);
            }
        }
    }
}
