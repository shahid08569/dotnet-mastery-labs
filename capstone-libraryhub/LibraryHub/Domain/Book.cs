namespace LibraryHub.Domain;

public record Book(int Id, string Isbn, string Title, string Author, int Year, Genre Genre, int TotalCopies);