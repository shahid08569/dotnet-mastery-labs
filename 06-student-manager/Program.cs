
var students = new List<Student>
{
    new(1, "Ali", 78),
    new(2, "Sara", 45),
    new(3, "Omar", 91),
    new(4, "Hina", 62),
    new(5, "Zain", 33),
};

Console.WriteLine("All students:");
foreach (var s in students)
    Console.WriteLine($"{s.RollNo} - {s.Name}: {s.Marks}");



//var result=students.Where(student =>
//{
//    //Console.WriteLine(n);
//   return student.Marks > 50;
//}).ToList();


//foreach (var marks in result)
//{
//    Console.WriteLine(marks);
//}

//Task 2
var passedStd = Helper.myWhere(students, student => student.Marks >= 50);
foreach (var marks in passedStd)
{
    Console.WriteLine(marks);
}

//Task 3
var studentsByRoll = students.ToDictionary(student => student.RollNo);
if (studentsByRoll.TryGetValue(3, out var student99))
{
    Console.WriteLine($"Found: {student99.Name}");
}
else
{
    Console.WriteLine("Not Found");
}

//Task 4:
var rollNumbers = students.Select(student => student.RollNo).ToHashSet();

void AddStudents(Student newStd)
{
    if(rollNumbers.Contains(newStd.RollNo))
    {
        Console.WriteLine($"Student with Roll No {newStd.RollNo} already exists.");
        return;
    }
    students.Add(newStd);
    rollNumbers.Add(newStd.RollNo);
    Console.WriteLine($"Added: {newStd.Name}");
}
AddStudents(new Student(6, "Ayesha", 88));
AddStudents(new Student(3, "Bilal", 55)); 
AddStudents(new Student(7, "Bilal", 55));

Console.WriteLine($"Total Students: {students.Count}");

//Task 5:
Func<int, string> getGrade = marks =>
{
    if (marks >= 90) return "A";
    else if (marks >= 80) return "B";
    else if (marks >= 70) return "C";
    else if (marks >= 60) return "D";
    else
    {
        return "F";
    }
};

foreach (var student in students)
{
    var grade = getGrade(student.Marks);
    Console.WriteLine($"{student.Name} - {student.Marks} -> Grade: {grade}");
}
void printReports(List<Student> allstd, Func<int, string> gradeRule)
{
    foreach (var std in allstd)
    {
        Console.WriteLine($"{std.Name} -> {gradeRule(std.Marks)}");
    }
}
printReports(students, getGrade);
Func<int, string> passOrFail = marks => marks >= 50 ? "Pass" : "Fail";
printReports(students, passOrFail);

Console.WriteLine();
Console.WriteLine("===Task 6 part 1===");
var passedQuery = students.Where(std => std.Marks >= 50);

students.Add(new Student(8, "Khair", 55));
foreach (var std in passedQuery)
    Console.WriteLine(std.Name);
Console.WriteLine();
Console.WriteLine("=== Task 6 Part B ===");

var passedList = students.Where(std => std.Marks >= 50).ToList(); 

students.Add(new Student(8, "Zara", 95));                           

foreach (var std in passedList)
    Console.WriteLine(std.Name);

Console.WriteLine();
Console.WriteLine("=== Task 6 Part C ===");

var checks = 0;
var counted = students.Where(std =>
{
    checks++;                          // har baar rule chale to counter barhao
    return std.Marks >= 50;
});

Console.WriteLine($"Query bani, checks = {checks}");

var total = counted.Count();
Console.WriteLine($"Count() ke baad, checks = {checks}");

var list = counted.ToList();
Console.WriteLine($"ToList() ke baad, checks = {checks}");
Console.ReadLine();
record Student(int RollNo, string Name, int Marks);