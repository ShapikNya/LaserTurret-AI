using DevTurret.Classes;
using DevTurret.Classes.VideoSource;
using DevTurret.Classes.VideoSourceClasses;
using DevTurret.Classes.VideoSourceClasses.Processors;
using Emgu.CV;          
using Emgu.CV.CvEnum;  
using Emgu.CV.Structure;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace DevTurret
{
    public partial class Form1 : Form
    {
        const string logImgPath = @"C:\Users\Shapi\Desktop\Turret-dev\Image\Log";
        const string faceCascadePath = @"C:\Users\Shapi\Desktop\Turret-dev\Haar Cascades\haarcascade_frontalface_default.xml";
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
                        Mat changedImg = builder.SetGrayScale().Build();
                        //using

                        DetectFaceProcessor faceCascade = new DetectFaceProcessor(faceCascadePath);
                        Rectangle[] faces = faceCascade.GetFaces(changedImg);

                        foreach (Rectangle face in faces)
                        {
                            CvInvoke.Rectangle(changedImg, face, new MCvScalar(0, 255, 0), 10); // Зелёный, толщина 2
                        }




                        ImageViewerForm viewForm = new ImageViewerForm(changedImg.ToBitmap()); viewForm.Show();

                        changedImg.ToBitmap().Save(Path.Combine(logImgPath, "test.jpeg"), ImageFormat.Jpeg);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            }
        }

        private void cameraBtn_Click(object sender, EventArgs e)
        {
            List<Rectangle> rect = new List<Rectangle>();

            VideoProcessorBuilder builder = new VideoProcessorBuilder();
            builder.AddGrayscale().AddGaussianBlur(5).AddDetector(new DetectFaceProcessor(faceCascadePath));

            VideoProcessor processor = builder.Build();

            VideoSource videoSource = new VideoSource();

            string windowName = "Webcam";
            CvInvoke.NamedWindow(windowName);

            while (true)
            {
                processor.ProcessFrame(videoSource.GetNextFrame(), rect);


                foreach (Rectangle face in rect)
                {
                    CvInvoke.Rectangle(videoSource._frame, face, new MCvScalar(0, 255, 0), 10); // Зелёный, толщина 10
                }

                CvInvoke.Imshow(windowName, videoSource._frame);

                rect.Clear();

                if (CvInvoke.WaitKey(30) == 'q') // 30 мс = ~33 кадра/с
                    break;
           
            }
        }
    }
}
