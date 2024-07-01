// docs: https://learn.microsoft.com/en-us/dotnet/core/diagnostics/diagnostics-client-library
using Microsoft.Diagnostics.NETCore.Client;
using System.Diagnostics.Tracing;
using System.Diagnostics;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing;

Console.WriteLine("Hello in Diagnostics Client (Microsoft.Diagnostics.NETCore.Client).");

// PROVIDE VALID PROCESS ID
int processPid = 41108;

var providers = new List<EventPipeProvider>()
        {
            new EventPipeProvider("Microsoft-Windows-DotNETRuntime",
                EventLevel.Informational, (long)ClrTraceEventParser.Keywords.GC)
        };

var client = new DiagnosticsClient(processPid);

using (EventPipeSession session = client.StartEventPipeSession(providers, false))
{
    var source = new EventPipeEventSource(session.EventStream);

    source.Clr.All += (TraceEvent obj) => Console.WriteLine(obj.ToString());

    try
    {
        source.Process();
    }
    catch (Exception e)
    {
        Console.WriteLine("Error encountered while processing events");
        Console.WriteLine(e.ToString());
    }
}
