# Лабораторна робота №8

**Тема:** Делегати й лямбда-вирази у практиці колекцій: CollectionHelper<T>.

**Варіант:** 9

Проєкт: `lab8v9`.

## Метод за варіантом

```csharp
public static T First(IEnumerable<T> collection, Predicate<T> predicate)
```

Повертає перший елемент колекції, який задовольняє умову `predicate`. Якщо такого елемента немає, кидає `InvalidOperationException`.

Метод написаний власним кодом: цикл `foreach` перебирає елементи, і перший елемент, для якого `predicate` повертає `true`, одразу повертається. LINQ-оператор `First` не використовується.

## Структура

- `CollectionHelper<T>` — статичний узагальнений клас з методом `First`
- `Product` — допоміжний клас (Id, Name, Category, Price) для демонстрації
- `Program.cs` — демонстрація в `Main`

## Що показує Main

- `List<int>`: перше парне число, перше число більше 15
- `List<string>`: перший рядок, що починається на "a", перший рядок довший за 6 букв
- `List<Product>`: перший продукт з ціною більше 500, перший продукт з категорії Computers
- випадок, коли нічого не знайдено: ловиться `InvalidOperationException` для `List<int>` і `List<Product>`

## Запуск

```
cd OOP-Kasianchyk
dotnet run --project lab8v9
```

## Приклад виводу

<img width="975" height="564" alt="image" src="https://github.com/user-attachments/assets/2a0b0f25-2b89-47f0-b28a-5f5203d1c548" />

