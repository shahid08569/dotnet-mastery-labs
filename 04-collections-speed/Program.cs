using System.Diagnostics;

var products = Enumerable.Range(1, 100_000)
    .Select(i => new Product(i, $"Product {i}"))
    .ToList();

// Prepare collections
var productsById = products.ToDictionary(p => p.Id);
var idList = products.Select(p => p.Id).ToList();

var idSet = idList.ToHashSet();

// IDs to search
var wantedIds = Enumerable.Range(99_000, 1_000).ToList();


foreach (var id in wantedIds)
{
    _ = products.First(p => p.Id == id); //check id's one by one O(n);
    _ = productsById.TryGetValue(id, out var product); // direct jo wanted id h us ka value aur product nikalty hain O(1), Dictionary retreive corresponding product to key
    _ = idList.Contains(id);// just checking id is exist in list koyi product retrieve nahi kariengy..
    _ = idSet.Contains(id);
}

// Measure List.First()
var sw = Stopwatch.StartNew();
foreach (var id in wantedIds)
{
    products.First(p => p.Id == id);
}
sw.Stop();
Console.WriteLine($"List.First()         : {sw.Elapsed.TotalMilliseconds:F3} ms");

// Measure Dictionary lookup
sw.Restart();
foreach (var id in wantedIds)
{
    _ = productsById.TryGetValue(id, out var product);
}
sw.Stop();
Console.WriteLine($"List.First()         : {sw.Elapsed.TotalMilliseconds:F3} ms");

// Measure List.Contains()

sw.Restart();

foreach (var id in wantedIds)
{
    _ = idList.Contains(id);
}

sw.Stop();
Console.WriteLine($"List.First()         : {sw.Elapsed.TotalMilliseconds:F3} ms");

//  Measure HashSet.Contains()
sw.Restart();

foreach (var id in wantedIds)
{
    _ = idSet.Contains(id);
}

sw.Stop();
Console.WriteLine($"List.First()         : {sw.Elapsed.TotalMilliseconds:F3} ms");

//Console.WriteLine(idSet.Contains(50000));
Console.ReadLine();

record Product(int Id, string Name);



//var productsById = products.ToDictionary(p => p.Id);
//var idList = products.Select(p => p.Id).ToList();
//var idSet = idList.ToHashSet();

//var wantedIds = Enumerable.Range(99_000, 1_000).ToList();   // 1,000 ids jo list ke aakhir ke paas hain

//var sw = Stopwatch.StartNew();
//foreach (var id in wantedIds) _ = products.First(p => p.Id == id);
//Console.WriteLine($"List.First         : {sw.Elapsed.TotalMilliseconds:F3} ms");

//sw.Restart();
//foreach (var id in wantedIds) _ = productsById[id];
//Console.WriteLine($"Dictionary lookup  : {sw.Elapsed.TotalMilliseconds:F3} ms");

//sw.Restart();
//foreach (var id in wantedIds) _ = idList.Contains(id);
//Console.WriteLine($"List<int>.Contains : {sw.Elapsed.TotalMilliseconds:F3} ms");

//sw.Restart();
//foreach (var id in wantedIds) _ = idSet.Contains(id);
//Console.WriteLine($"HashSet.Contains   : {sw.Elapsed.TotalMilliseconds:F3} ms");
