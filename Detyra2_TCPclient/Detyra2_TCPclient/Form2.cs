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
    public partial class Form2 : Form
    {

        string form = "UPDATE";
        string emri = "";
        string mbiemri = "";
        string llojiFatures = "";
        string vlera = "";
        string Data = "";
        string adresa = "";



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
             emri = txtEmri.Text;
             mbiemri = txtMbiemri.Text;
             llojiFatures = txtlloji.Text;
             vlera = txtVlera.Text;
             Data = txtData.Text;
             adresa = txtAdresa.Text;

            string msg = form + "?" + emri + "?" + mbiemri + "?" + llojiFatures + "?" + vlera + "?" + Data + "?" + adresa + "?";

            clientConnection.Instance.sendData(msg);
            clientConnection.Instance.readData();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

    }
}
