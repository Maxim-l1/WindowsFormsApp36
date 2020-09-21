using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp36
{
    public partial class Form1 : Form
    {
        DB db = new DB();
        void SelectDB()
        {
            db.SelectImage1FromDB();
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Task tsk = new Task(SelectDB);
            tsk.Start();
            tsk.Wait();
            foreach (MyImage image in db.images)
            {
                if (image.Id != 1 && image.Id != 2 && image.Id != 3 && image.Id != 7 && image.Id != 8 && image.Id != 13 && image.Id != 14 && image.Id != 24)
                {
                    Stream stream = new MemoryStream(image.Data);
                    Image newImage = Image.FromStream(stream);
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Image = newImage;
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox.Width = 730;
                    pictureBox.Height = 430;
                    flowLayoutPanel1.Controls.Add(pictureBox);
                }
            }
            sw.Stop();
            MessageBox.Show("На загрузку картинок ушло " + sw.Elapsed.TotalSeconds.ToString() + " секунд");
        }
    }
}
