using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.classes
{
    public class DetectorOptions
    {
        public string ModelPath { get; set; } = @"C:\Users\Shapi\Desktop\dev\models\yolo11n_fixed.onnx";
        public string EngineCache { get; set; } = @"C:\Users\Shapi\YoloEngineCache";
        public int Width { get; set; } = 640;
        public int Height { get; set; } = 480;
        public double Confidence { get; set; } = 0.25;
        public double Iou { get; set; } = 0.7;
    }
}
