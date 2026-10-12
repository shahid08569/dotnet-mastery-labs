var numbers = new List <int> { 1, 2, 3, 4, 5 };

//1. func: input int,ouput bool
Func<int, bool> isEven = n => n % 2 == 0;
//static bool isEven(int n)
//{
//    return n % 2 == 0;
//}
Console.WriteLine(isEven(4));
Console.WriteLine(isEven(5));

//2. Action : no return value

Action<string> say = msg => Console.WriteLine($"Message:{msg}");
say("Hello");

//3. Where 

List<int> MyWhere(List<int> source, Func <int, bool> rule)
{
    var result = new List<int>();
    foreach (var item in source)
    {
        if (rule(item))
        {
            result.Add(item);
        }

    }
    return result;
}

var evens = MyWhere(numbers, isEven);
var bigOnes = MyWhere(numbers, n => n > 3);

Console.WriteLine(string.Join(",", evens));
Console.WriteLine(string.Join(",", bigOnes));

Console.ReadLine();
