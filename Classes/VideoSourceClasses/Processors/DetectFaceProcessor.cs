using DevTurret.Interfaces;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.Classes.VideoSourceClasses.Processors
{
    public class DetectFaceProcessor : IDetectProcessor
    {
        private CascadeClassifier _faceCascade;

        public DetectFaceProcessor(string cascadePath)
        {
            _faceCascade = new CascadeClassifier(cascadePath);
        }
        //доработать настройку параметров
        public Rectangle[] GetFaces(Mat frame)  
        {
            return _faceCascade.DetectMultiScale(
            frame,
            scaleFactor: 1.05,
            minNeighbors: 6,   
            minSize: new Size(50, 50) 
        );
        }
    }
}