using DevTurret.Interfaces;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DevTurret.Classes.VideoSource.Processors
{
    public class CannyProcessor : IFrameProcessor
    {
        readonly double _threshold1, _threshold2;
        public CannyProcessor(double threshold1, double threshold2)
        {
            _threshold1 = threshold1; _threshold2 = threshold2;
        }

        public void Process(Mat frame) => CvInvoke.Canny(frame, frame, _threshold1, _threshold2);

    }
}
