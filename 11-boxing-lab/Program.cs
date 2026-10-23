// 1. Boxing aur unboxing
int i = 123;
object o = i;
int j = (int)o;
i = 456;
Console.WriteLine($"i={i}, o={o}, j={j}");


//2. wrong unboxing
try
{
    long wrong = (long)o;
    Console.WriteLine(wrong);
}
catch (InvalidCastException ex)
{
    Console.WriteLine($"Error: {ex.GetType().Name}");
}

// 3. List<object> vs List<int>: heap?
const int count = 1_000_000;

long before = GC.GetAllocatedBytesForCurrentThread();
var boxedList = new List<object>();
for (int k = 0; k < count; k++) boxedList.Add(k);
long boxedBytes = GC.GetAllocatedBytesForCurrentThread() - before;

before = GC.GetAllocatedBytesForCurrentThread();
var plainList = new List<int>();
for (int k = 0; k < count; k++) plainList.Add(k);
long plainBytes = GC.GetAllocatedBytesForCurrentThread() - before;

Console.WriteLine($"List<object>: {boxedBytes / 1024 / 1024} MB");
Console.WriteLine($"List<int>:    {plainBytes / 1024 / 1024} MB");

Console.ReadLine();