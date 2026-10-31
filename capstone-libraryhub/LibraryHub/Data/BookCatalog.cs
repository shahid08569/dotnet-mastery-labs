using LibraryHub.Domain;

namespace LibraryHub.Data;

public class BookCatalog
{
    private readonly Repository<Book> _books = new();
    private readonly HashSet<string> _isbns = [];

    public Result<Book> Add(Book book)
    {
        var isbn = NormalizeIsbn(book.Isbn);

        if (_isbns.Contains(isbn))
            return Result<Book>.Failure("duplicate_isbn", $"ISBN {isbn} already exists");

        var result = _books.Add(book with { Isbn = isbn });

        if (result.IsSuccess)
            _isbns.Add(isbn);

        return result;
    }

    public Result<Book> GetById(int id) => _books.GetById(id);

    public IReadOnlyList<Book> GetAll() => _books.GetAll();

    private static string NormalizeIsbn(string isbn) =>
        new string(isbn.Where(char.IsDigit).ToArray());
}