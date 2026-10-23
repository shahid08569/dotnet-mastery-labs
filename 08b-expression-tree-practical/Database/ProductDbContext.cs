using Microsoft.EntityFrameworkCore;
using System;
namespace Model;
public class ProductDbContext : DbContext
{
    public DbSet<Product> Products { get; set;}

    public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(new List<Product>
            {
                new Product(){Id =1, Category = "TV", IsActive = true,Name="LG",Price =3000},
                new Product(){Id =2, Category = "Mpbile", IsActive =false,Name="Samsung",Price =50000},
                new Product(){Id =3, Category = "Laptop", IsActive = true,Name="Hp",Price =700000}
            });
    }
}