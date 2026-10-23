// 1. Value type copy
int a = 5;
int b = a;
b = 10;
Console.WriteLine($"a={a}, b={b}");

// 2. Reference type copy
var p1 = new Person { Name = "Ali" };
var p2 = p1;
p2.Name = "Sara";
Console.WriteLine($"p1.Name={p1.Name}, p2.Name={p2.Name}");
Console.WriteLine(ReferenceEquals(p1, p2));

// 3. Alag object, same content
var p3 = new Person { Name = "Sara" };
Console.WriteLine(p2 == p3);
Console.WriteLine(ReferenceEquals(p2, p3));

// 4. Method ko value aur reference dena
void ChangeNumber(int x) { x = 99; }
void ChangePerson(Person p) { p.Name = "Changed"; }

int n = 1;
ChangeNumber(n);
Console.WriteLine(n);

ChangePerson(p1);
Console.WriteLine(p1.Name);

Console.ReadLine();

class Person
{
    public string Name { get; set; } = "";
}