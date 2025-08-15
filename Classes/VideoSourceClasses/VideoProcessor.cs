using DevTurret.Interfaces;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.Classes.VideoSource
{
    public class VideoProcessor
    {
        private readonly List<IFrameProcessor> _processors;
        private readonly List<IDetectProcessor> _detectors;

        public VideoProcessor(List<IFrameProcessor> processors, List<IDetectProcessor> detectors)
        {
            _processors = processors; if (detectors != null) _detectors = detectors;
        }

        public void ProcessFrame(Mat frame)
        {
            foreach (var processor in _processors)
            {
                processor.Process(frame); 
            }
        }

        public void ProcessFrame(Mat frame, List<Rectangle> rectangles)
        {
            foreach (var processor in _processors)
            {
                processor.Process(frame);
            }

            foreach (var detector in _detectors)
            {
                var faces = detector?.GetFaces(frame); 

                if (faces != null)
                {
                    rectangles.AddRange(faces);
                }
            }
        }


    }
}