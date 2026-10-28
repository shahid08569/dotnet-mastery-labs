using System.Collections.Concurrent;

// ===== Section 1: Thread pool =====
var threadIds = new ConcurrentBag<int>();

var tasks = Enumerable.Range(1, 100)
    .Select(n => Task.Run(() => threadIds.Add(Environment.CurrentManagedThreadId)))
    .ToArray();

Task.WaitAll(tasks);

Console.WriteLine($"Kaam: 100, items bag mein: {threadIds.Count}");
Console.WriteLine($"Alag alag threads jo istemal hue: {threadIds.Distinct().Count()}");

// ===== Section 2: Deadlock =====
bool RunLocks(bool sameOrder)
{
    var lockA = new object();
    var lockB = new object();

    var t1 = new Thread(() => TakeBoth(lockA, lockB, "T1")) { IsBackground = true };
    var t2 = sameOrder
        ? new Thread(() => TakeBoth(lockA, lockB, "T2")) { IsBackground = true }
        : new Thread(() => TakeBoth(lockB, lockA, "T2")) { IsBackground = true };

    t1.Start();
    t2.Start();

    return t1.Join(3000) && t2.Join(3000);      // 3 second tak intezar
}

void TakeBoth(object first, object second, string name)
{
    lock (first)
    {
        Console.WriteLine($"{name}: pehla lock liya");
        Thread.Sleep(100);
        Console.WriteLine($"{name}: doosre lock ka intezar...");
        lock (second)
        {
            Console.WriteLine($"{name}: dono mil gaye");
        }
    }
}

Console.WriteLine();
Console.WriteLine("--- Ulti tarteeb ---");
Console.WriteLine(RunLocks(sameOrder: false) ? "Khatam" : "DEADLOCK: 3 second baad bhi atke hue hain");

Console.WriteLine();
Console.WriteLine("--- Ek hi tarteeb ---");
Console.WriteLine(RunLocks(sameOrder: true) ? "Khatam" : "DEADLOCK");

// ===== Section 3: SemaphoreSlim =====
var semaphore = new SemaphoreSlim(3);       // ek waqt mein max 3
int active = 0;

var work = Enumerable.Range(1, 8)
    .Select(n => Task.Run(() =>
    {
        semaphore.Wait();
        try
        {
            var now = Interlocked.Increment(ref active);
            Console.WriteLine($"Kaam {n} shuru, abhi chal rahe: {now}");
            Thread.Sleep(500);
            Interlocked.Decrement(ref active);
            Console.WriteLine($"Kaam {n} khatam");
        }
        finally
        {
            semaphore.Release();
        }
    }))
    .ToArray();

Task.WaitAll(work);
Console.WriteLine("Sab khatam");