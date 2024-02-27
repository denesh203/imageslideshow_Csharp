using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
         // Define a CircularButton variable
        private string[] folderFile = null;
        private int selected = 0;
        private int begin = 0;
        private int end = 0;

        public Form1()
        {
            InitializeComponent();

            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // Handle click event for button1 (Pause button)
        }

      

        private void OpenFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] part1 = null, part2 = null, part3 = null;

                part1 = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.jpg");
                part2 = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.jpeg");
                part3 = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.bmp");

                folderFile = new string[part1.Length + part2.Length + part3.Length];

                Array.Copy(part1, 0, folderFile, 0, part1.Length);
                Array.Copy(part2, 0, folderFile, part1.Length, part2.Length);
                Array.Copy(part3, 0, folderFile, part1.Length + part2.Length, part3.Length);

                selected = 0;
                begin = 0;
                end = folderFile.Length;

                showImage(folderFile[selected]);


            }
        }
        private void showImage(string path)
        {

            Image imgtemp = Image.FromFile(path);

            // Calculate the aspect ratio
            double aspectRatioImg = (double)imgtemp.Width / imgtemp.Height;
            double aspectRatioPictureBox = (double)pictureBox1.Width / pictureBox1.Height;

            int newWidth;
            int newHeight;

            if (aspectRatioImg > aspectRatioPictureBox)
            {
                // Image is wider than PictureBox
                newWidth = pictureBox1.Width;
                newHeight = (int)(newWidth / aspectRatioImg);
            }
            else
            {
                // Image is taller than PictureBox
                newHeight = pictureBox1.Height;
                newWidth = (int)(newHeight * aspectRatioImg);
            }

            // Resize the image
            Image resizedImage = new Bitmap(imgtemp, newWidth, newHeight);

            // Draw the resized image onto a new bitmap that fills the PictureBox
            Bitmap filledImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(filledImage))
            {
                g.FillRectangle(Brushes.Black, 0, 0, pictureBox1.Width, pictureBox1.Height);
                int x = (pictureBox1.Width - newWidth) / 2;
                int y = (pictureBox1.Height - newHeight) / 2;
                g.DrawImage(resizedImage, x, y, newWidth, newHeight);
            }

            // Assign the filled image to pictureBox1
            pictureBox1.Image = filledImage;
        }

        private void prevImage()
        {
            if (selected == 0)
            {
                selected = folderFile.Length - 1;
                showImage(folderFile[selected]);
            }
            else
            {
                selected = selected - 1;
                showImage(folderFile[selected]);
            }
        }

        private void nextImage()
        {
            if (selected == folderFile.Length - 1)
            {
                selected = 0;
                showImage(folderFile[selected]);
            }
            else
            {
                selected = selected + 1;
                showImage(folderFile[selected]);
            }
        }

        private void Preview_Click(object sender, EventArgs e)
        {
            prevImage();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            nextImage();
        }
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            nextImage();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

        }

        private void Play_Pause_Click(object sender, EventArgs e)
        {
            string imagePath = @"D:\arduino-nightly-windows\play.PNG";
            string imagePath1 = @"D:\arduino-nightly-windows\pause.PNG";
            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
                Play_Pause.Image = Image.FromFile(imagePath);
            }
            else
            {
                timer1.Enabled = true;
                Play_Pause.Image = Image.FromFile(imagePath1);
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
