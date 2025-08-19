using DevTurret.Interfaces;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.Classes.VideoSource.Processors
{
    public class GrayscaleProcessor : IFrameProcessor
    {
        public void Process(Mat frame) => CvInvoke.CvtColor(frame, frame, ColorConversion.Bgr2Gray);
    }
}
