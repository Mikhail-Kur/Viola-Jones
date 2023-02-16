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

        private void Form1_Load(object sender, EventArgs e)
        {


        }
        public Mat MyImage;
        public int SizeForRec= 200;
        public int Neighbors = 15;
        private void button1_Click(object sender, EventArgs e)
        {
            
            pictureBox1.Image = null;
            
            int SizeFor;
            int Neig;
            if (textBox1.Text == string.Empty)
            {
                SizeFor = SizeForRec;
            }
            else
            {
                SizeFor = Int32.Parse(textBox1.Text);
            }
            if (textBox2.Text == string.Empty)
            {
                Neig = Neighbors;
            }
            else
            {
                Neig = Int32.Parse(textBox2.Text);
            }
            if (SizeFor < 0)
            {
                SizeFor = SizeForRec;
            }
            if (Neig < 0)
            {
                Neig = Neighbors;
            }
            Mat MyImageForProces;
            try
            {
                MyImageForProces = MyImage.Clone();

            }
            catch (NullReferenceException)
            {

                throw new Exception("не выбранно изображение, нажмите продолжить") ;
            }

            
            CascadeClassifier cascade = new CascadeClassifier("cascade/cascade-40-3s-0.000308372.xml");        
            Mat gray = new Mat();
            if (MyImage.Channels()>1)
            {
                Cv2.CvtColor(MyImageForProces, gray, ColorConversionCodes.BGR2GRAY);
                Cv2.EqualizeHist(gray, gray);

            }

            OpenCvSharp.Size minsize = new OpenCvSharp.Size(SizeFor, SizeFor);
            OpenCvSharp.Size maxSize = new OpenCvSharp.Size(800, 800);

            Rect[] rect = cascade.DetectMultiScale(gray,3, Neig,0,minsize, maxSize);
            Console.WriteLine(rect.Length);
            for (int i=0; i<=rect.Length-1;i++)
            {
                
                Cv2.Rectangle(MyImageForProces, rect[i], Scalar.Red, thickness: 3);

            }

            Image img;
            using (var ms = new MemoryStream(MyImageForProces.ToBytes()))
            {
                img = Image.FromStream(ms);
            }
            pictureBox1.Image =img;

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    MyImage = Cv2.ImRead(dlg.FileName);
                    pictureBox1.Image = new Bitmap(dlg.FileName);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void фильтрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Mat final = new Mat();
            //Mat guss = new Mat();

            ////Cv2.GaussianBlur(MyImage, guss, new OpenCvSharp.Size(1, 1), 109, 0, BorderTypes.Default);
            //Cv2.BilateralFilter(MyImage, guss,20, 100,20);
            //Mat gray = new Mat();
            //Cv2.CvtColor(guss, gray, ColorConversionCodes.RGB2GRAY);
            //Cv2.Laplacian(gray, final, -1,1,2,0,BorderTypes.Default);
            //Cv2.Threshold(final, final, 0, 255, ThresholdTypes.Otsu );
            //Cv2.ConvertScaleAbs(final, final);
            //Image img;
            //using (var ms = new MemoryStream(final.ToBytes()))
            //{
            //    img = Image.FromStream(ms);
            //}
            //pictureBox1.Image = img;
            //MyImage = final;
            Mat gray1 = new Mat();
            Mat gray2 = new Mat();
            Mat img1  = Cv2.ImRead("D:/DataSet/Daphnia_new_set/holo_2019_05_31_16_40_46_8750-0072000mkm.bmp");
            Mat fone = Cv2.ImRead("D:/DataSet/Daphnia_new_set/Holograms/holo_2019_05_31_16_40_46_8750.bmp-2048.bmp");
            Cv2.CvtColor(img1, gray1, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(gray1, gray1);
            Cv2.BitwiseNot(gray1, gray1);
            Cv2.CvtColor(fone, gray2, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(gray2, gray2);
            Mat final = img1.Clone();
            Cv2.Absdiff(gray1, gray2, final);
            Image img;
            using (var ms = new MemoryStream(final.ToBytes()))
            {
                img = Image.FromStream(ms);
            }
            pictureBox1.Image = img;
            


        }

        private void фильтрГауссаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Mat final = new Mat();
            Mat gray = new Mat();

            Cv2.CvtColor(MyImage, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.EqualizeHist(gray, gray);
            OpenCvSharp.Size size = new OpenCvSharp.Size(5, 5);
            
            Cv2.GaussianBlur(gray,gray,size,10,10,BorderTypes.Default);
            Cv2.AdaptiveThreshold(gray, final,255, AdaptiveThresholdTypes.GaussianC,ThresholdTypes.Binary,751,35);
            Image img;
            using (var ms = new MemoryStream(final.ToBytes()))
            {
                img = Image.FromStream(ms);
            }
            pictureBox1.Image = img;
            MyImage = final;
            
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            int SizeFor;
            int Neig;
            if (textBox1.Text == string.Empty)
            {
                SizeFor = SizeForRec;
            }
            else
            {
                SizeFor = Int32.Parse(textBox1.Text);
            }
            if (textBox2.Text == string.Empty)
            {
                Neig = Neighbors;
            }
            else
            {
                Neig = Int32.Parse(textBox2.Text);
            }
            if (SizeFor < 0)
            {
                SizeFor = SizeForRec;
            }
            if (Neig < 0)
            {
                Neig = Neighbors;
            }


            CascadeClassifier cascade = new CascadeClassifier("cascade/cascadetest2.xml");
            Mat gray = new Mat();
            OpenCvSharp.Size minsize = new OpenCvSharp.Size(SizeFor, SizeFor);
            OpenCvSharp.Size maxSize = new OpenCvSharp.Size(600, 600);
            var dir = new DirectoryInfo("D:/OpenCV/opencv/build/x64/vc15/bin/Source/folder");

            foreach (FileInfo file in dir.GetFiles())
            {
                string path = "D:/OpenCV/opencv/build/x64/vc15/bin/Source/folder/" + file.Name;
                Mat image = Cv2.ImRead(path);
                if (MyImage.Channels() > 1)
                {
                    Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
                    Cv2.EqualizeHist(gray, gray);

                }
                Rect[] rect = cascade.DetectMultiScale(gray, 3, Neig, 0, minsize, maxSize);
                Console.WriteLine(rect.Length);
                for (int i = 0; i <= rect.Length - 1; i++)
                {

                    Cv2.Rectangle(image, rect[i], Scalar.Red, thickness: 3);

                }
                Image img;
                using (var ms = new MemoryStream(image.ToBytes()))
                {
                    img = Image.FromStream(ms);
                }
                img.Save(file.Name);
            }
        }
    }
}
