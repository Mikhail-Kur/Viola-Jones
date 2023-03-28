using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Viola_Jones
{
    public partial class MultiProcesForm : Form
    {
        public MultiProcesForm()
        {
            InitializeComponent();
        }
        private string pathOut;
        private string pathIn;            
        
        private  void button1_Click(object sender, EventArgs e)
        {
             
            CascadeClassifier cascade = new CascadeClassifier("cascade/cascade-40-3s-0.000308372.xml");
            Mat gray = new Mat();
            OpenCvSharp.Size minsize = new OpenCvSharp.Size(Settings.SizeForRec, Settings.SizeForRec);
            OpenCvSharp.Size maxSize = new OpenCvSharp.Size(600, 600);
            try
            {
                var dir = new DirectoryInfo(pathOut);
                foreach (FileInfo file in dir.GetFiles())
                {
                    string path = pathOut + "/" + file.Name;
                    Mat image = Cv2.ImRead(path);
                    if (image.Channels() > 1)
                    {
                        Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
                        Cv2.EqualizeHist(gray, gray);

                    }
                    Rect[] rect = cascade.DetectMultiScale(gray, 3, Settings.Neighbors, 0, minsize, maxSize);
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
                    string savePath = pathIn + "/" + file.Name;
                    img.Save(savePath);
                }
            }
            catch (ArgumentNullException)
            {
                ErrorForm errorForm = new ErrorForm("Путь не выбран, нажмите продолжить");
                errorForm.ShowDialog(this);
            }          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Выбрать папку с изображениями";
                
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    pathOut = dlg.SelectedPath.Replace('\\', '/');
                    label3.Text = pathOut;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Выбрать папку для сохранения";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    pathIn = dlg.SelectedPath.Replace('\\', '/');
                    label4.Text = pathIn;
                }
            }
        }
    }
}
