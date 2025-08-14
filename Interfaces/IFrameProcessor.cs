using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;

namespace DevTurret.Interfaces
{
    public interface IFrameProcessor
    {
        void Process(Mat frame);
    }
}
