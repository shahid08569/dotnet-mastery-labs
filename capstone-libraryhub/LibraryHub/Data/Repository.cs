using LibraryHub.Domain;

namespace LibraryHub.Data;

public class Repository<T> where T : class, IEntity
{
    private readonly Dictionary<int, T> _items = [];

    public int Count => _items.Count;

    public Result<T> Add(T item)
    {
        if (_items.TryAdd(item.Id, item))
            return Result<T>.Success(item);

        return Result<T>.Failure("duplicate_id", $"{typeof(T).Name} with id {item.Id} already exists");
    }

    public Result<T> GetById(int id)
    {
        if (_items.TryGetValue(id, out var item))
            return Result<T>.Success(item);

        return Result<T>.Failure("not_found", $"{typeof(T).Name} with id {id} not found");
    }

    public Result<T> Remove(int id)
    {
        if (_items.Remove(id, out var removed))
            return Result<T>.Success(removed);

        return Result<T>.Failure("not_found", $"{typeof(T).Name} with id {id} not found");
    }

    public IReadOnlyList<T> GetAll() => _items.Values.ToList().AsReadOnly();
}