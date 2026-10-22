# Самостійна робота №10

**Тема:** Колекції в практиці ООП: інвертований індекс і вимірювання продуктивності.

**Проєкт:** `InvertedIndexDemo`

Опис реалізації, таблиця вимірювань для 10/100/1000/10000 документів, аналіз і висновок про поріг вигідності індексу — у файлі [REPORT.md](REPORT.md).

## Що робить програма

1. **Вимірювання продуктивності** — для 10, 100, 1000 і 10 000 документів порівнює лінійний пошук і `InvertedIndex` для слів `programming` та `microsoft`, виводить час (мкс і тіки `Stopwatch`), час побудови індексу і перевірку, що обидва способи знайшли однакову кількість документів.
2. **Інтерактивний пошук** — завантажує 5 тестових документів, показує кількість документів і унікальних слів, демонструє OR-пошук (`SearchAnyWord`), а далі в циклі шукає документи, що містять **усі** введені слова (`SearchMultipleWords`). Вихід — `exit`.

## Запуск

```
cd Самостійні/Самостійна_10/InvertedIndexDemo
dotnet run
```

## Приклад виводу

```
=== Частина 4: Інтерактивна демонстрація ===
Loaded 5 documents
Index contains 37 unique words

Демо OR-пошуку: SearchAnyWord("python", "linq")
  [3] Python Overview
  [4] C# Advanced Features
  [5] Comparing Languages

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

Enter search query (or 'exit'): exit
```

