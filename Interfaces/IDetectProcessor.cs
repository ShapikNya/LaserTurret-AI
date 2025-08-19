using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.Interfaces
{
    public interface IDetectProcessor
    {
        public Rectangle[] GetFaces(Mat frame);
    }
}
