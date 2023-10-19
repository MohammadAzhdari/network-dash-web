using ConsoleApp_SystemUsage;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

#region Initialize
[DllImport("kernel32.dll")]
static extern bool GetProcessIoCounters(IntPtr ProcessHandle, out IO_COUNTERS IoCounters);

PerformanceCounter cpuCounter = new("Processor", "% Processor Time", "_Total");
PerformanceCounter ramCounter = new("Memory", "Available MBytes");
#endregion

#region Start Thread
Console.WriteLine("Hello, World!");

var t = new Thread(TraceCpu)
{
    IsBackground = true
};
t.Start();

Console.WriteLine("Press Enter To Exit");
Console.ReadLine();
#endregion

#region Methods
float GetCpuUsageForProcess() =>
     cpuCounter.NextValue();

float GetAvailableRAM() =>
    ramCounter.NextValue();

double GetProcessDiskUsage()
{
    Process[] process = Process.GetProcesses();

    double totalBytes1 = 0;
    foreach (var prss in process)
    {
        try
        {
            IO_COUNTERS ioC1 = new IO_COUNTERS();
            GetProcessIoCounters(prss.Handle, out ioC1);
            totalBytes1 += (ioC1.ReadTransferCount + ioC1.WriteTransferCount) / 1024f / 1024f;
        }
        catch
        {

        }
    }

    Thread.Sleep(1000);

    double totalBytes2 = 0;
    foreach (var prss in process)
    {
        try
        {
            IO_COUNTERS ioC2 = new IO_COUNTERS();
            GetProcessIoCounters(prss.Handle, out ioC2);
            totalBytes2 += (ioC2.ReadTransferCount + ioC2.WriteTransferCount) / 1024f / 1024f;
        }
        catch
        {

        }
    }

    return Math.Round(totalBytes2 - totalBytes1, 2);
}

double GetNetworkUsage()
{
    const int numberOfIterations = 10;

    PerformanceCounterCategory pcg = new PerformanceCounterCategory("Network Interface");
    string instance = pcg.GetInstanceNames()[0];
    var bandwidthCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", instance);
    var dataSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
    var dataReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);

    float sendSum = 0;
    float receiveSum = 0;
    float bandwidth = 0;
    try
    {
        for (var index = 0; index < numberOfIterations; index++)
        {
            sendSum += dataSentCounter.NextValue();
            receiveSum += dataReceivedCounter.NextValue();
        }
        bandwidth = bandwidthCounter.NextValue();
    }
    catch
    {

    }

    return Math.Round(8 * (sendSum + receiveSum) / (bandwidth * numberOfIterations) * 100, 2);
}

#endregion

async void TraceCpu()
{
    var db = new MonitorContext();

    Console.WriteLine("Thread Start");

    while (true)
    {
        var disk_usage = GetProcessDiskUsage();
        var cpu_usage = GetCpuUsageForProcess();
        var ram_usage = GetAvailableRAM();
        var network_usage = GetNetworkUsage();

        Console.WriteLine($"\r\n{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"CPU: {cpu_usage}%");
        Console.WriteLine($"RAM: {ram_usage}MB");
        Console.WriteLine($"I/O Disk Usage: {disk_usage} MB/s");
        Console.WriteLine($"Network Usage: {network_usage} MB/s");
        Console.WriteLine("----------------------------");

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
        await db.DiskMnts.AddAsync(new()
        {
            Id = Guid.NewGuid(),
            MBPerSecond = disk_usage,
            CDate = DateTime.Now
        });
        await db.NetworkMnts.AddAsync(new()
        {
            Id = Guid.NewGuid(),
            MBPerSecond = network_usage,
            CDate = DateTime.Now
        });

        await db.SaveChangesAsync();

        Thread.Sleep(1000);
    }
}

struct IO_COUNTERS
{
    public ulong ReadOperationCount;
    public ulong WriteOperationCount;
    public ulong OtherOperationCount;
    public ulong ReadTransferCount;
    public ulong WriteTransferCount;
    public ulong OtherTransferCount;
}