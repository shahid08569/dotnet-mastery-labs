using LibraryHub.Domain;
using LibraryHub.Infrastructure;

var books = SeedData.Books();
var members = SeedData.Members();

Console.WriteLine("=== Books ===");
foreach (var book in books)
{
    Console.WriteLine($"{book.Id} | {book.Title} | {book.Author} | {book.Year} | {book.Genre} | copies: {book.TotalCopies}");
}


Console.WriteLine();
Console.WriteLine("=== Members ===");
foreach (var member in members)
{
    Console.WriteLine($"{member.Id} | {member.Name} | {member.Email ?? "no email"} | {member.Type}");
}

Console.WriteLine();
Console.WriteLine("=== Fines (daysLate = fine) ===");
int[] days = [0, 3, 7, 8, 15];
foreach (var type in Enum.GetValues<MemberType>())
{
    var line = string.Join(", ", days.Select(d => $"{d}={Fines.Calculate(d, type)}"));
    Console.WriteLine($"{type,-8}: {line}");
}
Console.ReadLine();