# Звіт: Dictionary<TKey, TValue> та HashSet<T> у бібліотеках .NET

**Самостійна робота №8.** Посилання на код зафіксовані на конкретні коміти, тому номери рядків не зміняться.

## 1. Результати аналізу open-source проєктів

### Приклад 1 — Dictionary: таблиця типів у Dapper

**Файл:** [Dapper/SqlMapper.cs](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L171) (рядки 171, 202–212, 463–476, 309)

```csharp
private static Dictionary<Type, TypeMapEntry> typeMap;

typeMap = new Dictionary<Type, TypeMapEntry>(41
#if NET6_0_OR_GREATER
    + 4 // {Date|Time}Only[?]
#endif
    )
{
    [typeof(byte)] = DbType.Byte,
    [typeof(sbyte)] = DbType.SByte,
    [typeof(short)] = DbType.Int16,
    ...
};

// LookupDbType(...)
if (typeMap.TryGetValue(type, out var mapEntry)) { ... }
```

- **Контекст:** Dapper (micro-ORM) для кожного параметра SQL-запиту має визначити, який `DbType` відповідає типу C# (`int → Int32`, `string → String` тощо). `LookupDbType` викликається для **кожного** параметра кожного запиту.
- **Чому Dictionary:** потрібне відображення «ключ → значення» (`Type → DbType`). У `List<(Type, DbType)>` пошук був би лінійним перебором ~45 елементів на кожен параметр.
- **Складність:** `TryGetValue` — O(1) в середньому. `TryGetValue` замість індексатора, бо тип може бути відсутнім у таблиці, і це нормальна ситуація, а не помилка (без винятку).
- **Особливості:** початкова ємність задана наперед (41, а для .NET 6+ ще 4 типи `DateOnly`/`TimeOnly`), щоб словник не перебудовувався під час заповнення. Для потокобезпечності використано copy-on-write: при додаванні нового типу створюється **нова копія** словника `new Dictionary<Type, TypeMapEntry>(snapshot) { [type] = value }` і посилання підмінюється — потоки, що читають, ніколи не бачать словник у стані зміни.

### Приклад 1б — Dictionary з custom comparer: властивості JObject у Newtonsoft.Json

**Файл:** [Src/Newtonsoft.Json/Linq/JPropertyKeyedCollection.cs](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Linq/JPropertyKeyedCollection.cs#L37-L112)

```csharp
private static readonly IEqualityComparer<string> Comparer = StringComparer.Ordinal;
private Dictionary<string, JToken>? _dictionary;

private void EnsureDictionary()
{
    if (_dictionary == null)
    {
        _dictionary = new Dictionary<string, JToken>(Comparer);
    }
}
```

- **Контекст:** зберігання властивостей JSON-об'єкта, щоб `obj["name"]` знаходився швидко.
- **Чому Dictionary:** пошук властивості за назвою — O(1) замість перебору списку.
- **Особливості:** явно заданий `StringComparer.Ordinal` — у JSON ключі `"Name"` і `"name"` різні, і порівняння має бути побайтовим, без урахування культури (це ще й найшвидший компаратор). Словник створюється **ліниво** (`EnsureDictionary`) — для маленьких об'єктів без пошуку пам'ять не витрачається.

### Приклад 2 — HashSet: вбудовані скалярні типи в Serilog

**Файл:** [src/Serilog/Capturing/PropertyValueConverter.cs](https://github.com/serilog/serilog/blob/81af75f3a5e28ec9f5be18ef74357a9914a51745/src/Serilog/Capturing/PropertyValueConverter.cs#L23-L32) (рядки 23–32 і 428–433)

```csharp
static readonly HashSet<Type> BuiltInScalarTypes = new()
{
    typeof(decimal),
    typeof(string),
    typeof(DateTime), typeof(DateTimeOffset), typeof(TimeSpan),
    typeof(Guid), typeof(Uri),
};

static bool IsValidDictionaryKeyType(Type valueType)
{
    return valueType.IsPrimitive ||
           BuiltInScalarTypes.Contains(valueType) ||
           valueType.IsEnum;
}
```

- **Контекст:** при логуванні Serilog вирішує, чи записати значення «як є» (скаляр), чи розкласти об'єкт на властивості. Перевірка виконується для кожного значення в кожному повідомленні логу.
- **Чому HashSet:** потрібна тільки відповідь «так/ні» — чи належить тип множині. Значення (як у Dictionary) не потрібні, а дублікати неможливі за змістом.
- **Складність:** `Contains` — O(1) замість O(n) у `List<Type>`.
- **Особливості:** `static readonly` — множина створюється один раз на весь застосунок; компаратор не потрібен, бо `Type` правильно реалізує `Equals`/`GetHashCode`.

### Приклад 3 — комбінований: Dictionary + HashSet у Dapper

**Файл:** [Dapper/SqlMapper.cs, метод `PassByPosition`](https://github.com/DapperLib/Dapper/blob/eb47546a408bbf6c178bda4f04dc205bc51ffbfe/Dapper/SqlMapper.cs#L1907-L1951) (рядки 1907–1951)

```csharp
Dictionary<string, IDbDataParameter> parameters = new(StringComparer.Ordinal);
foreach (IDbDataParameter param in cmd.Parameters)
{
    if (!string.IsNullOrEmpty(param.ParameterName)) parameters[param.ParameterName] = param;
}
var consumed = new HashSet<string>(StringComparer.Ordinal);

cmd.CommandText = CompiledRegex.PseudoPositional.Replace(cmd.CommandText, match =>
{
    string key = match.Groups[1].Value;
    if (!consumed.Add(key))
    {
        throw new InvalidOperationException("When passing parameters by position, each parameter can only be referenced once");
    }
    else if (parameters.TryGetValue(key, out IDbDataParameter? param))
    {
        ...
        parameters.Remove(key);
        consumed.Add(key);
        return "?";
    }
    ...
});
```

- **Контекст:** деякі БД (наприклад, через ODBC) підтримують тільки позиційні параметри `?`. Dapper переписує SQL, замінюючи `?name?` на `?` і розставляючи параметри в правильному порядку.
- **Чому саме ці колекції:** `Dictionary` — щоб за назвою з тексту SQL миттєво знайти об'єкт параметра; `HashSet` — щоб запам'ятати, які назви вже використано, і заборонити повторне використання.
- **Складність:** кожен збіг регулярного виразу обробляється за O(1); весь метод — O(n) від кількості параметрів (зі списками було б O(n²)).
- **Особливості:** трюк `if (!consumed.Add(key))` — `HashSet.Add` повертає `false`, якщо елемент уже є, тому перевірка і додавання робляться однією операцією. Обидві колекції мають `StringComparer.Ordinal` (назви параметрів чутливі до регістру).

## 2. Порівняльна таблиця

| Операція | List\<T\> | Dictionary\<TKey,TValue\> | HashSet\<T\> | Коли використовувати |
|---|---|---|---|---|
| Додавання | O(1) амортизовано (в кінець) | O(1) амортизовано | O(1) амортизовано | List — коли важливий порядок і дублікати; Dictionary/HashSet — коли потрібен швидкий пошук |
| Пошук за значенням | O(n) (`Contains`, `IndexOf`) | N/A (`ContainsValue` — O(n)) | O(1) (`Contains`) | HashSet — «чи є такий елемент?» |
| Пошук за ключем | N/A (за індексом — O(1)) | O(1) (`TryGetValue`, `[key]`) | N/A | Dictionary — «знайди дані за ідентифікатором» |
| Видалення | O(n) (зсув елементів) | O(1) | O(1) | Dictionary/HashSet — часті видалення довільних елементів |
| Перевірка наявності | O(n) | O(1) (`ContainsKey`) | O(1) | Dictionary/HashSet — перевірки в циклах |

O(1) — середній випадок. У найгіршому випадку (багато колізій хешів через поганий `GetHashCode`) хеш-колекції деградують до O(n).

## 3. Висновки про вибір колекції

**Коли Dictionary краще за List:**
- коли дані шукаються за ключем (ID, назва, тип), а не перебираються по порядку;
- коли пошук виконується багато разів — кеші, таблиці відповідностей, індекси (як `typeMap` у Dapper чи `FileMetadataCache` у моєму коді);
- List кращий, коли потрібен порядок, доступ за індексом, дублікати або коли елементів дуже мало і всі перебираються.

**Коли HashSet є оптимальним вибором:**
- коли потрібні тільки унікальні значення і перевірка «чи є елемент» (теги, відвідані вузли, «вже використані» назви);
- коли потрібні операції над множинами: `UnionWith` (OR), `IntersectWith` (AND), `ExceptWith` (різниця) — як у `TagManager` і `SearchIndex`.

**Підводні камені:**
- індексатор `dict[key]` кидає `KeyNotFoundException`, якщо ключа немає — для читання краще `TryGetValue`;
- змінювати ключ-об'єкт після додавання не можна: зміниться хеш, і елемент «загубиться» в колекції;
- для власних класів-ключів треба правильно перевизначати `Equals` і `GetHashCode` разом (або передавати `IEqualityComparer<T>`), інакше порівнюватимуться посилання;
- рядкові ключі за замовчуванням чутливі до регістру — для назв файлів, тегів краще `StringComparer.OrdinalIgnoreCase`;
- порядок перебору Dictionary/HashSet не гарантований;
- колекції не потокобезпечні — для багатопотокового доступу `ConcurrentDictionary` або copy-on-write, як у Dapper;
- хеш-колекції займають більше пам'яті, ніж List.

## 4. Приклади з власного коду

Код — у `IndependentWork8/Program.cs`.

- **`FileMetadataCache`** — `Dictionary<string, FileMetadata>` з `StringComparer.OrdinalIgnoreCase`, тому `Get("REPORT.DOCX")` знаходить `report.docx`. Методи `Add`, `Get` (через `TryGetValue`, повертає `null` замість винятку), `Remove`, `Contains`.
- **`TagManager`** — `Dictionary<string, HashSet<string>>` (статя → теги). Дубль тегу (`CSharp` після `csharp`) не додається, бо `HashSet` з ignore-case компаратором. `GetAllUniqueTags` — `UnionWith`, `FindCommonTags` — `IntersectWith`, `FindUniqueTags` — `ExceptWith`.
- **`SearchIndex`** — інвертований індекс `Dictionary<string, HashSet<int>>` (слово → документи). `SearchMultiple` перетинає множини документів через `IntersectWith`.
- **`HashSet<Person>` з `PersonNameComparer`** — дві людини з однаковим ім'ям вважаються одним елементом, другий `Add` повертає `false`.
