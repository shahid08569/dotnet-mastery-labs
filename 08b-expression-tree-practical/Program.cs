using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Linq.Expressions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductDbContext>(options => options.UseInMemoryDatabase("ProdctDb"));

var app = builder.Build();
app.MapGet("/products", async ([FromBody]ProductSearchCriteria productSearch, ProductDbContext dbContext) =>
{
   await dbContext.Database.EnsureCreatedAsync();
   ParameterExpression param= Expression.Parameter(typeof(Product), "p");
   Expression pred= Expression.Constant(true);
    if (productSearch.IsActive.HasValue)
    {
       MemberExpression memExp= Expression.Property(param, nameof(Product.IsActive));
       ConstantExpression contExp = Expression.Constant(productSearch.IsActive.Value);
       BinaryExpression binExp= Expression.Equal(memExp, contExp);
       pred = Expression.AndAlso(pred, binExp);
       
    }
    var lambExp = Expression.Lambda<Func<Product, bool>>(pred, param);
    var data = dbContext.Products.Where(lambExp).ToList();
    return Results.Ok(data);
});

app.UseHttpsRedirection();


app.Run();
    
