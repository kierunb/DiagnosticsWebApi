using System.Diagnostics.Tracing;

Console.WriteLine("Hello, EventSource!");

DemoEventSource.Log.AppStarted("Hello World!", 12);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(500);
    DemoEventSource.Log.Ping($"Ping {i}");
}

Console.WriteLine("Done!");
Console.ReadKey();


[EventSource(Name = "Demo")]
class DemoEventSource : EventSource
{
    public static DemoEventSource Log { get; } = new DemoEventSource();

    [Event(1)]
    public void AppStarted(string message, int favoriteNumber) => WriteEvent(1, message, favoriteNumber);

    [Event(2)]
    public void Ping(string message) => WriteEvent(2, message);
}

// dotnet-trace collect --providers Demo -- EventSourceDemo.exe