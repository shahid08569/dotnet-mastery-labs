// Lab 01: LINQ deferred execution
// Version A: bina ToList() (deferred)
// Version B: ToList() ke saath (immediate)

Console.WriteLine("=== Version A: deferred ===");

var numbersA = new List<int> { 1, 2, 3 };

var queryA = numbersA.Where(n =>
{
    Console.WriteLine($"checking {n}");
    return n > 1;
});

Console.WriteLine("query created");
numbersA.Add(4);

foreach (var n in queryA) Console.WriteLine($"got {n}");

Console.WriteLine("--- second time ---");
Console.WriteLine(queryA.Count());

Console.WriteLine();
Console.WriteLine("=== Version B: with ToList() ===");

var numbersB = new List<int> { 1, 2, 3 };

var filteredB = numbersB.Where(n =>
{
    Console.WriteLine($"checking {n}");
    return n > 1;
}).ToList();

Console.WriteLine("query created");
numbersB.Add(4);

foreach (var n in filteredB) Console.WriteLine($"got {n}");

Console.WriteLine("--- second time ---");
Console.WriteLine(filteredB.Count);