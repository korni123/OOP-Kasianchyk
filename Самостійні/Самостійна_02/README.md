# Самостійна робота №2

**Тема:** Перевантаження конструкторів: приклади та сценарії.

**Проєкт:** `IndependentWork2`

## Клас Product

- приватні поля: `_id` (int), `_name` (string), `_price` (decimal), `_category` (string), `_stockCount` (int)
- read-only властивості: `Id`, `Name`, `Price`, `Category`, `StockCount` (тільки `get`, змінити після створення не можна)
- конструктори:
  - **основний** — `Product(int id, string name, decimal price, string category, int stockCount)`, ініціалізує всі поля;
  - **скорочений** — `Product(int id, string name, decimal price) : this(id, name, price, "Uncategorized", 0)`;
  - **копіювання** — `Product(Product other) : this(other.Id, other.Name, other.Price, other.Category, other.StockCount)`.
- `ToString()` перевизначено: `ID, Name, Price (формат :C), Category, Stock`

Скорочений конструктор і конструктор копіювання не дублюють код присвоєння полів — вони передають дані в основний конструктор через `: this(...)`, тому логіка ініціалізації знаходиться в одному місці.

## Що демонструє Main

- товар 1 створено основним конструктором, товар 2 — скороченим (категорія `Uncategorized`, склад 0), товар 3 — копія товару 1;
- для виводу ціни у гривнях встановлено культуру `uk-UA`, тому `:C` дає `₴`;
- в кінці перевіряється, що копія — це окремий об'єкт (`ReferenceEquals` = False).

## Запуск

```
cd Самостійні/Самостійна_02/IndependentWork2
dotnet run
```

## Приклад виводу

