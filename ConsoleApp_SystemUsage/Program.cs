using ConsoleApp_SystemUsage;
using System.ComponentModel;
using System.Diagnostics;


PerformanceCounter cpuCounter = new("Processor", "% Processor Time", "_Total");
PerformanceCounter ramCounter = new("Memory", "Available MBytes");

Console.WriteLine("Hello, World!");


var t = new Thread(TraceCpu);
t.IsBackground = true;
t.Start();

Console.WriteLine("Press Enter To Exit");
Console.ReadLine();


float GetCpuUsageForProcess() =>
     cpuCounter.NextValue();

float GetAvailableRAM() =>
    ramCounter.NextValue();

async void TraceCpu()
{
    var db = new MonitorContext();

    Console.WriteLine("Thread Start");

    while (true)
    {
        var cpu_usage = GetCpuUsageForProcess();
        var ram_usage = GetAvailableRAM();

        Console.WriteLine($"CPU: {DateTime.Now:yyyy-MM-dd HH:mm:ss} {cpu_usage}%");
        Console.WriteLine($"RAM: {DateTime.Now:yyyy-MM-dd HH:mm:ss} {ram_usage}MB");

        await db.CpuMnts.AddAsync(new()
        {
            Id = Guid.NewGuid(),
            Percent = (int)Math.Floor(cpu_usage + 0.5),
            CDate = DateTime.Now
        });
        await db.RamMnts.AddAsync(new()
        {
            Id = Guid.NewGuid(),
            MB = (int)Math.Floor(ram_usage + 0.5),
            CDate = DateTime.Now
        });

        await db.SaveChangesAsync();

        Thread.Sleep(2000);
    }
}
