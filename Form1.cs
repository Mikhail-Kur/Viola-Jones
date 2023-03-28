using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;


namespace Viola_Jones
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        private Mat GrayImg(Mat imgForProces,Mat Gray) {
            
            if (MyImage.Channels() > 1)
            {
                Cv2.CvtColor(imgForProces, Gray, ColorConversionCodes.BGR2GRAY);
                Cv2.EqualizeHist(Gray, Gray);
            }
            return Gray;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Settings.SizeForRec = 200;
            Settings.Neighbors = 15;

        }
        
        public Mat MyImage;
        private void button1_Click(object sender, EventArgs e)
        {
            
            pictureBox1.Image = null;          
          
            Mat MyImageForProces;
            try
            {
                MyImageForProces = MyImage.Clone();
                Mat gray = new Mat();
                GrayImg(MyImageForProces, gray);

                CascadeClassifier cascade = new CascadeClassifier("cascade/cascade-40-3s-0.000308372.xml");
                OpenCvSharp.Size minsize = new OpenCvSharp.Size(Settings.SizeForRec, Settings.SizeForRec);
                OpenCvSharp.Size maxSize = new OpenCvSharp.Size(800, 800);
                Rect[] rect = cascade.DetectMultiScale(gray, 3, Settings.Neighbors, 0, minsize, maxSize);
                Console.WriteLine(rect.Length);
                for (int i = 0; i <= rect.Length - 1; i++)
                {
                    Cv2.Rectangle(MyImageForProces, rect[i], Scalar.Red, thickness: 3);
                }

                Image img;
                using (var ms = new MemoryStream(MyImageForProces.ToBytes()))
                {
                    img = Image.FromStream(ms);
                }
                pictureBox1.Image = img;
            }
            catch (NullReferenceException)
            {           
                ErrorForm errorForm = new ErrorForm("не выбранно изображение, нажмите продолжить");
                errorForm.ShowDialog(this);
            }
           

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Выбрать изображение";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    MyImage = Cv2.ImRead(dlg.FileName);
                    pictureBox1.Image = new Bitmap(dlg.FileName);
                }
            }
        }

        private void задатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm();
                  
            settings.ShowDialog(this);
        }

        private void обработатьСериюИхображенийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            MultiProcesForm multiProcesForm = new MultiProcesForm();
            multiProcesForm.ShowDialog(this);
        }
    }
}
