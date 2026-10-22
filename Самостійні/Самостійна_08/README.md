# Самостійна робота №8

**Тема:** Вивчення `Dictionary<TKey, TValue>` та `HashSet<T>` у бібліотеках .NET.

**Проєкт:** `IndependentWork8`

## Опис класів

### FileMetadataCache (Dictionary)

- **Поле:** `Dictionary<string, FileMetadata> _cache` — без урахування регістру назви файлу
- **Методи:** `Add`, `Get` (через `TryGetValue`), `Remove`, `Contains`

### TagManager (Dictionary + HashSet)

- **Поле:** `Dictionary<string, HashSet<string>> _articleTags` — стаття → теги
- **Методи:** `AddTag`, `RemoveTag`, `GetAllUniqueTags` (`UnionWith`), `FindCommonTags` (`IntersectWith`), `FindUniqueTags` (`ExceptWith`)

### SearchIndex (Dictionary + HashSet)

- **Поле:** `Dictionary<string, HashSet<int>> _invertedIndex` — слово → документи
- **Методи:** `AddDocument`, `Search`, `SearchMultiple` (AND через `IntersectWith`)

### PersonNameComparer

- Реалізує `IEqualityComparer<Person>` — люди з однаковим ім'ям вважаються однаковими в `HashSet<Person>`

## Демонстрація в Main

1. **Кеш файлів** — додавання, пошук `REPORT.DOCX` без урахування регістру, видалення
2. **Теги** — дубль тегу не додається, об'єднання, перетин і різниця множин тегів
3. **Пошуковий індекс** — пошук за одним і кількома словами
4. **Custom comparer** — другий `John` не додається в множину

## Звіт

Аналіз прикладів з Dapper і Serilog, порівняльна таблиця складності та висновки — у файлі [REPORT.md](REPORT.md).

## Приклад виводу
<img width="731" height="605" alt="image" src="https://github.com/user-attachments/assets/7a5019f3-3388-407f-b119-a6887aabfb21" />

