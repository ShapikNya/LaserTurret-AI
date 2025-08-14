using DevTurret.Classes;
<<<<<<< HEAD
using DevTurret.Classes.VideoSource;
using DevTurret.Classes.VideoSourceClasses;
=======
>>>>>>> a20105c934463e9ad5cdeee966de2f06756b45da
using Emgu.CV;          
using Emgu.CV.CvEnum;  
using Emgu.CV.Structure;
using System.Drawing;
using System.Drawing.Imaging;

namespace DevTurret
{
    public partial class Form1 : Form
    {
        const string logImgPath = @"C:\Users\Shapi\Desktop\DevTurret\Image\Log";
        public Form1()
        {
            InitializeComponent();
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Images (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        imgPath.Text = openFileDialog.FileName;

                        MatBuilder builder = new MatBuilder(imgPath.Text);
<<<<<<< HEAD
                        Mat changedImg = builder.SetCanny(10, 100).Build();
=======
                        Mat changedImg = builder.SetCanny(10,100).Build();
>>>>>>> a20105c934463e9ad5cdeee966de2f06756b45da

                        ImageViewerForm viewForm = new ImageViewerForm(changedImg.ToBitmap()); viewForm.Show();

                        changedImg.ToBitmap().Save(Path.Combine(logImgPath, "test.jpeg"), ImageFormat.Jpeg);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"ќшибка: {ex.Message}");
                    }
                }
            }
        }
<<<<<<< HEAD

        private void cameraBtn_Click(object sender, EventArgs e)
        {
            VideoProcessorBuilder builder = new VideoProcessorBuilder();
            builder.AddGrayscale();
            //builder.AddCanny(10,100);

            VideoProcessor processor = builder.Build();

            VideoSource videoSource = new VideoSource();

            string windowName = "Webcam";
            CvInvoke.NamedWindow(windowName);

            while (true)
            {

                processor.ProcessFrame(videoSource.GetNextFrame());

                CvInvoke.Imshow(windowName, videoSource._frame);


                if (CvInvoke.WaitKey(30) == 'q') // 30 мс = ~33 кадра/с
                    break;
            }
        }
=======
>>>>>>> a20105c934463e9ad5cdeee966de2f06756b45da
    }
}
