using System.Collections.Concurrent;
using System.Diagnostics;

async Task<int> GetNumberAsync()
{
Console.WriteLine($"  GetNumber shuru, thread {Environment.CurrentManagedThreadId}");
await Task.Delay(1000);
Console.WriteLine($"  GetNumber wapas, thread {Environment.CurrentManagedThreadId}");
return 42;
}

async Task<string> MakeChaiAsync(string customer, int ms)
{
await Task.Delay(ms);
return $"{customer} ki chai";
}

async Task FailAsync()
{
await Task.Delay(100);
throw new InvalidOperationException("Chai ka doodh khatam!");
}

async Task<int> GetValueAsync(bool configureAwaitFalse)
{
if (configureAwaitFalse)
await Task.Delay(100).ConfigureAwait(false);
else
await Task.Delay(100);
return 42;
}

void DeadlockDemo(bool configureAwaitFalse)
{
var context = new SingleThreadContext();
var thread = new Thread(() => context.Run(() =>
{
Console.WriteLine("  Context wale thread par: .Result se pehle");
var value = GetValueAsync(configureAwaitFalse).Result;
Console.WriteLine($"  .Result ke baad mila: {value}");
context.Stop();
}))
{ IsBackground = true };

thread.Start();
bool finished = thread.Join(3000);
Console.WriteLine(finished ? "  Khatam" : "  DEADLOCK: 3 second baad bhi atka hua");
}

// ===== Calls =====
Console.WriteLine("=== 1. Pehla async method ===");
Console.WriteLine($"Main, thread {Environment.CurrentManagedThreadId}");
var number = await GetNumberAsync();
Console.WriteLine($"Jawab: {number}");

Console.WriteLine();
Console.WriteLine("=== 2. Ek ke baad ek ===");
var sw = Stopwatch.StartNew();
var a = await MakeChaiAsync("Ali", 1000);
var b = await MakeChaiAsync("Sara", 1000);
var c = await MakeChaiAsync("Omar", 1000);
Console.WriteLine($"{sw.ElapsedMilliseconds} ms ({a}, {b}, {c})");

Console.WriteLine();
Console.WriteLine("=== 3. Ek saath (WhenAll) ===");
sw.Restart();
var t1 = MakeChaiAsync("Ali", 1000);
var t2 = MakeChaiAsync("Sara", 1000);
var t3 = MakeChaiAsync("Omar", 1000);
var results = await Task.WhenAll(t1, t2, t3);
Console.WriteLine($"{sw.ElapsedMilliseconds} ms ({string.Join(", ", results)})");

Console.WriteLine();
Console.WriteLine("=== 4. Error ===");
try
{
await FailAsync();
}
catch (InvalidOperationException ex)
{
Console.WriteLine($"Pakda: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("=== 5. Cancel ===");
using (var cts = new CancellationTokenSource())
{
cts.CancelAfter(500);
try
{
Console.WriteLine("Lamba kaam shuru (5 second)...");
await Task.Delay(5000, cts.Token);
Console.WriteLine("Kaam khatam");
}
catch (OperationCanceledException)
{
Console.WriteLine("Kaam cancel ho gaya (0.5 second baad)");
}
}

Console.WriteLine();
Console.WriteLine("=== 6. Console mein .Result (koi context nahi) ===");
var blocked = GetValueAsync(false).Result;
Console.WriteLine($".Result ke baad mila: {blocked} (deadlock nahi, lekin thread bekaar bandh raha)");

Console.WriteLine();
Console.WriteLine("=== 7. Single-thread context par .Result (DEADLOCK) ===");
DeadlockDemo(configureAwaitFalse: false);

Console.WriteLine();
Console.WriteLine("=== 8. Wahi, lekin ConfigureAwait(false) ke saath ===");
DeadlockDemo(configureAwaitFalse: true);

// Ek chhota "kursi" jo ek waqt mein ek hi kaam chalata hai (UI thread jaisa)
class SingleThreadContext : SynchronizationContext
{
    private readonly BlockingCollection<(SendOrPostCallback Callback, object? State)> _queue = new();

    public override void Post(SendOrPostCallback d, object? state) => _queue.Add((d, state));

    public void Run(Action firstWork)
    {
        SetSynchronizationContext(this);
        Post(_ => firstWork(), null);
        foreach (var (callback, state) in _queue.GetConsumingEnumerable())
            callback(state);
    }

    public void Stop() => _queue.CompleteAdding();
}
Console.ReadLine();