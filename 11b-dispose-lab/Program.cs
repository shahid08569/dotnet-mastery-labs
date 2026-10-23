// 1. Apni class jo IDisposable hai (mechanism dekhne ke liye)
using (var a = new Resource("A"))
{
    Console.WriteLine("A ka kaam chal raha hai");
}
Console.WriteLine("--- A ka block khatam ---");

// 2. Error ke bawajood Dispose chalta hai?
try
{
    using var b = new Resource("B");
    Console.WriteLine("B ka kaam chal raha hai");
    throw new Exception("kuch masla ho gaya!");
}
catch (Exception ex)
{
    Console.WriteLine($"Pakda: {ex.Message}");
}

// 3. File: Dispose se pehle aur baad
var writer = new StreamWriter("test1.txt");
writer.WriteLine("hello");

try
{
    Console.WriteLine(File.ReadAllText("test1.txt"));
}
catch (IOException ex)
{
    Console.WriteLine($"File error: {ex.GetType().Name}");
}

writer.Dispose();
Console.WriteLine($"Dispose ke baad: {File.ReadAllText("test1.txt").Trim()}");

Console.ReadLine();
class Resource : IDisposable
{
    private readonly string _name;

    public Resource(string name)
    {
        _name = name;
        Console.WriteLine($"{_name} khola");
    }

    public void Dispose()
    {
        Console.WriteLine($"{_name} band (Dispose)");
    }
}