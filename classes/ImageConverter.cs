using Emgu.CV;
using Emgu.CV.CvEnum;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.classes
{
    static class FrameConverter
    {
        public static SKBitmap MatToSKBitmap(Mat frame, ref SKBitmap skBitmap)
        {
            int width = frame.Width;
            int height = frame.Height;

            using Mat matBGRA = new Mat(height, width, DepthType.Cv8U, 4);
            CvInvoke.CvtColor(frame, matBGRA, ColorConversion.Bgr2Bgra);

            unsafe
            {
                byte* srcPtr = (byte*)matBGRA.DataPointer.ToPointer();
                byte* dstPtr = (byte*)skBitmap.GetPixels().ToPointer();
                int bytes = width * height * 4;
                Buffer.MemoryCopy(srcPtr, dstPtr, bytes, bytes);
            }

            return skBitmap;
        }
      
    }
        
}
