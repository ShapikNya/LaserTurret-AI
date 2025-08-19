using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace DevTurret.Interfaces
{
    public interface IMatBuilder
    {
        public IMatBuilder SetGrayScale();
        public IMatBuilder Rotate(int angle);
        public IMatBuilder SetSize(int height, int weight);
        public IMatBuilder Crop(int x, int y, int width, int height);
        public IMatBuilder SetGaussianBlur(int x, int y, double sigma=0);
        public IMatBuilder IncreaseSharpness(double strength, int kernelSize);
        public IMatBuilder SetCanny(int threshold1, int threshold2);


        public Mat Build();

    }
}
