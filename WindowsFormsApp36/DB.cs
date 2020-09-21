using MySql.Data.MySqlClient.Memcached;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp36
{
    class DB
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public List<MyImage> images = new List<MyImage>();

        private string[] LinksImage()
        {
            string[] links = new string[56];
            string path = @"D:\images.txt";
            try
            {
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    int i = 0;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        links[i] = line;
                        i++;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return links;
        }
        private void InsertImage1ToDB() //вызывается только один раз при добалении картинок
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Images1 VALUES (@FileName, @ImageData)";
                command.Parameters.Add("@FileName", SqlDbType.NVarChar, 50);
                command.Parameters.Add("@ImageData", SqlDbType.Image, 1000000);

                string[] links = LinksImage();

                byte[] imageData;
                int k = 0;
                foreach (string link in links)
                {
                    if (k != 55)
                    {
                        imageData = new WebClient().DownloadData(link);

                        command.Parameters["@FileName"].Value = link.Substring(link.LastIndexOf('/') + 1);
                        command.Parameters["@ImageData"].Value = imageData;

                        command.ExecuteNonQuery();
                        k++;
                    }
                }
            }
        }
        private void InsertImageToDB() //вызывается только один раз при добалении картинок
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Images1 VALUES (@FileName, @ImageData)";
                command.Parameters.Add("@FileName", SqlDbType.NVarChar, 50);
                command.Parameters.Add("@ImageData", SqlDbType.Image, 1000000);

                string[] filenames = new string[55];
                string[] shortFileNames = new string[55];
                for (int i = 0; i < 25; i++)
                {
                    filenames[i] = $@"D:\images\{i + 1}.jpg";
                    shortFileNames[i] = $"{i + 1}.jpg";
                }
                int j = 1;
                for (int i = 25; i < 48; i++, j++)
                {
                    filenames[i] = $@"D:\images\wallpapers-nature-{j}.jpg";
                    shortFileNames[i] = $"wallpapers-nature-{j}.jpg";
                }

                byte[] imageData;
                int k = 0;
                foreach (string filename in filenames)
                {
                    if (k != 55)
                    {
                        using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Open))
                        {
                            imageData = new byte[fs.Length];
                            fs.Read(imageData, 0, imageData.Length);

                            command.Parameters["@FileName"].Value = filename;
                            command.Parameters["@ImageData"].Value = imageData;

                            command.ExecuteNonQuery();
                        }
                        k++;
                    }
                }
            }
        }

        public void SelectImageFromDB()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Images";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string filename = reader.GetString(1);
                    byte[] data = (byte[])reader.GetValue(2);

                    MyImage image = new MyImage(id, filename, data);
                    images.Add(image);
                }
            }
        }

        public void SelectImage1FromDB()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                images.Clear();
                connection.Open();
                string sql = "SELECT * FROM Images1";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string filename = reader.GetString(1);
                    byte[] data = (byte[])reader.GetValue(2);

                    MyImage image = new MyImage(id, filename, data);
                    images.Add(image);
                }
            }
        }
    }
}