# Самостійна робота №13

**Тема:** Узагальнений кеш Cache&lt;TKey, TValue&gt; із тестами.

Проєкти: `IndependentWork13` (основний) і `IndependentWork13.Tests` (xUnit-тести).

## Cache&lt;TKey, TValue&gt;

Кеш з обмеженою ємністю. Коли кеш повний, новий елемент витісняє найстаріший (політика FIFO).

- Поля: `_capacity` (int), `_cache` (Dictionary&lt;TKey, TValue&gt;), `_keysOrder` (Queue&lt;TKey&gt;)
- Конструктор: `Cache(int capacity)` — для ємності менше 1 кидає `ArgumentOutOfRangeException`
- `Add(TKey key, TValue value)` — додає елемент; якщо ключ уже є, оновлює значення; якщо кеш повний, видаляє найстаріший ключ
- `Get(TKey key)` — повертає значення через `TryGetValue`; якщо ключа немає, кидає `KeyNotFoundException`
- `ContainsKey(TKey key)` — перевіряє, чи є ключ
- `Count` — кількість елементів

## Тести

Клас `CacheTests`, 12 тестів: додавання, оновлення існуючого ключа, `Get`, неіснуючий ключ, видалення найстарішого при переповненні, порядок FIFO, `ContainsKey`, `Count`, неправильна ємність.

## Що показує Main

- `Cache<string, string>` з ємністю 3: додавання, отримання, витіснення найстарішого, оновлення, помилка для відсутнього ключа
- `Cache<int, double>` з ємністю 2

## Запуск

```
cd Самостійні/Самостійна_13
dotnet run --project IndependentWork13
dotnet test IndependentWork13.Tests
```
