// 1. Naya object kis generation mein hai, aur safai ke baad?
var obj = new object();
Console.WriteLine($"Naya object:             Gen {GC.GetGeneration(obj)}");

GC.Collect();
Console.WriteLine($"1 collection ke baad:     Gen {GC.GetGeneration(obj)}");

GC.Collect();
Console.WriteLine($"2 collections ke baad:    Gen {GC.GetGeneration(obj)}");

GC.Collect();
Console.WriteLine($"3 collections ke baad:    Gen {GC.GetGeneration(obj)}");

// 2. Chhota array vs bara array
var small = new byte[1_000];
var large = new byte[100_000];
Console.WriteLine($"Chhota array (1,000 bytes):   Gen {GC.GetGeneration(small)}");
Console.WriteLine($"Bara array (100,000 bytes):   Gen {GC.GetGeneration(large)}");

// 3. Kachra kab banta hai? (WeakReference: sirf dekhne ke liye pata, object ko zinda nahi rakhta)
var p1 = new Person { Name = "Ali" };
var p2 = p1;
var weak = new WeakReference(p1);

p1 = null;
GC.Collect();
Console.WriteLine($"p1 = null ke baad zinda?  {weak.IsAlive}");

p2 = null;
GC.Collect();
Console.WriteLine($"p2 = null ke baad zinda?  {weak.IsAlive}");

// 4. Gen 0 ki safai kitni baar hui?
var keep = new int[100][];
Console.WriteLine($"Gen 0 collections shuru mein: {GC.CollectionCount(0)}");

for (int i = 0; i < 1_000_000; i++)
{
    keep[i % 100] = new int[10];       // chhote objects, zyada tar jaldi kachra
}

Console.WriteLine($"Gen 0 collections baad mein:  {GC.CollectionCount(0)}");

Console.ReadLine();
class Person
{
    public string Name { get; set; } = "";
}