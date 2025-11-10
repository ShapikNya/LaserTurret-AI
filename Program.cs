using DevTurret.classes;
using Emgu.CV;
using Emgu.CV.CvEnum;
using SkiaSharp;
using System.Diagnostics;
using System.Drawing;
using YoloDotNet;
using YoloDotNet.Core;
using YoloDotNet.Enums;
using YoloDotNet.Models;
using YoloDotNet.Models.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO.Ports;
using System.Threading;
using System;

//Инициализация COM-порта
SerialPort serial;
InitSerial(); //!!!ЗДЕСЬ МОЖНО ПОМЕНЯТЬ НОМЕР COM-ПОРТА

//Выставление приоритета процесса
Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
Thread.CurrentThread.Priority = ThreadPriority.Highest;

//Инициализация таймера (подсчёт fps)
Stopwatch timer = new Stopwatch(); int frameCount = 0;

/*Console.WriteLine("Инициализация Yolo...");
using var yolo = new ObjectDetector();


yolo.OnDetection += (results) =>
{
    frameCount++;

    // Каждую секунду выводим FPS
    if (timer.ElapsedMilliseconds >= 1000)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Real FPS: {frameCount}");
        Console.ResetColor();

        frameCount = 0;
        timer.Restart();
    }

    // Выводим детекции
    if (results.Count == 0)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("No detection");
        Console.ResetColor();
    }

    else
    {
        try
        {
            var first = results.Where(r => r.Label.Name == "person");

            PointF targetPx = new PointF(first.First().BoundingBox.MidX, first.First().BoundingBox.MidY);

            var (yawCam, pitchCam) = GetAnglesFromPixel(targetPx);

            //
            var (yawLaser, pitchLaser) = GetLaserAnglesFromCameraAngles(yawCam, pitchCam, new double[] { 0.31, -0.14, 0 });
            //

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"Detected {results.Count}, first: {first.First().Label.Name}, confidence: {first.First().Confidence}");
            Console.WriteLine($"width = {targetPx.X}, height = {targetPx.Y}");
            Console.WriteLine($"yaw = {yawLaser}, pich = {pitchLaser}");

            SendToArduino($"{yawLaser+90} {yawLaser+90}");


            Console.ResetColor();
            Console.WriteLine();
        }
        catch
        {
            Console.WriteLine("No detection");
        }
    }
};*/





//основной цикл программы
while (true)
{
    await toStartPage();
}







                //НАВИГАЦИЯ

/*async Task AImode()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("======================================");
    Console.WriteLine("                 AI mode              ");
    Console.WriteLine("======================================");
    Console.ResetColor();

    Console.WriteLine("Controls:");
    Console.WriteLine("  W - Start detector");
    Console.WriteLine("  S - Stop detector");
    Console.WriteLine("  ESQ - Exit");
    Console.WriteLine();


    while (true)
    {
        var key = Console.ReadKey(true).Key;

        if (key == ConsoleKey.W)
        {
            Task.Run(() => yolo.Start()); // Start в фоне
            timer.Start();
            Log("Detector started", ConsoleColor.Green);
        }
        else if (key == ConsoleKey.S)
        {
            Task.Run(() => yolo.Stop()); // Stop в фоне
            timer.Stop();
            timer.Restart();
            frameCount = 0;
            Log("Detector stopped", ConsoleColor.Yellow);
        }
        else if (key == ConsoleKey.Escape)
        {
                toStartPage();
                break;   
        }
    }
}*/

async Task UserMode()
{
    double yaw = 0, pitch = 0;
    int width = 0, height = 0;
    int sendMode = 0;

    string input = "";

    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("======================================");
    Console.WriteLine("                User mode             ");
    Console.WriteLine("======================================");
    Console.ResetColor();

    Console.WriteLine("Controls:");
    Console.WriteLine("  W - Enter Angles");
    Console.WriteLine("  E - Enter px");
    Console.WriteLine("  S - Send");
    Console.WriteLine("  ESС - Exit");
    Console.WriteLine();


    while (true)
    {
        var key = Console.ReadKey(true).Key;

        //Ввод углов
        if (key == ConsoleKey.W)
        {
            sendMode = 0;
            Console.WriteLine("Format: Yaw;pitch. Ex: 12.45;-5.32");
            try
            {
                input = Console.ReadLine();
                int separatorIndex = input.IndexOf(';');
                if (separatorIndex != -1)
                {
                    yaw = Convert.ToDouble(input.Substring(0, separatorIndex));
                    pitch = Convert.ToDouble(input.Substring(separatorIndex + 1));
                }
                Log($"set value: yaw={yaw};pitch={pitch}",ConsoleColor.DarkGray);

                SendToArduino($"{yaw} {pitch}");

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
        //Ввод координат объекта в px
        else if (key == ConsoleKey.E)
        {
            sendMode = 1;
            Console.WriteLine("Format: width;height. Ex: 320;420");
            try
            {
                input = Console.ReadLine();
                int separatorIndex = input.IndexOf(';');
                if (separatorIndex != -1)
                {
                    width = Convert.ToInt32(input.Substring(0, separatorIndex));
                    height = Convert.ToInt32(input.Substring(separatorIndex + 1));
                }

                PointF targetPx = new PointF(width, height);
                var (yawCam, pitchCam) = GetAnglesFromPixel(targetPx);


                //!!!!Rоординаты в формате [-90;90]. 
                var (yawLaser, pitchLaser) = GetLaserAnglesFromCameraAngles(yawCam, pitchCam, new double[] { 0.0, 0.3, -0.2 }); //!!!! СМЕЩЕНИЕ ДВИГАТЕЛЕЙ ОТНОСИТЕЛЬНО КАМЕРЫ
                                                                                                                                //!!!! X,Y,Z в м
                //!!!Чтобы перейти к положительным числам [0,180], я добавлю +90 к координатам.
                yaw = yawLaser+90; pitch = pitchLaser+90;


                SendToArduino($"{yaw} {pitch}");


                Log($"set value: yaw={yaw};pitch={pitch}. for width={width}, height={height}", ConsoleColor.DarkGray);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

        }
        //Повторная отправка
        else if (key == ConsoleKey.S) 
        {
            Log($"sent to port: yaw={yaw};pitch={pitch}", ConsoleColor.Green);
            SendToArduino($"{yaw} {pitch}");
            Console.WriteLine();
        }
        else if (key == ConsoleKey.Escape)
        {
            toStartPage();
            break;
        }
    }

}

//Отображение стартовой страницы в консоли
async Task toStartPage()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("======================================");
    Console.WriteLine("        Welcome to Object Detector     ");
    Console.WriteLine("======================================");
    Console.ResetColor();

    Console.WriteLine("Select mode:");
    Console.WriteLine("  1 - User Control");
    //Console.WriteLine("  2 - AI Control");
    Console.WriteLine("  ESC - Exit");
    Console.WriteLine();

    var key = Console.ReadKey(true).Key;

    switch (key)
    {
        case ConsoleKey.D1: { await UserMode(); break; }
        //case ConsoleKey.D2: { await AImode(); break; }
        case ConsoleKey.Escape: { Console.WriteLine("Exit..."); break; }
    }
}




               //РАСЧЁТ УГЛОВ

//Получение углов серво по пикселям
(double yaw, double pitch) GetAnglesFromPixel(PointF pixel)
{
    // --- Матрица камеры ---
    double[,] K = new double[3, 3]
    {
                { 492.279154, 0.0, 337.929005 },
                { 0.0, 490.880685, 229.548998 },
                { 0.0, 0.0, 1.0 }
    };

    // --- Коэффициенты дисторсии ---
    double[] dist = { -0.241251, -1.747846, -0.012708, 0.014139, -18.669241,
                      -0.218984, -1.957205, -18.235285 };

    // Преобразуем в Emgu-структуры
    using (var cameraMatrix = new Matrix<double>(K))
    using (var distCoeffs = new Matrix<double>(dist.Length, 1))
    {
        for (int i = 0; i < dist.Length; i++)
            distCoeffs[i, 0] = dist[i];

        // Оборачиваем входную точку в VectorOfPointF
        using (var src = new Emgu.CV.Util.VectorOfPointF(new PointF[] { pixel }))
        using (var dst = new Emgu.CV.Util.VectorOfPointF())
        {
            // Корректируем пиксель с учётом дисторсии
            CvInvoke.UndistortPoints(src, dst, cameraMatrix, distCoeffs, null, cameraMatrix);

            // Получаем "распрямлённые" координаты
            PointF corrected = dst[0];

            // --- Вычисляем углы ---
            Matrix<double> Kinv = new Matrix<double>(3, 3);
            CvInvoke.Invert(cameraMatrix, Kinv, DecompMethod.LU);

            Matrix<double> uv1 = new Matrix<double>(new double[,] {
                { corrected.X },
                { corrected.Y },
                { 1.0 }
            });

            Matrix<double> xyz = new Matrix<double>(3, 1);
            CvInvoke.Gemm(Kinv, uv1, 1.0, null, 0.0, xyz);

            double x = xyz[0, 0] / xyz[2, 0];
            double y = xyz[1, 0] / xyz[2, 0];

            double angleXdeg = Math.Atan(x) * 180.0 / Math.PI; // yaw
            double angleYdeg = Math.Atan(y) * 180.0 / Math.PI; // pitch

            return (angleXdeg, angleYdeg);
        }
    }
}

//Перевод углов из системы координат камеры в систему координат серво
(double yawLaser, double pitchLaser) GetLaserAnglesFromCameraAngles(
double yawCamDeg,
double pitchCamDeg,
double[] laserOffset) // [x, y, z] в метрах
{
    // 1. Переводим углы камеры в радианы
    double yawCam = yawCamDeg * Math.PI / 180.0;
    double pitchCam = pitchCamDeg * Math.PI / 180.0;

    // 2. Получаем направляющий вектор в системе камеры
    double xCam = Math.Tan(yawCam);
    double yCam = Math.Tan(pitchCam);
    double zCam = 1.0; // направление вперёд

    // 3. Преобразуем в систему лазера (R = I, только смещение)
    double xLaser = xCam + laserOffset[0];
    double yLaser = yCam + laserOffset[1];
    double zLaser = zCam + laserOffset[2];

    // 4. Вычисляем углы для моторов лазера
    double yawLaser = Math.Atan2(xLaser, zLaser) * 180.0 / Math.PI;
    double pitchLaser = Math.Atan2(yLaser, zLaser) * 180.0 / Math.PI;

    return (yawLaser, pitchLaser);
}







            //РАБОТА С ПОРТОМ

//Открытие порта
void InitSerial()
{
    serial = new SerialPort("COM4", 115200);
    serial.NewLine = "\n";
    serial.Open();
    Thread.Sleep(2000); 
}

//Отправка данных на ардуино
void SendToArduino(string data)
{
    if (serial != null && serial.IsOpen)
    {
        serial.WriteLine(data);
        Console.WriteLine("отправлено: " + data);
    }
}




            //ПРОЧЕЕ

//Логирование информации в консоли
void Log(string message, ConsoleColor color = ConsoleColor.Green)
{
    var oldColor = Console.ForegroundColor;
    Console.ForegroundColor = color;
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
    Console.ForegroundColor = oldColor;
}