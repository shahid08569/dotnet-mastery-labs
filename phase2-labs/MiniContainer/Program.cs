var container = new MiniContainer();

// Rules likhna (REGISTER)
container.Add<IMessageSender, ConsoleSender>(Lifetime.Transient);
container.Add<NotificationService>(Lifetime.Transient);
container.Add<Counter>(Lifetime.Singleton);        // ek hi (cash register)
container.Add<OrderSlip>(Lifetime.Scoped);         // har request ka ek (order slip)
container.Add<Cup>(Lifetime.Transient);            // har baar naya (paper cup)
container.Add<BadCache>(Lifetime.Singleton);       // singleton jisko scoped cheez chahiye (BUG!)

// Do "requests" (har request ka apna scope)
var request1 = container.CreateScope();
var request2 = container.CreateScope();

// 1. Zanjeer khud bani: NotificationService ko IMessageSender chahiye tha
request1.Resolve<NotificationService>().Notify("Ali", "Kitab wapas karo");

// 2. Lifetimes
Console.WriteLine($"Singleton: do requests mein same?      {ReferenceEquals(request1.Resolve<Counter>(), request2.Resolve<Counter>())}");
Console.WriteLine($"Scoped: ek request ke andar same?      {ReferenceEquals(request1.Resolve<OrderSlip>(), request1.Resolve<OrderSlip>())}");
Console.WriteLine($"Scoped: do requests mein same?         {ReferenceEquals(request1.Resolve<OrderSlip>(), request2.Resolve<OrderSlip>())}");
Console.WriteLine($"Transient: ek request mein bhi same?   {ReferenceEquals(request1.Resolve<Cup>(), request1.Resolve<Cup>())}");

// 3. Captive dependency (bug) khud dekho
var bad1 = request1.Resolve<BadCache>();
var bad2 = request2.Resolve<BadCache>();
var request1Slip = request1.Resolve<OrderSlip>();
Console.WriteLine($"Request 2 ki BadCache mein REQUEST 1 ki slip? {bad2.SlipId == request1Slip.Id}");

// ===== Container ka dil =====
enum Lifetime { Singleton, Scoped, Transient }

class MiniContainer
{
    internal readonly Dictionary<Type, (Type Impl, Lifetime Life)> Registrations = new();
    internal readonly Dictionary<Type, object> Singletons = new();

    public void Add<TService, TImpl>(Lifetime life) where TImpl : TService
        => Registrations[typeof(TService)] = (typeof(TImpl), life);

    public void Add<TService>(Lifetime life) => Add<TService, TService>(life);

    public MiniScope CreateScope() => new(this);
}

class MiniScope(MiniContainer root)
{
    private readonly Dictionary<Type, object> _scoped = new();

    public T Resolve<T>() => (T)Resolve(typeof(T));

    private object Resolve(Type serviceType)
    {
        if (!root.Registrations.TryGetValue(serviceType, out var reg))
            throw new InvalidOperationException($"Unable to resolve service for type '{serviceType.Name}'");

        // Kaun si "almari" (cache) is lifetime ki hai?
        Dictionary<Type, object>? cache = reg.Life switch
        {
            Lifetime.Singleton => root.Singletons,
            Lifetime.Scoped => _scoped,
            _ => null                                   // Transient: koi cache nahi
        };

        if (cache is not null && cache.TryGetValue(serviceType, out var existing))
            return existing;                            // pehle se bana hua hai, wohi do

        // Naya banao: constructor padho, har parameter ke liye Resolve (RECURSION!)
        var ctor = reg.Impl.GetConstructors().First();
        var args = ctor.GetParameters().Select(p => Resolve(p.ParameterType)).ToArray();
        var instance = ctor.Invoke(args);

        if (cache is not null)
            cache[serviceType] = instance;              // aage ke liye yaad rakho
        return instance;
    }
}

interface IMessageSender { void Send(string to, string text); }

class ConsoleSender : IMessageSender
{
    public void Send(string to, string text) => Console.WriteLine($"[ConsoleSender] {to}: {text}");
}

class NotificationService(IMessageSender sender)
{
    public void Notify(string user, string text) => sender.Send(user, text);
}

class Counter { }
class Cup { }
class OrderSlip { public Guid Id { get; } = Guid.NewGuid(); }
class BadCache(OrderSlip slip) { public Guid SlipId => slip.Id; }