using DevTurret.Interfaces;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.Classes.VideoSourceClasses.Processors
{
    public class GaussianBlurProcessor : IFrameProcessor
    {
        private readonly int _kernelSize;
        public GaussianBlurProcessor(int kernelSize) => _kernelSize = kernelSize;
        public void Process(Mat frame) => CvInvoke.GaussianBlur(frame, frame, new Size(_kernelSize, _kernelSize), 0);
    }
}
