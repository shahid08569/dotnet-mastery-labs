var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Middleware 1 (guard)
app.Use(async (context, next) =>
{
    Console.WriteLine("1 shuru");
    await next(context);
    Console.WriteLine("1 khatam");
});

// Middleware 2 (token counter), /blocked ko rok deta hai
app.Use(async (context, next) =>
{
    Console.WriteLine("  2 shuru");

    if (context.Request.Path == "/blocked")
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Rok diya (short-circuit)");
        Console.WriteLine("  2 ne request rok di");
        return;                               // next nahi bulaya
    }

    await next(context);
    Console.WriteLine("  2 khatam");
});
app.Run(async context => await context.Response.WriteAsync("Run ne pipeline khatam ki"));
// Endpoint (teller)
app.MapGet("/", () =>
{
    Console.WriteLine("    endpoint chala");
    return "Hello from endpoint";
});

app.Run();