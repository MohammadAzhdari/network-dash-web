using ConsoleApp_SystemUsage;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Text;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

#region Initialize
[DllImport("kernel32.dll")]
static extern bool GetProcessIoCounters(IntPtr ProcessHandle, out IO_COUNTERS IoCounters);

PerformanceCounter cpuCounter = new("Processor", "% Processor Time", "_Total");
PerformanceCounter ramCounter = new("Memory", "Available MBytes");

var db = new MonitorContext();
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
async Task GetCpuUsageForProcess()
{
    var result = cpuCounter.NextValue();

    Console.WriteLine($"{DateTime.Now} --- CPU: {result}%");

    await db.CpuMnts.AddAsync(new()
    {
        Id = Guid.NewGuid(),
        Percent = (int)Math.Floor(result + 0.5),
        CDate = DateTime.Now
    });
    await db.SaveChangesAsync();
}

async Task GetAvailableRAM()
{
    var result = ramCounter.NextValue();

    Console.WriteLine($"{DateTime.Now} --- RAM: {result}MB");

    await db.RamMnts.AddAsync(new()
    {
        Id = Guid.NewGuid(),
        MB = (int)Math.Floor(result + 0.5),
        CDate = DateTime.Now
    });
    await db.SaveChangesAsync();
}

async Task GetProcessDiskUsage()
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

    Thread.Sleep(500);

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

    var result = Math.Round(totalBytes2 - totalBytes1, 2);

    Console.WriteLine($"{DateTime.Now} --- I/O Disk Usage: {result} MB/s");

    await db.DiskMnts.AddAsync(new()
    {
        Id = Guid.NewGuid(),
        MBPerSecond = result,
        CDate = DateTime.Now
    });
    await db.SaveChangesAsync();
}

async Task GetNetworkUsage()
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

    var result = Math.Round(8 * (sendSum + receiveSum) / (bandwidth * numberOfIterations) * 100, 2);

    Console.WriteLine($"{DateTime.Now} --- Network Usage: {result} MB/s");

    await db.NetworkMnts.AddAsync(new()
    {
        Id = Guid.NewGuid(),
        MBPerSecond = result,
        CDate = DateTime.Now
    });
    await db.SaveChangesAsync();
}

#endregion

async void TraceCpu()
{

    Console.WriteLine("Thread Start");

    while (true)
    {
        GetProcessDiskUsage();
        GetCpuUsageForProcess();
        GetAvailableRAM();
        GetNetworkUsage();

        Thread.Sleep(500);
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