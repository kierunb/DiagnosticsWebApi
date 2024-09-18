using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using System.Diagnostics.Tracing;

namespace DiagnosticsClientDemo;

public class Dumper
{
    /// <summary>
    /// Write a core dump
    /// </summary>
    /// <param name="processId"></param>
    public static void TriggerCoreDump(int processId)
    {
        var client = new DiagnosticsClient(processId);
        client.WriteDump(DumpType.Normal, "/tmp/minidump.dmp");
    }

    /// <summary>
    /// Trigger a core dump when CPU usage goes above a threshold
    /// </summary>
    /// <param name="processId"></param>
    /// <param name="threshold"></param>
    public static void TriggerDumpOnCpuUsage(int processId, int threshold)
    {
        var providers = new List<EventPipeProvider>()
        {
            new EventPipeProvider(
                "System.Runtime",
                EventLevel.Informational,
                (long)ClrTraceEventParser.Keywords.None,
                new Dictionary<string, string>
                {
                    ["EventCounterIntervalSec"] = "1"
                }
            )
        };
        var client = new DiagnosticsClient(processId);
        using (var session = client.StartEventPipeSession(providers))
        {
            var source = new EventPipeEventSource(session.EventStream);
            source.Dynamic.All += (TraceEvent obj) =>
            {
                if (obj.EventName.Equals("EventCounters"))
                {
                    var payloadVal = (IDictionary<string, object>)(obj.PayloadValue(0));
                    var payloadFields = (IDictionary<string, object>)(payloadVal["Payload"]);
                    if (payloadFields["Name"].ToString().Equals("cpu-usage"))
                    {
                        double cpuUsage = Double.Parse(payloadFields["Mean"].ToString());
                        if (cpuUsage > (double)threshold)
                        {
                            client.WriteDump(DumpType.Normal, "/tmp/minidump.dmp");
                        }
                    }
                }
            };
            try
            {
                source.Process();
            }
            catch (Exception) { }
        }
    }
}
