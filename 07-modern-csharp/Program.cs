// 1. Class vs record: print
var c1 = new StudentClass { RollNo = 1, Name = "Ali", Marks = 78 };
var r1 = new Student(1, "Ali", 78);
Console.WriteLine(c1);
Console.WriteLine(r1);

// 2. Equality
var c2 = new StudentClass { RollNo = 1, Name = "Ali", Marks = 78 };
var r2 = new Student(1, "Ali", 78);
Console.WriteLine($"class equal?  {c1 == c2}");
Console.WriteLine($"record equal? {r1 == r2}");

// 3. with
var improved = r1 with { Marks = 85 };
Console.WriteLine(r1);
Console.WriteLine(improved);
Console.WriteLine($"r1 == improved? {r1 == improved}");

// 4. Is line ko comment hata kar dekho (error ka text copy karna)
// r1.Marks = 90;


var teacher = new Teacher { Name = "Hamid", Experience = 5 };
Console.WriteLine($"{teacher.Name}, {teacher.Experience} years");



// 7. switch expression: grade
string GetGrade(int marks) => marks switch
{
    >= 90 => "A",
    >= 75 => "B",
    >= 50 => "C",
    _ => "F"
};

foreach (var m in new[] { 100, 90, 89, 75, 74, 50, 49, 0 })
    Console.WriteLine($"{m} -> {GetGrade(m)}");

// 8. or
string DayType(string day) => day switch
{
    "Saturday" or "Sunday" => "Weekend",
    _ => "Weekday"
};
Console.WriteLine(DayType("Sunday"));
Console.WriteLine(DayType("Monday"));

// 9. property pattern
string Describe(Student s) => s switch
{
    { Marks: >= 90 } => $"{s.Name}: Excellent",
    { Marks: >= 50 } => $"{s.Name}: Pass",
    _ => $"{s.Name}: Fail"
};
Console.WriteLine(Describe(new Student(1, "Ali", 95)));
Console.WriteLine(Describe(new Student(2, "Sara", 45)));

// 10. type pattern
string DescribeObject(object o) => o switch
{
    int n => $"int {n}",
    string text => $"string of length {text.Length}",
    null => "null",
    _ => "something else"
};
Console.WriteLine(DescribeObject(42));
Console.WriteLine(DescribeObject("hello"));
Console.WriteLine(DescribeObject(null));
Console.WriteLine(DescribeObject(3.14));


Console.ReadLine();
record Student(int RollNo, string Name, int Marks);

class StudentClass
{
    public int RollNo { get; set; }
    public string Name { get; set; } = "";
    public int Marks { get; set; }
}
class Teacher
{
    public required string Name { get; init; }
    public int Experience { get; init; }
};