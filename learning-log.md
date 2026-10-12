# Learning Log

## 2026-10-08 (Week 1)
- **Seekha:** Plan samjha, ratings di, repo banai.
- **Toota:** Kuch nahi abhi.
- **Samajh nahi aaya:** Kuch nahi abhi.

## 2026-10-10 (Week 2) - LINQ Deferred Execution

**Concept:** Deferred execution = query ka kaam abhi taal dena. Query likhne par kuch nahi chalta, sirf instruction yaad rakhi jati hai. Kaam tab hota hai jab result maanga jaye (foreach, ToList, Count).
Misaal: shopping list likhne se saman nahi aata, bazaar jaoge tab aayega.

**Deferred methods:** Where, Select, OrderBy, Skip, Take
**Immediate methods:** ToList, ToArray, Count, First, Any, Sum, foreach

**Prediction vs Result (Lab 01):**
- Bina ToList(): "query created" ke baad kaam hua, 4 bhi result mein aaya, "checking" 8 baar print hua (har enumeration par query dobara chali).
- ToList() ke saath: "checking" 3 baar, query created se pehle, 4 result mein nahi aaya (list ka "photo" ban gaya).

**Real-life bug (Mercato):**
Count() ke baad ToList() lagane se database ko 2 calls jati hain, endpoint slow hota hai, aur dono ke beech data badal sakta hai.
Fix: ek baar ToList() karke variable mein rakho, phir usi se kaam lo.

**Rule:**
- ToList() lagao jab: result kai baar use karna ho, snapshot chahiye, ya kaam isi waqt khatam karna ho.
- Mat lagao jab: sirf ek baar loop karna ho, ya abhi aur filter lagana baqi ho.

**Toota (meri galti):** Socha tha deferred query mein naya item nahi aayega. Ulta tha, deferred query tazaa data dekhti hai. Purana snapshot ToList() wale version mein milta hai.

**Samajh nahi aaya:** EF Core mein ToList() jaldi lagane se saara data memory mein kyun aata hai? (Lesson 2: IEnumerable vs IQueryable)

---

## 2026-10-10 (Week 2) - Lesson 2: IEnumerable vs IQueryable

**Concept:**
Filter ka kaam 2 jagah ho sakta hai: database mein ya C# memory mein.
Misaal: godown mein 10 lakh shirts hain, mujhe 10 laal chahiye.
- Tareeqa 1 (IQueryable): godown ko hukm do "sirf 10 laal bhejo".
- Tareeqa 2 (IEnumerable / pehle ToList): saari 10 lakh mangwao, phir yahan chhanto.

| | IEnumerable<T> | IQueryable<T> |
|---|---|---|
| Kaam kahan hota hai | C# memory mein | Database mein (EF Core SQL banata hai) |
| Where kya leta hai | Func (C# method) | Expression (parchi) |
| Kahan use hota hai | List, array (memory ka data) | EF Core ka DbSet (_db.Products) |

**Func vs Expression:**
- Func = hukm jo C# khud chalata hai.
- Expression = parchi jisko EF Core padh kar SQL banata hai.

**Galat tareeqa (bug):**
```csharp
_db.Products.ToList().Where(p => p.Price > 1000).ToList();
// SQL: SELECT ... FROM Products   (koi WHERE nahi, saari rows aati hain)
```

**Sahi tareeqa:**
```csharp
_db.Products.Where(p => p.Price > 1000).ToList();
// SQL: SELECT ... FROM Products WHERE Price > 1000
```

**Chhupa hua bug:** method ka return type `IEnumerable<Product>` ho (andar se `_db.Products` return ho raha ho) to bahar wala `Where` C# mein chalta hai. Type ka naam decide karta hai ke Where kahan chalega.

**Count:**
- `_db.Products.Count()` = SELECT COUNT(*), sirf ek number aata hai.
- `_db.Products.ToList().Count` = saari rows memory mein, phir ginti.

**ToList() jaldi lagane se kya masla hota hai, aur kyun:**
- Kyun: ToList() lagte hi EF foran SQL bhejta hai, us waqt Where lag hi nahi chuka hota, to SQL mein WHERE nahi hota. Database saari rows deta hai. ToList() ke baad data List ban jata hai, aur uske baad ka Where C# mein chalta hai.
- Nuksan: network zaya, RAM zaya, endpoint slow, database ka index bekaar, data barhne par app slow ya crash.

**Golden rule:**
> Filter, sort, paging pehle (IQueryable par). ToList() sab se aakhir mein.

**Lab 02b result:**
```
IEnumerable ka Where: ListWhereIterator`1
IQueryable ka Where:  EnumerableQuery`1
```
- IEnumerable.Where ek iterator banata hai jo C# method seedha chalata hai.
- IQueryable.Where condition ko Expression (parchi) ki shakal mein rakhta hai.

**Lab 02 (SQLite):** baaqi hai. NuGet error `NU1301 ... No such host is known (api.nuget.org)` = internet/DNS ka masla.

**Dev skills:**
- Error padhna: `Program.cs(1,28): error CS1002: ; expected` = file, (line, column), error code, matlab.
- VS mein console band ho jaye to Ctrl+F5 ya terminal mein `dotnet run`.

**Toota:** Copy paste mein code kat gaya (line 1 syntax error). VS mein window foran band hoti thi.

**Samajh nahi aaya:** Expression tree andar se kaise padhi jati hai aur SQL kaise banta hai?

---


## 2026-10-12 (Week 2) - Lesson 3: Delegates, Lambda, Func, Action

**Concept:**
- Delegate = method ko variable ki tarah pakadna (hukm ka card). Signature fix hoti hai.
- `Func<...>`: return karta hai, aakhri type = return type. `Func<int, string, bool>` = inputs (int, string), return bool.
- `Action<...>`: kuch return nahi karta (void), return kar hi nahi sakta.
- Lambda = chhota method wahin likha hua. `n => n % 2 == 0` = "n lo, n % 2 == 0 wapas do".

**Masla jo delegate hal karta hai:** har rule ke liye alag filter method (copy paste). Ab ek `MyWhere(list, rule)` aur alag lambdas.

**MyWhere trace:** `rule(item)` ka matlab pass kiya hua method chalana. `MyWhere(numbers, isEven)` mein rule = isEven.
- Bina brackets = method pass karna. Brackets ke saath = method chalana.

**Lesson 2 se link:** `IEnumerable.Where` leta hai `Func<T,bool>`. `IQueryable.Where` leta hai `Expression<Func<T,bool>>`. Lambda same likhta hoon, compiler decide karta hai ke method banana hai ya parchi.

**Method vs lambda:**
- Method mein parameter ka type zaroori: `static bool isEven(int n)`.
- Func + lambda mein type left side par hota hai, isliye lambda mein `n` ka type nahi likhna parta.
- Chhota rule = lambda. Bara/dobara use hone wala = naam wala method.

**Lab 03 result:**
```
True
False
Message:Hello
2,4
4,5
```
(list `{1,2,3,4,5}` thi.)

**Toota:** `static bool isEven(n)` likha tha (type missing). Fix: `int n`.

**Samajh nahi aaya:** (yahan likho)

---										