using System.Linq.Expressions;

var numbers = new List<int> { 1, 2, 3, 4, 5 };

IEnumerable<int> a = numbers.Where(n => n > 2);
IQueryable<int> b = numbers.AsQueryable().Where(n => n > 2);

Console.WriteLine("IEnumerable ka Where: " + a.GetType().Name);
Console.WriteLine("IQueryable ka Where:  " + b.GetType().Name);
Console.WriteLine();

var call = (MethodCallExpression)b.Expression;
Console.WriteLine("Parchi ki qism (NodeType): " + b.Expression.NodeType);
Console.WriteLine("Parchi mein kaunsa method: " + call.Method.DeclaringType!.Name + "." + call.Method.Name);
Console.WriteLine("Parchi mein condition: " + call.Arguments[1]);

Console.ReadLine();