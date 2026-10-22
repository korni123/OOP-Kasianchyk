# Звіт до самостійної роботи №8

**Тема:** `Dictionary<TKey, TValue>` та `HashSet<T>` у бібліотеках .NET.

## 1. Приклади з open-source проєктів

### Приклад 1 — Dictionary у Dapper

Файл: [Dapper/SqlMapper.cs](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L171)

```csharp
private static Dictionary<Type, TypeMapEntry> typeMap;
...
[typeof(int)] = DbType.Int32,
[typeof(string)] = DbType.String,
...
if (typeMap.TryGetValue(type, out var mapEntry)) { ... }
```

- **Задача:** Dapper має знати, який тип у базі даних відповідає типу C# (`int` → `Int32`, `string` → `String`).
- **Чому Dictionary:** треба за ключем (тип C#) швидко знайти значення (тип БД). У списку довелося б щоразу перебирати всі елементи.
- **Складність:** пошук `TryGetValue` — O(1).
- **Особливості:** використано `TryGetValue`, бо типу може не бути в таблиці, і тоді не потрібен виняток.

### Приклад 2 — HashSet у Serilog

Файл: [src/Serilog/Capturing/PropertyValueConverter.cs](https://github.com/serilog/serilog/blob/81af75f3a5e28ec9f5be18ef74357a9914a51745/src/Serilog/Capturing/PropertyValueConverter.cs#L23-L32)

```csharp
static readonly HashSet<Type> BuiltInScalarTypes = new()
{
    typeof(decimal), typeof(string), typeof(DateTime), typeof(Guid), ...
};
...
BuiltInScalarTypes.Contains(valueType)
```

- **Задача:** під час запису логу Serilog перевіряє, чи значення простого типу (тоді воно пишеться як є).
- **Чому HashSet:** потрібна лише відповідь «є тип у наборі чи ні». Значення не потрібні, повторів бути не може.
- **Складність:** `Contains` — O(1).
- **Особливості:** набір `static readonly` — створюється один раз на всю програму.

### Приклад 3 — Dictionary + HashSet разом у Dapper

Файл: [Dapper/SqlMapper.cs, метод PassByPosition](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L1907-L1951)

```csharp
Dictionary<string, IDbDataParameter> parameters = new(StringComparer.Ordinal);
var consumed = new HashSet<string>(StringComparer.Ordinal);
...
if (!consumed.Add(key))
{
    throw new InvalidOperationException("... each parameter can only be referenced once");
}
else if (parameters.TryGetValue(key, out IDbDataParameter? param)) { ... }
```

- **Задача:** замінити іменовані параметри в SQL на `?` і розставити їх у правильному порядку.
- **Чому ці колекції:** `Dictionary` — щоб за назвою швидко знайти параметр; `HashSet` — щоб запам'ятати, які назви вже використані, і не дати використати їх двічі.
- **Складність:** кожна перевірка O(1).
- **Особливості:** використано custom comparer `StringComparer.Ordinal` — назви порівнюються з урахуванням регістру. `HashSet.Add` повертає `false`, якщо елемент уже є, тому перевірка і додавання робляться одним викликом.

## 2. Порівняльна таблиця

| Операція | List | Dictionary\<TKey,TValue\> | HashSet | Коли використовувати |
|---|---|---|---|---|
| Додавання | O(1) | O(1) | O(1) | усі три — швидко |
| Пошук за значенням | O(n) | N/A | O(1) | HashSet — перевірити, чи є елемент |
| Пошук за ключем | N/A | O(1) | N/A | Dictionary — знайти дані за ID чи назвою |
| Видалення | O(n) | O(1) | O(1) | Dictionary/HashSet — часті видалення |
| Перевірка наявності | O(n) | O(1) | O(1) | Dictionary/HashSet — перевірки в циклах |

## 3. Висновки

**Коли Dictionary краще за List:** коли дані потрібно шукати за ключем (ID, назва) і таких пошуків багато — кеші, довідники, індекси. List кращий, коли важливий порядок і елементи просто перебираються.

**Коли HashSet — оптимальний вибір:** коли потрібні тільки унікальні значення і перевірка «чи є елемент», а також операції над множинами — об'єднання (`UnionWith`), перетин (`IntersectWith`), різниця (`ExceptWith`).

**Підводні камені:**
- `dict[key]` кидає виняток, якщо ключа немає — краще `TryGetValue`;
- рядкові ключі за замовчуванням чутливі до регістру — для назв файлів чи тегів варто `StringComparer.OrdinalIgnoreCase`;
- для своїх класів треба перевизначати `Equals` і `GetHashCode` або передавати `IEqualityComparer<T>`;
- порядок елементів не гарантований;
- ці колекції не потокобезпечні (для потоків — `ConcurrentDictionary`).

## 4. Приклади з власного коду

- **FileMetadataCache** — `Dictionary<string, FileMetadata>` з `StringComparer.OrdinalIgnoreCase`, тому `Get("REPORT.DOCX")` знаходить `report.docx`.
- **TagManager** — `Dictionary<string, HashSet<string>>`: повторний тег не додається, спільні теги шукаються через `IntersectWith`, усі теги — через `UnionWith`, різниця — через `ExceptWith`.
- **SearchIndex** — `Dictionary<string, HashSet<int>>`: слово → номери документів, пошук за кількома словами через `IntersectWith`.
- **PersonNameComparer** — `HashSet<Person>` вважає людей з однаковим ім'ям однаковими, тому другий `John` не додається.
