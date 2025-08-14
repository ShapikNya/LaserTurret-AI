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

        public VideoProcessor(List<IFrameProcessor> processors)
        {
            _processors = processors;
        }

        public void ProcessFrame(Mat frame)
        {
            foreach (var processor in _processors)
            {
                processor.Process(frame); 
            }
        }
    }
}