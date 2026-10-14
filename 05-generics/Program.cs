// 1. Bina generics: copy paste
void PrintInt(int value) => Console.WriteLine($"Int: {value}");
void PrintString(string value) => Console.WriteLine($"String: {value}");

PrintInt(5);
PrintString("hello");

// 2. Generic method: ek method, har type
void Print<T>(T value) => Console.WriteLine($"Value: {value}");

Print(5);
Print("hello");
Print(3.14);
Print(true);

// 3. Swap
void Swap<T>(ref T a, ref T b)
{
    T temp = a;
    a = b;
    b = temp;
}

int num1 = 1, num2 = 2;
Swap(ref num1, ref num2);
Console.WriteLine($"num1={num1}, num2={num2}");

string name1 = "Ali", name2 = "Sara";
Swap(ref name1, ref name2);
Console.WriteLine($"name1={name1}, name2={name2}");
// 4. Generic class
var intBox = new Box<int>(42);
var stringBox = new Box<string>("hello");
Console.WriteLine(intBox.Get());
Console.WriteLine(stringBox.Get());

// 5. Do placeholders
var person = new Pair<string, int>("Ali", 25);
Console.WriteLine($"{person.First} is {person.Second}");

// 6. Constraint
T Max<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) >= 0 ? a : b;
Console.WriteLine(Max(3, 7));
Console.WriteLine(Max("apple", "banana"));

// 7. Generic MyWhere (Lesson 3 wala, ab har type ke liye)
List<T> MyWhere<T>(List<T> source, Func<T, bool> rule)
{
    var result = new List<T>();
    foreach (var item in source)
    {
        if (rule(item)) result.Add(item);
    }
    return result;
}

var numbers = new List<int> { 1, 2, 3, 4, 5 };
var words = new List<string> { "apple", "fig", "banana", "kiwi" };
var people = new List<Person> { new("Ali", 30), new("Sara", 17), new("Omar", 22) };

Console.WriteLine(string.Join(",", MyWhere(numbers, n => n > 3)));
Console.WriteLine(string.Join(",", MyWhere(words, w => w.Length > 4)));
Console.WriteLine(string.Join(",", MyWhere(people, p => p.Age >= 18).Select(p => p.Name)));

// 8. object ka bug (ye aakhri line crash karegi)
var mixed = new List<object> { 1, "hello" };
if (mixed[1] is int number)
    Console.WriteLine(number);
else
    Console.WriteLine("ye int nahi hai");

Console.ReadLine();
record Person(string Name, int Age);

class Box<T>
{
    private T _item;
    public Box(T item) { _item = item; }
    public T Get() => _item;
}

class Pair<TFirst, TSecond>
{
    public TFirst First { get; }
    public TSecond Second { get; }
    public Pair(TFirst first, TSecond second) { First = first; Second = second; }
}
