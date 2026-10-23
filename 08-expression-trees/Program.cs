using System.Linq.Expressions;

// 1. Func vs Expression: print
Func<int, bool> f = n => n > 3;
Expression<Func<int, bool>> expr = n => n > 3;

Console.WriteLine(f);
Console.WriteLine(expr);

// 2.Inside Tree
Console.WriteLine(expr.NodeType);
var body = (BinaryExpression)expr.Body;
Console.WriteLine(body.NodeType);
Console.WriteLine(body.Left);
Console.WriteLine(body.Right);
Console.WriteLine(body.Right.NodeType);

// 3. Compile
var compiled = expr.Compile();
Console.WriteLine(compiled(5));
Console.WriteLine(compiled(2));

// 4. Student par (ek property wali condition)
Expression<Func<Student, bool>> passExpr = std => std.Marks >= 50;
var passBody = (BinaryExpression)passExpr.Body;

Console.WriteLine(passExpr);
Console.WriteLine(passBody.NodeType);
Console.WriteLine(passBody.Left.NodeType);
Console.WriteLine(((MemberExpression)passBody.Left).Member.Name);
Console.WriteLine(passBody.Right);

Console.ReadLine();

record Student(int RollNo, string Name, int Marks);