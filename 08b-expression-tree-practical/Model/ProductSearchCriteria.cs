namespace Model;

public record PriceRange(decimal? min, decimal? max);
public record Category(string name);
public record ProductName(string name);
public class ProductSearchCriteria
{
	public bool? IsActive { get; set; }
	public PriceRange? Price { get; set; }

	public Category[]? Categories { get; set; }

	public ProductName[]? Name { get; set; }

}
