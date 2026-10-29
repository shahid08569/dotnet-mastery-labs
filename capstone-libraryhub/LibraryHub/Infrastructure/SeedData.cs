using LibraryHub.Domain;
namespace LibraryHub.Infrastructure;

public static class SeedData
{
    public static List<Book> Books() =>
    [
		new(1, "978-3-16-148410-0", "The Great Gatsby", "F. Scott Fitzgerald", 1925, Genre.Fiction, 5),
        new(2, "978-0-14-044913-6", "Crime and Punishment", "Fyodor Dostoevsky", 1866, Genre.Fiction, 3),
        new(3, "978-0-307-26369-2", "A Brief History of Time", "Stephen Hawking", 1988, Genre.Science, 4),
        new(4, "978-0-307-26369-3", "The Selfish Gene", "Richard Dawkins", 1976, Genre.Science, 2),
	];

    public static List<Member> Members() =>
    [
        new() { Id = 1, Name = "Alice Johnson", Email = "alice.johnson@example.com", Type = MemberType.Regular },
        new() { Id = 2, Name = "Bob Smith", Email = "bob.smith@example.com", Type = MemberType.Student },
        new() { Id = 3, Name = "Charlie Brown", Email = "charlie.brown@example.com", Type = MemberType.Premium }

    ];
}