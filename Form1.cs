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
using SkiaSharp;  // Для работы с изображениями (загрузка, ресайз, отрисовка)
using YoloDotNet;  // Основная библиотека YoloDotNet
using YoloDotNet.Enums;  // Для перечислений, как типы ресайза
using YoloDotNet.Models;  // Для моделей результатов (ObjectDetectionModel и т.д.)
using YoloDotNet.Extensions;  // Для расширений, как Draw()
using static System.Net.Mime.MediaTypeNames;
using YoloDotNet.Core;

namespace DevTurret
{
    public partial class Form1 : Form
    {
        const string logImgPath = @"C:\Users\Shapi\Desktop\Turret\Log";
        const string faceCascadePath = @"C:\Users\Shapi\Desktop\Turret\Haar Cascades\haarcascade_frontalface_default.xml";
        const string modelPath = @"C:\Users\Shapi\Desktop\Turret\Models\yolo11n_fixed.onnx";
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

        private void imgYoloBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Images (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imgPath.Text = openFileDialog.FileName;

                    var yolo = new Yolo(new YoloOptions
                    {
                        OnnxModel = modelPath,
                        ImageResize = ImageResize.Proportional,
                        ExecutionProvider = new CudaExecutionProvider(GpuId: 0, PrimeGpu: true)
                    });

                    using var image = SKBitmap.Decode(imgPath.Text);

                    var results = yolo.RunObjectDetection(image, confidence: 0.25, iou: 0.7);

                    image.Draw(results);

                    // --- исправленная часть ---
                    string outputFile = Path.Combine(logImgPath, "yolo_result.jpg");
                    image.Save(outputFile); // сохраняем обработанное изображение
                                            // --------------------------

                    yolo.Dispose();
                }
            }
        }
    }
}
