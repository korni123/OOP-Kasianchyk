# Самостійна робота №7

**Тема:** Узагальнений кеш `Cache<TKey, TValue>` із тестами.

**Проєкти:** `IndependentWork7` (консольний), `IndependentWork7.Tests` (xUnit, посилається на основний проєкт), рішення `IndependentWork7.slnx`.

## Клас Cache<TKey, TValue> (`IndependentWork7/Cache.cs`)

- приватні поля:
  - `_capacity` (int) — максимальна кількість елементів;
  - `_cache` (`Dictionary<TKey, TValue>`) — ключі та значення, пошук за O(1);
  - `_keysOrder` (`Queue<TKey>`) — порядок додавання ключів; найстаріший завжди на початку черги.
- `Cache(int capacity)` — конструктор; при ємності ≤ 0 кидає `ArgumentException`.
- `Add(TKey key, TValue value)`:
  - якщо ключ уже є — оновлює значення (порядок у черзі не змінюється, нічого не видаляється);
  - якщо кеш повний — `Dequeue()` найстарішого ключа і видалення його зі словника (політика **FIFO**);
  - потім додає новий елемент у словник і в кінець черги.
- `Get(TKey key)` — через `TryGetValue`; якщо ключа немає — кидає `KeyNotFoundException` зі своїм повідомленням.
- `ContainsKey(TKey key)` — перевірка наявності.
- `Count` — поточна кількість елементів, `Capacity` — ємність.

## Юніт-тести (`IndependentWork7.Tests/CacheTests.cs`)

| Тест | Що перевіряє |
|---|---|
| `Constructor_ZeroCapacity_ThrowsArgumentException` | не можна створити кеш з ємністю 0 |
| `Add_NewItem_IncreasesCount` | додавання збільшує `Count` |
| `Add_ExistingKey_UpdatesValueWithoutGrowing` | повторний ключ оновлює значення, `Count` не росте |
| `Get_ExistingKey_ReturnsValue` | отримання значень за ключем |
| `Get_MissingKey_ThrowsKeyNotFoundException` | неіснуючий ключ → `KeyNotFoundException` |
| `Add_OverCapacity_RemovesOldestItem` | при переповненні видаляється найстаріший елемент |
| `Add_OverCapacityTwice_RemovesItemsInFifoOrder` | кілька витіснень підряд ідуть строго по черзі |
| `Get_DoesNotChangeFifoOrder` | читання не впливає на порядок витіснення (це FIFO, а не LRU) |
| `Get_EvictedKey_ThrowsKeyNotFoundException` | витіснений ключ більше недоступний |
| `ContainsKey_ReturnsTrueForAddedAndFalseForMissing` | метод `ContainsKey` |
| `Count_EmptyCache_IsZero` | `Count` порожнього кешу |
| `Count_NeverExceedsCapacity` | після 10 додавань у кеш на 3 лишаються 3 останні елементи |

Тести написані за схемою Arrange–Act–Assert, кожен перевіряє одну річ — якщо щось зламається, одразу видно, який метод винен.

## Що демонструє Main

Кеш `Cache<string, string>` на 3 елементи: додаються країни `ua, pl, de`, потім `fr` → витісняється `ua`, потім `it` → витісняється `pl`, і спроба `Get("ua")` ловить `KeyNotFoundException`.

## Запуск

```
cd Самостійні/Самостійна_07
dotnet test
cd IndependentWork7
dotnet run
```

## Приклад виводу

