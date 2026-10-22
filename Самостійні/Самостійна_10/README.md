# Самостійна робота №10

**Тема:** Колекції в практиці ООП: інвертований індекс і вимірювання продуктивності.

**Проєкт:** `InvertedIndexDemo`

## Опис класів

- **Document** — `Id`, `Title`, `Content`, `ToString()` у форматі `[Id] Title`
- **TextProcessor** — `Tokenize` (нижній регістр + `Regex`, прибирає пунктуацію), `RemoveStopWords`
- **InvertedIndex** — `Dictionary<string, HashSet<int>>` (слово → документи) і `Dictionary<int, Document>`; методи `AddDocument`, `SearchSingleWord`, `SearchMultipleWords` (AND через `IntersectWith`), `SearchAnyWord` (OR через `UnionWith`)
- **PerformanceTest** — порівнює лінійний пошук і пошук через індекс з однаковою нормалізацією тексту

## Демонстрація в Main

1. **Вимірювання** — для 10, 100, 1000 і 10 000 документів виводиться час лінійного пошуку, пошуку в індексі та побудови індексу
2. **Інтерактивний пошук** — 5 документів, пошук за всіма введеними словами, вихід — `exit`

## Звіт

Таблиця вимірювань, аналіз і висновок про поріг вигідності індексу — у файлі [REPORT.md](REPORT.md).

## Приклад виводу

```
Loaded 5 documents
Index contains 37 unique words

Enter search query (or 'exit'): programming language

--- Search Results (AND) ---
[1] Introduction to C#
  C# is a modern object-oriented programming language developed by Microsoft....
[2] Java Basics
  Java is a popular programming language known for its portability and performance....
[3] Python Overview
  Python is a high-level programming language with simple syntax....

Found 3 documents

Enter search query (or 'exit'): c# async

--- Search Results (AND) ---
[4] C# Advanced Features
  C# supports LINQ, async/await, and many modern programming paradigms....

Found 1 documents
```

