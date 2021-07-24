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
using MySqlConnector;
using System.Xml;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace Detyra2_TCPserver
{
    public partial class Form1 : Form
    {

        Socket serveri;
        Socket accept;
        string ConnectionString = "Server=localhost;Database=sigdb;Uid=root;Pwd=;";
        RSACryptoServiceProvider objRSA = new RSACryptoServiceProvider();
        DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
        X509Certificate2 certifikata = new X509Certificate2();
        XmlDocument objXml = new XmlDocument();
        byte[] ClientKey;
        byte[] ClientInitialVector;


        Socket socket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }


        public Form1()
        {
            InitializeComponent();
        }

        public void handleClient(Socket accept)
        {
            while (true)
            {
                byte[] buffer = new byte[2048];
                int rec = accept.Receive(buffer, 0, buffer.Length, 0);

                try
                {

                    Array.Resize(ref buffer, rec);

                    String data = Encoding.Default.GetString(buffer);
                    data = decrypt(data);

                    String[] list = data.Split('?');

                    String requestToServer = list[0];

                    if (requestToServer.Equals("LOGIN"))
                    {
                        txtReceiver.AppendText("Trying To Login \r\n");
                        try
                        {
                            MySqlConnection connection = new MySqlConnection(ConnectionString);

                            connection.Open();
                            string email = list[1];
                            string password = list[2];
                            string dbPassword = "";
                            string dbSalt = "";
                            string id = "";
                            string name = "";
                            string surname = "";




                            string strCommand = "Select password,salt,id,name,surname from users where email = '" + email + "'";

                            using (MySqlCommand command = new MySqlCommand(strCommand, connection))
                            {
                                using (MySqlDataReader reader = command.ExecuteReader())
                                {
                                    {
                                        while (reader.Read())
                                        {
                                            dbPassword = reader["password"].ToString();
                                            dbSalt = reader["salt"].ToString();
                                            id = reader["id"].ToString();
                                            name = reader["name"].ToString();
                                            surname = reader["surname"].ToString();


                                            string saltedPassword = dbSalt + password;
                                            string hashedSaltedPassword = CalculateHash(saltedPassword);
                                            password = hashedSaltedPassword;

                                        }
                                        if (password.Equals(dbPassword))
                                        {
                                            txtReceiver.AppendText("You have been Logged In" + "\r\n");

                                            string msg = "Login completed?";

                                            var payload = new Dictionary<string, object>
                                            {
                                                {"userid", id },
                                                {"name", name },
                                                {"surname", surname },
                                                {"email", email }
                                            };

                                            IJwtAlgorithm algorithm = new JWT.Algorithms.HMACSHA256Algorithm();
                                            IJsonSerializer serializer = new JsonNetSerializer();
                                            IBase64UrlEncoder base64UrlEncoder = new JwtBase64UrlEncoder();
                                            IJwtEncoder jwtEncoder = new JwtEncoder(algorithm, serializer, base64UrlEncoder);
                                            const string secret = "As8DhbTY6Hb6jhFJK76GH76hvg&";

                                            var token = jwtEncoder.Encode(payload, secret);
                                            msg = msg + token + "?";

                                            byte[] answer = Encoding.Default.GetBytes(encrypt(msg));
                                            accept.Send(answer, 0, answer.Length, 0);
                                        }
                                        else
                                        {
                                            txtReceiver.AppendText("password was not correct" + "\r\n");

                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lidhja deshtoi!\n " + ex.Message);
                        }
                    }


                    else if (requestToServer.Equals("REGISTER"))
                    {
                        txtReceiver.AppendText("Registering\r\n");
                        try
                        {
                            MySqlConnection connection = new MySqlConnection(ConnectionString);

                            connection.Open();
                            String name = list[1];
                            String surname = list[2];
                            String email = list[3];
                            String password = list[4];
                            String salt = list[5];


                            string strCommand = "INSERT INTO users(`name`, `surname`, `email`, `password`, `salt`) VALUES ('" + name + "','" + surname + "','" + email + "','" + password + "','" + salt + "')";
                            using (MySqlCommand command = new MySqlCommand(strCommand, connection))
                            {
                                int executed = command.ExecuteNonQuery();
                                if (executed > 0)
                                {
                                    txtReceiver.AppendText("User registered successully" + "\r\n");
                                    string msg = "Registering completed?";
                                    byte[] answer = Encoding.Default.GetBytes(encrypt(msg));
                                    accept.Send(answer, 0, answer.Length, 0);
                                    /*rec = 0;*/
                                }
                                else
                                {
                                    txtReceiver.AppendText("Couldnt register user" + "\r\n");
                               
                                    /*rec = 0;*/
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lidhja deshtoi!\n " + ex.Message);
                        }

                    }

                    else if (requestToServer.Equals("UPDATE"))
                    {
  
                        txtReceiver.AppendText("Updating\r\n");

                        try
                        {
                            MySqlConnection connection = new MySqlConnection(ConnectionString);

                            connection.Open();
                            String name = list[1];
                            String surname = list[2];
                            String lloji = list[3];
                            String vlera = list[4];
                            String Data = list[5];
                            String adresa = list[6];



                            string strCommand = "UPDATE `faturat` SET `lloji`='" + lloji + "',`data`='" + Data + "',`vlera`='" + vlera + "',`adresa`='" + adresa + "' WHERE id in (SELECT id FROM users WHERE name = '" + name + "' and surname = '" + surname + "')";
                            using (MySqlCommand command = new MySqlCommand(strCommand, connection))
                            {
                                int executed = command.ExecuteNonQuery();
                                if (executed > 0)
                                {

                                    txtReceiver.AppendText("User Data updated successully" + "\r\n");
                                    string msg = "Update completed?";
                                    byte[] answer = Encoding.Default.GetBytes(encrypt(msg));
                                    accept.Send(answer, 0, answer.Length, 0);
                                }
                                else
                                {

                                    txtReceiver.AppendText("Couldnt Update Data" + "\r\n");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lidhja deshtoi!\n " + ex.Message);
                        }

                    }

                    else if (requestToServer.Equals("INSERT"))
                    {
                        txtReceiver.AppendText("Updating\r\n");

                        try
                        {
                            MySqlConnection connection = new MySqlConnection(ConnectionString);

                            connection.Open();
                            String name = list[1];
                            String surname = list[2];
                            String lloji = list[3];
                            String vlera = list[4];
                            String Data = list[5];
                            String adresa = list[6];
   
                            string strCommand = "select id from users where name = '"+name+"' and surname = '"+surname+"'";
                            using (MySqlCommand command = new MySqlCommand(strCommand, connection))
                            {
                                using (MySqlDataReader reader = command.ExecuteReader())
                                {
                                    int executed = 0;
                                    if (reader.Read())
                                    {
                                        string id = reader["id"].ToString();
                                        reader.Close();
                                        command.CommandText = "INSERT INTO `faturat`(`id`, `lloji`, `data`, `vlera`, `adresa`) VALUES ('" + id + "','"+lloji+"','"+Data+"','"+vlera+"','"+adresa+"')";
                                        executed = command.ExecuteNonQuery();
                                    };

                                    if (executed > 0)
                                    {
                                        txtReceiver.AppendText("User data was inserted" + "\r\n");

                                        string msg = "Insert completed?";


                                        byte[] answer = Encoding.Default.GetBytes(encrypt(msg));
                                        accept.Send(answer, 0, answer.Length, 0);
                                    }
                                    txtReceiver.AppendText("Insert failed" + "\r\n");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error inserting\n " + ex.Message);
                        }
                    }

                }
                catch
                {
                    MessageBox.Show("Connection lost");
                    Application.Exit();
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void serverStart_bttn_Click(object sender, EventArgs e)
        {
            serveri = socket();
            string ipaddress = "127.0.0.1";
            int portNumber = 250;
            serveri.Bind(new IPEndPoint(IPAddress.Parse(ipaddress), portNumber));
            serveri.Listen(20);
            txtReceiver.AppendText("Serveri filloi degjimin ne portin 1400\r\n");
            accept = serveri.Accept();
            handleClient(accept);
        }

        //ENKRIPTIMI
        private string encrypt(string plaintext)
        {
            objDES.GenerateKey();
            objDES.GenerateIV();
            ClientKey = objDES.Key;
            ClientInitialVector = objDES.IV;

            objDES.Mode = CipherMode.CBC;
            objDES.Padding = PaddingMode.PKCS7;

            byte[] bytePlaintext = Encoding.UTF8.GetBytes(plaintext);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, objDES.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(bytePlaintext, 0, bytePlaintext.Length);
            cs.Close();

            byte[] byteCiphertext = ms.ToArray();

            string iv = Convert.ToBase64String(ClientInitialVector);
            string key = Convert.ToBase64String(ClientKey);
            string ciphertxt = Convert.ToBase64String(byteCiphertext);

            return iv + "." + key + "." + ciphertxt;


        }

        //DEKRIPTIMI
        private string decrypt(string ciphertext)
        {


            string[] info = ciphertext.Split('.');

            ClientKey = Convert.FromBase64String(info[1]);
            ClientInitialVector = Convert.FromBase64String(info[0]);

            objDES.IV = ClientInitialVector;
            objDES.Padding = PaddingMode.PKCS7;
            objDES.Mode = CipherMode.CBC;

            var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var keyPath = Path.Combine(myDocs, "lumi", "server.xml");
            StreamReader sr = new StreamReader(keyPath);
            string xmlParameters = sr.ReadToEnd();
            objRSA.FromXmlString(xmlParameters);

            byte[] byteKey = objRSA.Decrypt(Convert.FromBase64String(info[1]), RSAEncryptionPadding.Pkcs1);
            objDES.Key = byteKey;
            byte[] byteCiphertexti = Convert.FromBase64String(info[2]);
            MemoryStream ms = new MemoryStream(byteCiphertexti);
            CryptoStream cs = new CryptoStream(ms, objDES.CreateDecryptor(), CryptoStreamMode.Read);

            byte[] byteTextiDekriptuar = new byte[ms.Length];
            cs.Read(byteTextiDekriptuar, 0, byteTextiDekriptuar.Length);
            cs.Close();

            string decryptedText = Encoding.UTF8.GetString(byteTextiDekriptuar);
            return decryptedText;

        }

        private string CalculateHash(string SaltedPassword)
        {
            byte[] byteSaltedPassword = Encoding.UTF8.GetBytes(SaltedPassword);
            SHA1CryptoServiceProvider objHash = new SHA1CryptoServiceProvider();
            byte[] byteSaltedHashPassword = objHash.ComputeHash(byteSaltedPassword);

            return Convert.ToBase64String(byteSaltedHashPassword);
        }
    }

  
}
