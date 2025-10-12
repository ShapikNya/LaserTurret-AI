using DevTurret.Classes;
using DevTurret.Classes.VideoSource;
using DevTurret.Classes.VideoSourceClasses;
using DevTurret.Classes.VideoSourceClasses.Processors;
using Emgu.CV;          
using Emgu.CV.CvEnum;  
using Emgu.CV.Reg;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using SkiaSharp;  // Для работы с изображениями (загрузка, ресайз, отрисовка)
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using YoloDotNet;  // Основная библиотека YoloDotNet
using YoloDotNet.Core;
using YoloDotNet.Enums;  // Для перечислений, как типы ресайза
using YoloDotNet.Extensions;  // Для расширений, как Draw()
using YoloDotNet.Models;  // Для моделей результатов (ObjectDetectionModel и т.д.)
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.DataFormats;

namespace DevTurret
{
    public partial class servoBtn : Form
    {
        const string PhtsPath = @"C:\Users\Shapi\Desktop\SandBox\Log";
        const string logImgPath = @"C:\Users\Shapi\Desktop\SandBox\Log";
        const string faceCascadePath = @"C:\Users\Shapi\Desktop\SandBox\HaarCascades\haarcascade_frontalface_default.xml";
        const string modelPath = @"C:\Users\Shapi\Desktop\SandBox\Models\yolo11n_fixed.onnx";
        public servoBtn()
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
            Stopwatch stopwatch = Stopwatch.StartNew(); double frameTime, fps;
            while (true)
            {
                processor.ProcessFrame(videoSource.GetNextFrame(), rect);


                foreach (Rectangle face in rect)
                {
                    CvInvoke.Rectangle(videoSource._frame, face, new MCvScalar(0, 255, 0), 10); // Зелёный, толщина 10
                }

                CvInvoke.Imshow(windowName, videoSource._frame);
                frameTime = stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart(); fps = 1 / frameTime;

                rect.Clear();

                fpsLabel.Text = $"FPS: {fps}";
                if (CvInvoke.WaitKey(1) == 'q') // 30 мс = ~33 кадра/с
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


                    /*
                                        // Создаём канвас для рисования
                                        using var canvas = new SKCanvas(image);
                                        using var paint = new SKPaint
                                        {
                                            Color = SKColors.Red,  // Цвет точки (красный)
                                            Style = SKPaintStyle.Fill,  // Заполненная точка
                                            IsAntialias = true  // Сглаживание
                                        };

                                        // Рисуем точку в центре каждого bounding box
                                        foreach (var detection in results)
                                        {
                                            var centerX = detection.BoundingBox.MidX;
                                            var centerY = detection.BoundingBox.MidY;
                                            canvas.DrawCircle(centerX, centerY, 15, paint);  // Рисуем круг радиусом 5 пикселей
                                        }
                                        string outputFile = Path.Combine(logImgPath, "yolo_result.jpg");
                                        image.Save(outputFile);
                                        yolo.Dispose();
                    */
                }
            }
        }

        private void cameraYoloBtn_Click(object sender, EventArgs e)
        {


            using (VideoSource videoSource = new VideoSource())
            {
                using (var yolo = new Yolo(new YoloOptions
                {
                    OnnxModel = modelPath,
                    ImageResize = ImageResize.Proportional,
                    ExecutionProvider = new CudaExecutionProvider(GpuId: 0, PrimeGpu: true)
                }))
                {
                    Mat frame = new();
                    Stopwatch stopwatch = Stopwatch.StartNew(); double frameTime, fps;
                    while (true)
                    {
                        frame = videoSource.GetNextFrame();
                        using Bitmap bitmap = frame.ToBitmap();
                        using var stream = new MemoryStream();
                        bitmap.Save(stream, ImageFormat.Jpeg); // Сохраняем в поток
                        stream.Position = 0; // Сбрасываем позицию
                        using var image = SKBitmap.Decode(stream); // Декодируем в SKBitmap

                        var results = yolo.RunObjectDetection(image, confidence: 0.25, iou: 0.7);

                        image.Draw(results);

                        // Конвертация SKBitmap в Mat
                        using var outputMat = new Mat(image.Height, image.Width, DepthType.Cv8U, 4); // RGBA
                        var pixels = image.Bytes; // Пиксели в формате RGBA
                        Marshal.Copy(pixels, 0, outputMat.DataPointer, pixels.Length); // Копируем пиксели
                        using var bgrMat = new Mat();
                        CvInvoke.CvtColor(outputMat, bgrMat, ColorConversion.Rgba2Bgr); // RGBA -> BGR для Imshow

                        // Отображение в окне

                        CvInvoke.Imshow("Detection Output", bgrMat);
                        frameTime = stopwatch.Elapsed.TotalSeconds;
                        stopwatch.Restart(); fps = 1 / frameTime;

                        if (CvInvoke.WaitKey(1) == 'q')
                            break;
                        fpsLabel.Text = $"FPS: {fps}";
                    }
                    frame.Dispose();

                }

            }
        }

        private void cameraTestBtn_Click(object sender, EventArgs e)
        {
            using (var camera = new VideoSource())
            {
                Stopwatch stopwatch = Stopwatch.StartNew(); double frameTime;
                while (true)
                {
                    //CvInvoke.Imshow("Camera", camera.GetNextFrame());
                    camera.GetNextFrame();
                    frameTime = stopwatch.Elapsed.TotalSeconds;
                    fpsLabel.Text = $"FPS: {1 / frameTime}";
                    stopwatch.Restart();


                    if (CvInvoke.WaitKey(1) == 'q') break;
                }


                //НУЖНОЕ
                /*        Mat frame = camera.GetNextFrame();
                        Mat matBGRA = new Mat(480, 640, DepthType.Cv8U, 4);
                        SKBitmap skBitmap = new SKBitmap(640, 480, SKColorType.Bgra8888, SKAlphaType.Premul);

                        CvInvoke.CvtColor(frame, matBGRA, ColorConversion.Bgr2Bgra);

                        // Unsafe copy через Span<byte>
                        unsafe
                        {
                            byte* srcPtr = (byte*)matBGRA.DataPointer.ToPointer();
                            byte* dstPtr = (byte*)skBitmap.GetPixels().ToPointer();
                            int bytes = 640 * 480 * 4;
                            Buffer.MemoryCopy(srcPtr, dstPtr, bytes, bytes);
                        }

                        // YOLO
                        using var yolo = new Yolo(new YoloOptions
                        {
                            OnnxModel = modelPath,
                            ImageResize = ImageResize.Proportional,
                            ExecutionProvider = new CudaExecutionProvider(GpuId: 0, PrimeGpu: true)
                        });

                        var results = yolo.RunObjectDetection(skBitmap, confidence: 0.25, iou: 0.7);





                        skBitmap.Draw(results);

                        string outputFile = Path.Combine(logImgPath, "yolo_result.jpg");
                        skBitmap.Save(outputFile);
                        yolo.Dispose();
                    }*/
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (VideoSource videoSource = new VideoSource())
            {
                int frameCount = 0;
                while (true)
                {
                    if (CvInvoke.WaitKey(1) == 'q') { break; }
                    Mat frame = videoSource.GetNextFrame();
                    CvInvoke.Imshow("Detection Output", frame);
                    if (CvInvoke.WaitKey(1) == 's')
                    {
                        if (frameCount == 15) break;
                        string outputFile = Path.Combine(PhtsPath, "pht" + frameCount.ToString() + ".jpg");
                        frame.Save(outputFile);
                        MessageBox.Show("pht" + frameCount.ToString() + ".jpg is saved!");
                        frameCount++;
                    }
                }

            }
        }

        private void calibBtn_Click(object sender, EventArgs e)
        {
            int boardWidth = 6;  // число внутренних углов по горизонтали
            int boardHeight = 4; // число внутренних углов по вертикали
            float squareSize = 0.015f; // размер клетки в метрах (1.5 см)
            int phtWithCorners = 0;

            Size patternSize = new Size(boardWidth, boardHeight);

            List<VectorOfPoint3D32F> objectPoints = new List<VectorOfPoint3D32F>();
            List<VectorOfPointF> imagePoints = new List<VectorOfPointF>();

            VectorOfPoint3D32F objPts = new VectorOfPoint3D32F();
            for (int i = 0; i < boardHeight; i++) // строки
            {
                for (int j = 0; j < boardWidth; j++) // столбцы
                {
                    objPts.Push(new MCvPoint3D32f[] { new MCvPoint3D32f(j * squareSize, i * squareSize, 0f) });
                }
            }

            string[] images = System.IO.Directory.GetFiles(PhtsPath, "*.jpg");

            foreach (string imagePath in images)
            {
                Mat img = CvInvoke.Imread(imagePath, ImreadModes.AnyColor);
                Mat gray = new Mat();
                CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);

                VectorOfPointF corners = new VectorOfPointF();
                bool found = CvInvoke.FindChessboardCorners(gray, patternSize, corners,
                    CalibCbType.AdaptiveThresh | CalibCbType.NormalizeImage);

                if (found)
                {
                    CvInvoke.CornerSubPix(gray, corners, new Size(11, 11), new Size(-1, -1),
                        new MCvTermCriteria(30, 0.01));

                    imagePoints.Add(corners);
                    objectPoints.Add(objPts);

                    CvInvoke.DrawChessboardCorners(img, patternSize, corners, found);
                    CvInvoke.Imshow("Corners", img);
                    CvInvoke.WaitKey(100);

                    phtWithCorners++;
                    MessageBox.Show("FOUND CORNERS - " + imagePath);
                }
                else
                {
                    MessageBox.Show("NOT FOUND CORNERS - " + imagePath);
                }
            }

            MessageBox.Show(phtWithCorners.ToString());

            CvInvoke.DestroyAllWindows();



            if (objectPoints.Count < 3)
            {
                MessageBox.Show("Недостаточно кадров с найденными углами для калибровки!");
                return;
            }

            MCvPoint3D32f[][] objectPointsArray = objectPoints
                .Select(v => v.ToArray())
                .ToArray();

            PointF[][] imagePointsArray = imagePoints
                .Select(v => v.ToArray())
                .ToArray();

            Size imageSize;
            using (Mat sample = CvInvoke.Imread(images[0], ImreadModes.AnyColor))
            {
                imageSize = sample.Size;
            }

            Mat cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1);
            Mat distCoeffs = new Mat(1, 8, DepthType.Cv64F, 1);

            MCvTermCriteria term = new MCvTermCriteria(30, 1e-6);

            Mat[] rvecs, tvecs;
            double reprojectionError = CvInvoke.CalibrateCamera(
                objectPointsArray,
                imagePointsArray,
                imageSize,
                cameraMatrix,
                distCoeffs,
                CalibType.RationalModel,
                term,
                out rvecs,
                out tvecs
            );


            StringBuilder sb = new StringBuilder();

            sb.AppendLine("=== РЕЗУЛЬТАТ КАЛИБРОВКИ ===");
            sb.AppendLine($"Средняя ошибка репроекции: {reprojectionError:F6}\n");

            sb.AppendLine("Матрица камеры (Camera Matrix):");
            sb.AppendLine(MatrixToString(cameraMatrix));
            sb.AppendLine();

            sb.AppendLine("Коэффициенты дисторсии (Distortion Coeffs):");
            sb.AppendLine(MatrixToString(distCoeffs));

          //  Clipboard.SetText(sb.ToString());
            MessageBox.Show(sb.ToString(), "Результаты калибровки");


        }


        private string MatrixToString(Mat mat)
        {
            var data = new double[mat.Rows * mat.Cols];
            mat.CopyTo(data);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < mat.Rows; i++)
            {
                for (int j = 0; j < mat.Cols; j++)
                {
                    sb.Append($"{data[i * mat.Cols + j],10:F6} ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*PointF targetPx = new PointF(1, 1); // пиксели цели
            var (yaw, pitch) = GetAnglesFromPixel(targetPx);
            MessageBox.Show($"Yaw: {yaw:F2}°, Pitch: {pitch:F2}°");*/

            //640w 480h

            StringBuilder sb = new StringBuilder();

            for (int w = 0; w != 640; w= w + 80)
            {
                for (int h = 0; h != 480; h= h + 80)
                {
                    PointF targetPx = new PointF(w,h); 
                    var (yaw, pitch) = GetAnglesFromPixel(targetPx);
                    sb.AppendLine($"X = {w}, Y = {h}. Yaw = {Math.Round(yaw,3)}, Pitch = {Math.Round(pitch,3)}");
                }
            }

            Clipboard.SetText(sb.ToString());
            MessageBox.Show(sb.ToString(), "Отображение лучей");
        }


        private (double yaw, double pitch) GetAnglesFromPixel(PointF pixel)
        {
            // Задаем константную матрицу камеры
            double[,] K = new double[3, 3]
            {
        { 492.279154,    0.0,      337.929005 },
        {    0.0,    490.880685,   229.548998 },
        {    0.0,       0.0,          1.0     }
            };

            // Преобразуем в Matrix<double> для работы с CvInvoke
            Matrix<double> Kmat = new Matrix<double>(K);
            Matrix<double> Kinv = new Matrix<double>(3, 3);
            CvInvoke.Invert(Kmat, Kinv, DecompMethod.LU);

            // Вектор пикселя (u, v, 1)
            Matrix<double> uv1 = new Matrix<double>(new double[,] {
        { pixel.X },
        { pixel.Y },
        { 1.0 }
    });

            // [xc, yc, 1] = K^-1 * [u, v, 1]
            Matrix<double> xyz = new Matrix<double>(3, 1);
            CvInvoke.Gemm(Kinv, uv1, 1.0, null, 0.0, xyz);

            double x = xyz[0, 0] / xyz[2, 0];
            double y = xyz[1, 0] / xyz[2, 0];

            double angleXdeg = Math.Atan(x) * 180.0 / Math.PI; // горизонтальный угол (yaw)
            double angleYdeg = Math.Atan(y) * 180.0 / Math.PI; // вертикальный угол (pitch)

            return (angleXdeg, angleYdeg);
        }

    }
}
