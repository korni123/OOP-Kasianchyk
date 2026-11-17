# Самостійна робота №15

**Тема:** Делегати й лямбда-вирази: власні утиліти для колекцій.

Проєкт: `lab9-self-v9`. Відповіді на запитання завдання 4 — у файлі [REPORT.md](REPORT.md).

## CollectionUtils

Статичний клас з узагальненими методами. Усі методи написані власним кодом (цикл `foreach`), без LINQ.

- `Map<TSource, TResult>(collection, Func<TSource, TResult> mapper)` — перетворює кожен елемент
- `MaxBy<T, TKey>(collection, Func<T, TKey> keySelector)` — знаходить елемент з найбільшим ключем (`where TKey : IComparable<TKey>`); для порожньої колекції кидає `InvalidOperationException`
- `Filter<T>(collection, Predicate<T> predicate)` — залишає елементи, що задовольняють умову
- `ForEach<T>(collection, Action<T> action)` — виконує дію для кожного елемента

## Product

- Властивості: `Name` (string), `Price` (decimal)

## Що показує Main

Три колекції: `List<int>`, `List<string>`, `List<Product>`. Для кожної викликаються всі чотири методи з різними лямбда-виразами.

## Запуск

```
cd Самостійні/Самостійна
dotnet run --project lab9-self-v9
```

## Приклад виводу

(скріншот)
