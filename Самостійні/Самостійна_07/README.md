# Самостійна робота №7

**Тема:** Узагальнений кеш `Cache<TKey, TValue>` із тестами.

**Проєкти:** `IndependentWork7` (консольний), `IndependentWork7.Tests` (xUnit)

## Опис класу

Реалізовано узагальнений клас `Cache<TKey, TValue>` з політикою FIFO:
- **Поля:** `_capacity` (int), `_cache` (`Dictionary<TKey, TValue>`), `_keysOrder` (`Queue<TKey>`) — приватні
- **Конструктор:** `Cache(int capacity)` — при ємності ≤ 0 кидає `ArgumentException`
- **Add(key, value):** якщо кеш повний — видаляє найстаріший ключ (`Dequeue`), якщо ключ уже є — оновлює значення
- **Get(key):** повертає значення через `TryGetValue`, якщо ключа немає — `KeyNotFoundException`
- **ContainsKey(key):** перевіряє наявність ключа
- **Властивості:** `Count`, `Capacity`

## Тести

12 юніт-тестів у `CacheTests.cs`: додавання, отримання, оновлення існуючого ключа, витіснення найстарішого при переповненні, порядок FIFO, `KeyNotFoundException` для відсутнього ключа, `ContainsKey`, `Count`, некоректна ємність.

## Демонстрація в Main

1. Створено кеш `Cache<string, string>` на 3 елементи, додано `ua`, `pl`, `de`
2. Додано `fr` — видаляється найстаріший `ua`, потім `it` — видаляється `pl`
3. Спроба `Get("ua")` — перехоплюється `KeyNotFoundException`

## Результат тестів

## Приклад виводу

