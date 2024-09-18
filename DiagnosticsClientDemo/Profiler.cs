using Microsoft.Diagnostics.NETCore.Client;

namespace DiagnosticsClientDemo;

public class Profiler
{
    /// <summary>
    /// Attach an ICorProfiler profiler to a process
    /// </summary>
    /// <param name="processId"></param>
    /// <param name="profilerGuid"></param>
    /// <param name="profilerPath"></param>
    public static void AttachProfiler(int processId, Guid profilerGuid, string profilerPath)
    {
        var client = new DiagnosticsClient(processId);
        client.AttachProfiler(TimeSpan.FromSeconds(10), profilerGuid, profilerPath);
    }
}
