using DevTurret.Interfaces;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Emgu.Util.Platform;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DevTurret.Classes
{
    public class MatBuilder : IMatBuilder
    {
        private Mat _image;

        private bool _disposed = false;

        public MatBuilder(Mat image)
        {
            if (image == null) throw new ArgumentNullException("image is null");
            _image = image;
        }

        public MatBuilder(string path)
        {
            if (path == null) throw new ArgumentNullException("path is null");
            try
            {
                _image = CvInvoke.Imread(path);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IMatBuilder SetGrayScale()
        {
            using (Mat grayMat = new Mat())
            {
                CvInvoke.CvtColor(_image, grayMat, ColorConversion.Bgr2Gray);

                _image?.Dispose();
                _image = grayMat.Clone();
            }
            return this;
        }
        public IMatBuilder Rotate(int angle)
        {
            PointF center = new PointF(_image.Width / 2f, _image.Height / 2f);
            Mat rotationMatrix = new Mat();

            // Исправленный вызов GetRotationMatrix2D
            CvInvoke.GetRotationMatrix2D(center, angle, 1.0, rotationMatrix);

            // Вычисляем новые размеры
            double radians = angle * Math.PI / 180;
            double sin = Math.Abs(Math.Sin(radians));
            double cos = Math.Abs(Math.Cos(radians));
            int newWidth = (int)(_image.Width * cos + _image.Height * sin);
            int newHeight = (int)(_image.Width * sin + _image.Height * cos);

            // Корректируем матрицу преобразования
            double[] data = new double[6];
            rotationMatrix.CopyTo(data);
            data[2] += (newWidth / 2 - center.X);
            data[5] += (newHeight / 2 - center.Y);
            rotationMatrix.SetTo(data);

            // Применяем аффинное преобразование
            Mat rotatedImage = new Mat();
            CvInvoke.WarpAffine(
                _image,
                rotatedImage,
                rotationMatrix,
                new Size(newWidth, newHeight),
                Inter.Linear,
                Warp.Default,
                BorderType.Constant,
                new MCvScalar(0, 0, 0) // Черный фон
            );

            // Обновляем изображение
            _image.Dispose();
            _image = rotatedImage;
            rotationMatrix.Dispose();

            return this;
        } //реализовать dispose

        public IMatBuilder SetSize(int height, int weight)
        {
            //Добавить проверку параметров 


            using (Mat resizeMat = new Mat())
            {
                CvInvoke.Resize(_image, resizeMat, new Size(300, 200));

                _image?.Dispose();
                _image = resizeMat.Clone();
            }
            return this;
        }

        public IMatBuilder Crop(int x, int y, int width, int height)
        {
            //Добавить проверку параметров 
            Rectangle roi = new Rectangle(x, y, width, height);
            using (Mat cropMat = new Mat(_image, roi))
            {
                _image?.Dispose();
                _image = cropMat.Clone();
            }
            return this;
        }

        public IMatBuilder SetGaussianBlur(int x, int y, double sigma = 0)
        {
            using (Mat blurMat = new Mat())
            {
                CvInvoke.GaussianBlur(_image, blurMat, new Size(x, y), sigma);

                _image?.Dispose();
                _image = blurMat.Clone();
            }
            return this;
        }

        public IMatBuilder IncreaseSharpness(double strength = 1.0, int kernelSize = 5)
        {
            using (Mat blurred = new Mat())
            using (Mat sharpened = new Mat())
            {
                CvInvoke.GaussianBlur(_image, blurred, new Size(kernelSize, kernelSize), 0);
                CvInvoke.AddWeighted(_image, 1.0 + strength, blurred, -strength, 0, sharpened);

                _image.Dispose();
                _image = sharpened.Clone();
            }
            return this;

        }

        public IMatBuilder SetCanny(int threshold1, int threshold2)
        {
            SetGrayScale();
            using (Mat canny = new Mat())
            {
                CvInvoke.Canny(_image, canny, threshold1, threshold2);

                _image.Dispose();
                _image = canny.Clone();
            }
            return this;
        }


        public Mat Build()
        {
            return _image.Clone();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _image?.Dispose();
                }

                _disposed = true;
            }
        }

        ~MatBuilder()
        {
            Dispose(false);
        }

    }
}
