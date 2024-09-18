// docs: https://learn.microsoft.com/en-us/dotnet/core/diagnostics/diagnostics-client-library
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using System.Diagnostics.Tracing;

Console.WriteLine("Hello in EvenPipes and Diagnostics Client (Microsoft.Diagnostics.NETCore.Client).");

// Attach to a process and print out all GC events

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