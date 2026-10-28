        using System.Collections.Concurrent;
        using System.Diagnostics;

        void WithLock()
        {
            int counter = 0;
            var gate = new object();                       // chaabi
    
            var waiter1 = new Thread(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    lock (gate) { counter++; }             // chaabi lo, jodo, chaabi wapas
                }
            });
            var waiter2 = new Thread(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    lock (gate) { counter++; }
                }
            });

            waiter1.Start(); waiter2.Start();
            waiter1.Join(); waiter2.Join();

            Console.WriteLine($"lock ke saath: {counter} (chahiye 200000)");
        }

        void WithInterlocked()
        {
            int counter = 0;

            var waiter1 = new Thread(() =>
            {
                for (int i = 0; i < 100000; i++) Interlocked.Increment(ref counter);
            });
            var waiter2 = new Thread(() =>
            {
                for (int i = 0; i < 100000; i++) Interlocked.Increment(ref counter);
            });

            waiter1.Start(); waiter2.Start();
            waiter1.Join(); waiter2.Join();

            Console.WriteLine($"Interlocked ke saath: {counter} (chahiye 200000)");
        }

        void DeadlockDemo(bool fixIt)
        {
            var pen = new object();
            var copy = new object();

            var ali = new Thread(() =>
            {
                lock (pen)                                         // Ali: pehle pen
                {
                    Console.WriteLine("Ali: pen liya");
                    Thread.Sleep(100);
                    Console.WriteLine("Ali: copy ka intezar...");
                    lock (copy) Console.WriteLine("Ali: likh liya");
                }
            })
            { IsBackground = true };

            var sara = new Thread(() =>
            {
                var first = fixIt ? pen : copy;                  // fixIt = false: Sara pehle copy (ULTI tarteeb)
                var second = fixIt ? copy : pen;                   // fixIt = true:  Sara bhi pehle pen (SAME tarteeb)
                lock (first)
                {
                    Console.WriteLine("Sara: pehli cheez li");
                    Thread.Sleep(100);
                    Console.WriteLine("Sara: doosri cheez ka intezar...");
                    lock (second) Console.WriteLine("Sara: likh liya");
                }
            })
            { IsBackground = true };

            ali.Start();
            sara.Start();

            bool done = ali.Join(3000) && sara.Join(3000);
            Console.WriteLine(done ? "Dono khatam" : "DEADLOCK! 3 second baad bhi atke hue hain");
        }

void ThreadPoolDemo()
{
    var threadIds = new ConcurrentBag<int>();

    var tasks = Enumerable.Range(1, 100)
        .Select(n => Task.Run(() => threadIds.Add(Environment.CurrentManagedThreadId)))
        .ToArray();

    Task.WaitAll(tasks);

    Console.WriteLine($"Kaam: 100, bag mein items: {threadIds.Count}");
    Console.WriteLine($"Alag alag threads jo istemal hue: {threadIds.Distinct().Count()}");
}

void SemaphoreDemo()
{
    var parking = new SemaphoreSlim(3);                // sirf 3 jagah
    int inside = 0;
    var sw = Stopwatch.StartNew();

    var tasks = Enumerable.Range(1, 8)
        .Select(n => Task.Run(() =>
        {
            parking.Wait();                            // jagah chahiye (na ho to ruko)
            try
            {
                var now = Interlocked.Increment(ref inside);
                Console.WriteLine($"Gaari {n} andar. Abhi andar: {now}");
                Thread.Sleep(500);
                Interlocked.Decrement(ref inside);
                Console.WriteLine($"Gaari {n} bahar");
            }
            finally
            {
                parking.Release();                     // jagah wapas
            }
        }))
        .ToArray();

    Task.WaitAll(tasks);
    Console.WriteLine($"Sab khatam. Waqt: {sw.ElapsedMilliseconds} ms");
}

void RaceCondition()
{
    int counter = 0;

    var waiter1 = new Thread(() => { for (int i = 0; i < 100000; i++) counter++; });
    var waiter2 = new Thread(() => { for (int i = 0; i < 100000; i++) counter++; });

    waiter1.Start(); waiter2.Start();
    waiter1.Join(); waiter2.Join();

    Console.WriteLine($"Bina lock: {counter} (chahiye 200000)");
}

Console.WriteLine("=== Bina lock (3 baar) ===");
for (int run = 1; run <= 3; run++) RaceCondition();

Console.WriteLine();
WithLock();
WithInterlocked();

Console.WriteLine();
Console.WriteLine("=== Deadlock (ulti tarteeb) ===");
DeadlockDemo(fixIt: false);

Console.WriteLine();
Console.WriteLine("=== Deadlock (same tarteeb) ===");
DeadlockDemo(fixIt: true);

Console.WriteLine();
ThreadPoolDemo();

Console.WriteLine();
SemaphoreDemo();
Console.ReadLine();