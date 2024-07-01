// docs: https://github.com/microsoft/clrmd
// samples: https://github.com/microsoft/clrmd/tree/main/src/Samples

using Microsoft.Diagnostics.Runtime;

Console.WriteLine("Hello in ClrMD (Microsoft.Diagnostics.Runtime).");

// PROVIDE VALID PATH TO DUMP FILE
string dumpPath = @"D:\1dumps\DiagnosticsWebApi\memory-leak\dump_20240530_211809_baseline.dmp";

Dictionary<ulong, (int Count, ulong Size, string Name)> stats = new();

using DataTarget dataTarget = DataTarget.LoadDump(dumpPath); //args[0]
foreach (ClrInfo clr in dataTarget.ClrVersions)
{
    using ClrRuntime runtime = clr.CreateRuntime();
    ClrHeap heap = runtime.Heap;

    Console.WriteLine("{0,16} {1,16} {2,8} {3}", "Object", "MethodTable", "Size", "Type");
    foreach (ClrObject obj in heap.EnumerateObjects())
    {
        Console.WriteLine($"{obj.Address:x16} {obj.Type.MethodTable:x16} {obj.Size,8:D} {obj.Type.Name}");

        if (!stats.TryGetValue(obj.Type.MethodTable, out (int Count, ulong Size, string Name) item))
            item = (0, 0, obj.Type.Name);

        stats[obj.Type.MethodTable] = (item.Count + 1, item.Size + obj.Size, item.Name);
    }

    Console.WriteLine("\nStatistics:");
    var sorted = from i in stats
                 orderby i.Value.Size ascending
                 select new
                 {
                     i.Key,
                     i.Value.Name,
                     i.Value.Size,
                     i.Value.Count
                 };

    Console.WriteLine("{0,16} {1,12} {2,12}\t{3}", "MethodTable", "Count", "Size", "Type");
    foreach (var item in sorted)
        Console.WriteLine($"{item.Key:x16} {item.Count,12:D} {item.Size,12:D}\t{item.Name}");

    Console.WriteLine($"Total {sorted.Sum(x => x.Count):0} objects");
}
