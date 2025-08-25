using Emgu.CV;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoloDotNet;
using YoloDotNet.Core;
using YoloDotNet.Enums;
using YoloDotNet.Models;
using YoloDotNet.Models.Interfaces;

namespace DevTurret.classes
{
    public class ObjectDetector : IDisposable
    {
        private readonly DetectorOptions _options;
        private readonly Yolo _yolo;
        private readonly VideoCapture _capture;
        private SKBitmap _skBitmap;
        private Mat _frame;
        private bool _isRunning;


        public delegate void DetectionHandler(IReadOnlyList<ObjectDetection> results);
        public event DetectionHandler OnDetection;

        public ObjectDetector(DetectorOptions options = null)
        {
            _options = options ?? new DetectorOptions();

            _yolo = new Yolo(new YoloOptions
            {
                OnnxModel = _options.ModelPath,
                ImageResize = ImageResize.Proportional,
                ExecutionProvider = new TensorRtExecutionProvider
                {
                    GpuId = 0,
                    Precision = TrtPrecision.FP16,
                    BuilderOptimizationLevel = 3,
                    EngineCachePath = _options.EngineCache,
                    EngineCachePrefix = "yolo11n_fp16_"
                }
            });

            _capture = new VideoCapture(0);
            _capture.Set(Emgu.CV.CvEnum.CapProp.FrameWidth, _options.Width);
            _capture.Set(Emgu.CV.CvEnum.CapProp.FrameHeight, _options.Height);

            _skBitmap = new SKBitmap(_options.Width, _options.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
            _frame = new Mat();
        }

        public void Detect()
        {
            while (_isRunning)
            {
                _frame = _capture.QueryFrame();
                if (_frame == null) continue;

                FrameConverter.MatToSKBitmap(_frame, ref _skBitmap);

                var results = _yolo.RunObjectDetection(_skBitmap, confidence: _options.Confidence, iou: _options.Iou);

                OnDetection?.Invoke(results);
            }
        }

        public void Start() { _isRunning = true; Detect(); }
        public void Stop() => _isRunning = false;

        public void Dispose()
        {
            _capture?.Dispose();
            _frame?.Dispose();
            _skBitmap?.Dispose();
            _yolo?.Dispose();
        }


    }
}