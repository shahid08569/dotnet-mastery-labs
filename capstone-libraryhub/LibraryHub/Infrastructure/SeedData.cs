using LibraryHub.Domain;

namespace LibraryHub.Infrastructure;

public static class SeedData
{
    public static List<Book> Books() =>
    [
        new(1, "9783161484100", "The Great Gatsby", "F. Scott Fitzgerald", 1925, Genre.Fiction, 5),
        new(2, "9780140449136", "Crime and Punishment", "Fyodor Dostoevsky", 1866, Genre.Fiction, 3),
        new(3, "9780307263698", "A Brief History of Time", "Stephen Hawking", 1988, Genre.Science, 4),
        new(4, "9780198788607", "The Selfish Gene", "Richard Dawkins", 1976, Genre.Science, 2),
        new(5, "9780000000019", "The Silent Archive", "A. Scribe", 1912, Genre.History, 1),
    ];

    public static List<Member> Members() =>
    [
        new() { Id = 1, Name = "Alice Johnson", Email = "alice.johnson@example.com", Type = MemberType.Regular },
        new() { Id = 2, Name = "Bob Smith", Email = "bob.smith@example.com", Type = MemberType.Student },
        new() { Id = 3, Name = "Charlie Brown", Email = "charlie.brown@example.com", Type = MemberType.Premium },
        new() { Id = 4, Name = "Dana Khan", Type = MemberType.Regular },
    ];
}