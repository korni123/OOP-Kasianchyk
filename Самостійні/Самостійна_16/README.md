# Самостійна робота №16

**Тема:** Створення інвертованого індексу для пошуку документів.

Проєкт: `IndependentWork16`. Таблиця вимірювань і висновки — у файлі [REPORT.md](REPORT.md).

## Класи

### Document

- Властивості: `Id` (int), `Title` (string), `Content` (string)

### TextProcessor

- `Tokenize(string text)` — розбиває текст на слова в нижньому регістрі, без розділових знаків
- `RemoveStopWords(IEnumerable<string> words)` — прибирає стоп-слова ("the", "is", "and" тощо)

### InvertedIndex

- Поля: `Dictionary<string, HashSet<int>> _index` (слово → множина Id документів), `Dictionary<int, Document> _documents`
- `AddDocument(Document doc)` — додає документ в індекс
- `SearchSingleWord(string word)` — документи зі словом
- `SearchMultipleWords(params string[] words)` — документи, де є всі слова (AND, `IntersectWith`)
- `SearchAnyWord(params string[] words)` — документи, де є хоча б одне слово (OR, `UnionWith`)
- Властивості: `DocumentCount`, `UniqueWords`

### PerformanceTest

- `GenerateDocuments(int count)` — створює задану кількість документів з випадкових слів
- `CompareSearchMethods(documents, searchWord)` — порівнює лінійний пошук з пошуком через індекс за часом (`Stopwatch`)

## Що показує Main

- завантаження 5 документів і кількість унікальних слів в індексі
- пошук за кількома словами (AND), за будь-яким зі слів (OR), за одним словом
- видалення стоп-слів з речення
- порівняння швидкості для 5, 10, 100, 1000 і 10000 документів

## Запуск

```
cd Самостійні/Самостійна_16
dotnet run --project IndependentWork16 -c Release
```
