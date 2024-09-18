using Microsoft.Diagnostics.NETCore.Client;
using System.Diagnostics;

namespace DiagnosticsClientDemo;

public class ProcessTracker
{
    /// <summary>
    /// Print names of processes that published a diagnostics channel
    /// </summary>
    public static void PrintProcessStatus()
    {
        var processes = DiagnosticsClient.GetPublishedProcesses()
            .Select(Process.GetProcessById)
            .Where(process => process != null);

        foreach (var process in processes)
        {
            Console.WriteLine($"{process.ProcessName}");
        }
    }
}
