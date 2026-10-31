using LibraryHub.Domain;
using LibraryHub.Infrastructure;
using LibraryHub.Data;

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

// Generic helper: koi bhi Result<T> print kare
void Print<T>(Result<T> result, Func<T, string> describe)
{
    if (result.IsSuccess)
        Console.WriteLine($"OK   : {describe(result.Value!)}");
    else
        Console.WriteLine($"FAIL : [{result.Error!.Code}] {result.Error.Message}");
}

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

Console.WriteLine();
Console.WriteLine("=== Catalog ===");
var catalog = new BookCatalog();
foreach (var book in SeedData.Books())
{
    Print(catalog.Add(book), b => $"Added Book {b.Id} ({b.Title})");
}

Console.WriteLine();
Console.WriteLine("=== Duplicate tests ===");
// 1. Id pehle se hai (1), ISBN naya
Print(catalog.Add(new Book(1, "9780000000002", "Another Book", "Someone", 2000, Genre.Fiction, 1)),
      b => $"Added Book {b.Id} ({b.Title})");

// 2. Id naya, ISBN maujooda kitab ka (hyphens ke saath, normalize hoke wahi ban jata hai)
Print(catalog.Add(new Book(99, "978-0-14-044913-6", "Crime and Punishment (copy)", "Fyodor Dostoevsky", 1866, Genre.Fiction, 1)),
      b => $"Added Book {b.Id} ({b.Title})");

// 3. Id pehle se hai, ISBN naya (ye nakaam hogi)
Print(catalog.Add(new Book(1, "9781111111113", "Ordering Check", "Tester", 2020, Genre.Technology, 1)),
      b => $"Added Book {b.Id} ({b.Title})");

// 4. Wahi ISBN, lekin Id naya: ab KAAMYAB honi chahiye (HashSet stale nahi hua)
Print(catalog.Add(new Book(50, "9781111111113", "Ordering Check", "Tester", 2020, Genre.Technology, 1)),
      b => $"Added Book {b.Id} ({b.Title})");

// 5. Bilkul nayi kitab
Print(catalog.Add(new Book(100, "9780000000002", "A Genuinely New Book", "Someone", 2000, Genre.Fiction, 1)),
      b => $"Added Book {b.Id} ({b.Title})");

Console.WriteLine();
Console.WriteLine("=== Lookup ===");
Print(catalog.GetById(2), b => $"Found Book {b.Id}: {b.Title}");
Print(catalog.GetById(999), b => $"Found Book {b.Id}: {b.Title}");

Console.WriteLine();
Console.WriteLine("=== Same Repository<T> for other types ===");
var membersRepo = new Repository<Member>();
foreach (var member in SeedData.Members())
{
    Print(membersRepo.Add(member), m => $"Added Member {m.Id} ({m.Name})");
}
Print(membersRepo.Add(new Member { Id = 1, Name = "Duplicate" }), m => $"Added Member {m.Id} ({m.Name})");

Console.WriteLine();
Console.WriteLine("=== Remove and snapshot ===");
var snapshot = membersRepo.GetAll();
Print(membersRepo.Remove(2), m => $"Removed Member {m.Id} ({m.Name})");
Print(membersRepo.Remove(2), m => $"Removed Member {m.Id} ({m.Name})");
Console.WriteLine($"Snapshot (pehle liya): {snapshot.Count} members | Repository ab: {membersRepo.Count} members");

Console.ReadLine();