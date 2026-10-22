# Самостійна робота №8

**Тема:** Вивчення `Dictionary<TKey, TValue>` та `HashSet<T>` у бібліотеках .NET.

**Проєкт:** `IndependentWork8`

Звіт з аналізом прикладів з open-source (Dapper, Serilog, Newtonsoft.Json), порівняльною таблицею складності та висновками — у файлі [REPORT.md](REPORT.md).

## Реалізовані сценарії

- **Сценарій 1 — `FileMetadataCache` (Dictionary):** кеш метаданих файлів `Dictionary<string, FileMetadata>` без урахування регістру назви; `Add`, `Get`, `Remove`, `Contains`.
- **Сценарій 2 — `TagManager` (Dictionary + HashSet):** теги статей `Dictionary<string, HashSet<string>>`; `AddTag`, `RemoveTag`, `GetAllUniqueTags` (`UnionWith`), `FindCommonTags` (`IntersectWith`), `FindUniqueTags` (`ExceptWith`).
- **Сценарій 3 — `SearchIndex` (Dictionary + HashSet):** інвертований індекс `Dictionary<string, HashSet<int>>`; `AddDocument`, `Search`, `SearchMultiple` (AND через `IntersectWith`).
- **Custom comparer:** `HashSet<Person>` з `PersonNameComparer` — люди з однаковим ім'ям вважаються одним елементом.

## Запуск

```
cd Самостійні/Самостійна_08/IndependentWork8
dotnet run
```

## Приклад виводу

