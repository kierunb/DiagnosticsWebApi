using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using System.Diagnostics.Tracing;

namespace DiagnosticsClientDemo;

public class Tracer
{
    /// <summary>
    /// Trigger a CPU trace for given number of seconds
    /// </summary>
    /// <param name="processId"></param>
    /// <param name="duration"></param>
    /// <param name="traceName"></param>
    public void TraceProcessForDuration(int processId, int duration, string traceName)
    {
        var cpuProviders = new List<EventPipeProvider>()
        {
            new EventPipeProvider("Microsoft-Windows-DotNETRuntime", EventLevel.Informational, (long)ClrTraceEventParser.Keywords.Default),
            new EventPipeProvider("Microsoft-DotNETCore-SampleProfiler", EventLevel.Informational, (long)ClrTraceEventParser.Keywords.None)
        };
        var client = new DiagnosticsClient(processId);
        using (var traceSession = client.StartEventPipeSession(cpuProviders))
        {
            Task copyTask = Task.Run(async () =>
            {
                using FileStream fs = new FileStream(traceName, FileMode.Create, FileAccess.Write);
                await traceSession.EventStream.CopyToAsync(fs);
            });

            Task.WhenAny(copyTask, Task.Delay(TimeSpan.FromMilliseconds(duration * 1000)));
            traceSession.Stop();
        }
    }

    /// <summary>
    /// Parse events in real time
    /// </summary>
    /// <param name="processId"></param>
    public static void PrintEventsLive(int processId)
    {
        var providers = new List<EventPipeProvider>()
        {
            new EventPipeProvider("Microsoft-Windows-DotNETRuntime",
                EventLevel.Informational, (long)ClrTraceEventParser.Keywords.Default)
        };
        var client = new DiagnosticsClient(processId);
        using (var session = client.StartEventPipeSession(providers, false))
        {

            Task streamTask = Task.Run(() =>
            {
                var source = new EventPipeEventSource(session.EventStream);
                source.Clr.All += (TraceEvent obj) => Console.WriteLine(obj.EventName);
                try
                {
                    source.Process();
                }
                // NOTE: This exception does not currently exist. It is something that needs to be added to TraceEvent.
                catch (Exception e)
                {
                    Console.WriteLine("Error encountered while processing events");
                    Console.WriteLine(e.ToString());
                }
            });

            Task inputTask = Task.Run(() =>
            {
                Console.WriteLine("Press Enter to exit");
                while (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Task.Delay(TimeSpan.FromMilliseconds(100));
                }
                session.Stop();
            });

            Task.WaitAny(streamTask, inputTask);
        }
    }
}
