## 2026-10-15 (Week 2) - Project: 06-student-manager (Tasks 1-6)

**Kya banaya:** console project jo Lesson 1-5 ko jodta hai (LINQ, delegates, collections, generics).

**Tasks:**
1. `students.Where(student => student.Marks >= 50)`, query ko `foreach` se chalana (deferred).
2. Apna `Helpers.MyWhere<T>(List<T> source, Func<T, bool> rule)`, alag file mein, `public static`.
3. `ToDictionary(student => student.RollNo)`, `TryGetValue` (crash nahi karta, `dict[key]` crash karta hai).
4. `HashSet<int>` se duplicate roll number rokna. List aur HashSet dono ko saath update karna.
5. `Func<int, string> getGrade`, kai `if` wali lambda ki body `{ }` mein, `return` zaroori. Func ko method (`printReports`) ko pass kiya.
6. Deferred execution experiment: query `foreach` par chalti hai (naya student dikhta hai), `ToList()` snapshot hai (baad ka student nahi dikhta), `Count()` + `ToList()` = rule 2 baar (checks 0, 9, 18).

**Errors jo padhe (file, line, code, matlab):**
- CS8803: `record` statements ke beech mein. Fix: type declarations aakhir mein.
- CS0019: `Student > 50`. `n` Student tha, number nahi. Fix: `student.Marks > 50`.
- CS0138/CS0116: `using Helpers;` (class hai, namespace nahi) aur `staic` ka typo.
- CS0103: `itme` ka typo.

**Meri galtiyan (jo khud pakadi/pakdi gayi):**
- `students.Add(...)` seedha kiya, `AddStudents` ki jagah: duplicate roll 8 aaya aur HashSet stale ho gaya.
- Grade aur Pass ka takrao: 55 marks par F bhi aur Pass bhi (rules do jagah alag).
- Naam: `marks` ke andar Student tha, `student99` mein roll 3 ka student tha. Rule: naam us cheez jaisa ho jo andar hai.
- Method ka naam PascalCase (`MyWhere`), variable camelCase.

**Rule:** Jab ek hi data do jagah ho (list + HashSet), dono ko saath update karo. Rules ek jagah define karo.

**Samajh nahi aaya:** (yahan likho)

---