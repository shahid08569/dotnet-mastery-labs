const int ThreadCount = 4;
const int Increments = 100_000;

void RunInThreads(Action work)
{
    var threads = new List<Thread>();
    for (int n = 0; n < ThreadCount; n++)
    {
        var thread = new Thread(() => work());
        threads.Add(thread);
        thread.Start();
    }
    foreach (var thread in threads) thread.Join();
}

int UnsafeCount()
{
    int counter = 0;
    RunInThreads(() =>
    {
        for (int i = 0; i < Increments; i++) counter++;
    });
    return counter;
}

int LockCount()
{
    int counter = 0;
    var gate = new object();
    RunInThreads(() =>
    {
        for (int i = 0; i < Increments; i++)
        {
            lock (gate) { counter++; }
        }
    });
    return counter;
}

int InterlockedCount()
{
    int counter = 0;
    RunInThreads(() =>
    {
        for (int i = 0; i < Increments; i++) Interlocked.Increment(ref counter);
    });
    return counter;
}

Console.WriteLine($"Expected:    {ThreadCount * Increments}");
for (int run = 1; run <= 3; run++)
    Console.WriteLine($"Unsafe run {run}: {UnsafeCount()}");
Console.WriteLine($"Lock:        {LockCount()}");
Console.WriteLine($"Interlocked: {InterlockedCount()}");
Console.ReadLine();