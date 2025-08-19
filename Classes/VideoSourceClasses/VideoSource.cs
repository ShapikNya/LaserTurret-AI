using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.Classes.VideoSourceClasses
{
    public class VideoSource : IDisposable
    {
        private VideoCapture _capture;
        public Mat _frame; 

        public VideoSource(string filePath = null)
        {
            _capture = string.IsNullOrEmpty(filePath) ? new VideoCapture(0) : new VideoCapture(filePath);
            _frame = new Mat();
        }

        public bool IsOpened => _capture.IsOpened;

        public Mat GetNextFrame()
        {
            _capture.Read(_frame); 
            return _frame.IsEmpty ? null : _frame;
        }

        /*_capture.Read(_frame);
            return _frame.IsEmpty? null : _frame.Clone(); // Возвращаем клон*/

        public void Dispose()
        {
            _frame?.Dispose();
            _capture.Dispose();
        }
    }
}
