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

Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
Thread.CurrentThread.Priority = ThreadPriority.Highest;

Stopwatch timer = new Stopwatch(); int frameCount = 0;

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
        Console.ForegroundColor = ConsoleColor.Green;
        var first = results.First();
        Console.WriteLine($"Detected {results.Count}, first: {first.Label.Name}, confidence: {first.Confidence:F2}");
        Console.ResetColor();
    }
};

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("======================================");
Console.WriteLine("        Welcome to Object Detector     ");
Console.WriteLine("======================================");
Console.ResetColor();
Console.WriteLine("Controls:");
Console.WriteLine("  W - Start detector");
Console.WriteLine("  S - Stop detector");
Console.WriteLine("  Esc - Exit");
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
        Log("Exiting...", ConsoleColor.Red);
        break;
    }
}

void Log(string message, ConsoleColor color = ConsoleColor.Green)
{
    var oldColor = Console.ForegroundColor;
    Console.ForegroundColor = color;
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
    Console.ForegroundColor = oldColor;
}



