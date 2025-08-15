using DevTurret.Classes.VideoSource.Processors;
using DevTurret.Classes.VideoSourceClasses.Processors;
using DevTurret.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.Classes.VideoSource
{
    public class VideoProcessorBuilder
    {
        private List<IFrameProcessor> _processors = new List<IFrameProcessor>();

        private List<IDetectProcessor> _detectors = new List<IDetectProcessor>();
        public VideoProcessorBuilder AddGrayscale()
        {
            _processors.Add(new GrayscaleProcessor());
            return this;
        }

        public VideoProcessorBuilder AddCanny(double thresh1, double thresh2)
        {
            _processors.Add(new CannyProcessor(thresh1, thresh2));
            return this;
        }

        public VideoProcessorBuilder SetSize(Size size)
        {
            _processors.Add(new ResizeProcessor(size));
            return this;
        }

        public VideoProcessorBuilder AddDetector(IDetectProcessor detector)
        {
            _detectors.Add(detector);
            return this;
        }

        public VideoProcessorBuilder AddGaussianBlur(int kernelSize = 5)
        {
            _processors.Add(new GaussianBlurProcessor(kernelSize));
            return this;
        }

        public VideoProcessor Build()
        {
            return new VideoProcessor(_processors, _detectors);
        }

    }
}
