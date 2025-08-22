using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;

namespace DevTurret.classes.Cashe
{
    public class FrameSlot : IDisposable
    {
        public Mat Mat {  get; }
        public bool IsRead { get; set; }    
        public long Timestamp { get; set; }
        public object Lock { get; }

        public FrameSlot(int width, int height)
        {
            Mat = new Mat(height, width, Emgu.CV.CvEnum.DepthType.Cv8U, 3);
            IsRead = true;   
            Timestamp = 0;
            Lock = new object();
        }

        public void Dispose()
        {
            Mat.Dispose();
        }

    }
}
