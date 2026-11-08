var middlewares = new List<Func<Context, Func<Context, Task>, Task>> {

    async(ctx,next)=>{
        Console.WriteLine("start of first ");
        await next (ctx);
        Console.WriteLine("end of first");
    },
    async(ctx,next)=>{
       Console.WriteLine("Start of second");
        if(ctx.Path =="/blocked"){
            ctx.Status=403;
            ctx.Body="Stop it";
            Console.WriteLine("Request blocked by second middleware");
            return;
        }
        await next(ctx);
        Console.WriteLine("End of second");
    },
};

Func<Context, Task> endpoint = ctx =>
{
    if (ctx.Path == "/")
    {
        Console.WriteLine("    endpoint chala");
        ctx.Body = "Hello";
    }
    else
    {
        ctx.Status = 404;
        ctx.Body = "Not found";
    }
    return Task.CompletedTask;
};

Func<Context, Task> Build(
    List<Func<Context, Func<Context, Task>, Task>> middlewares,
    Func<Context, Task> endpoint)
{
    var current = endpoint;
    for (int i = middlewares.Count - 1; i >= 0; i--)
    {
        var middleware = middlewares[i];
        var next = current;
        current = ctx => middleware(ctx, next);
    }
    return current;
}

var pipeline = Build(middlewares, endpoint);

foreach (var path in new[] { "/", "/blocked", "/nothing" })
{
    var ctx = new Context { Path = path };
    Console.WriteLine($"--- Request: {path}");
    await pipeline(ctx);
    Console.WriteLine($"--- Response: {ctx.Status} {ctx.Body}");
}


Console.ReadLine();

class Context
{
    public string Path { get; init; } = "";
    public int Status { get; set; } = 200;
    public string Body { get; set; } = "";
}