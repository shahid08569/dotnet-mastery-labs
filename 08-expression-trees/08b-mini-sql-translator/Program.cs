using System.Linq.Expressions;

string operatorToSql(ExpressionType type) => type switch
{
    ExpressionType.GreaterThan => ">",
    ExpressionType.GreaterThanOrEqual => ">=",
    ExpressionType.LessThan => "<",
    ExpressionType.LessThanOrEqual => "<=",
    ExpressionType.Equal => "=",
    ExpressionType.NotEqual => "<>",
    ExpressionType.AndAlso => "AND",
    ExpressionType.OrElse => "OR",
    _ => throw new NotSupportedException($"Operator {type} is not supported")
};

string ToSql(Expression node) => node switch
{
    BinaryExpression b => $"({ToSql(b.Left)} {operatorToSql(b.NodeType)} {ToSql(b.Right)})",
    MemberExpression m => m.Member.Name,
    ConstantExpression c => c.Value is string text ? $"'{text}'" : c.Value?.ToString() ?? "NULL",
    _ => throw new NotSupportedException($"Expression type {node.NodeType} is not supported")
};

string ToWhere(Expression<Func<Student, bool>> condition) => "WHERE " + ToSql(condition.Body);

Console.WriteLine(ToWhere(std => std.Marks >= 50));
Console.WriteLine(ToWhere(std => std.Marks >= 50 && std.Name == "Ali"));
Console.WriteLine(ToWhere(std => std.Marks < 40 || std.Marks > 90));

Console.ReadLine();
record Student(int RollNo, string Name, int Marks);