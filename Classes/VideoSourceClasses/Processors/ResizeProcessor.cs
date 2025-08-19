using DevTurret.Interfaces;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.Classes.VideoSource.Processors
{
    public class ResizeProcessor : IFrameProcessor
    {
        private readonly Size _size;
        public ResizeProcessor(Size size) => _size = size;
        public void Process(Mat frame) => CvInvoke.Resize(frame, frame, _size);
    }
}
