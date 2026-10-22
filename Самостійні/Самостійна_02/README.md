# Самостійна робота №2

**Тема:** Перевантаження конструкторів: приклади та сценарії.

**Проєкт:** `IndependentWork2` — клас `Product`

## Опис класу

Реалізовано клас `Product` для інтернет-магазину:
- **Приватні поля:** `_id` (int), `_name` (string), `_price` (decimal), `_category` (string), `_stockCount` (int)
- **Властивості:** `Id`, `Name`, `Price`, `Category`, `StockCount` — тільки для читання
- **Конструктор 1 (основний):** приймає всі п'ять параметрів і ініціалізує поля
- **Конструктор 2 (скорочений):** `Product(int id, string name, decimal price) : this(id, name, price, "Uncategorized", 0)`
- **Конструктор 3 (копіювання):** `Product(Product other) : this(other.Id, other.Name, other.Price, other.Category, other.StockCount)`
- **Метод:** `override ToString()` — повна інформація про товар, ціна у форматі `:C`

## Демонстрація в Main

1. **Товар 1** — створено основним конструктором
2. **Товар 2** — створено скороченим конструктором (категорія `Uncategorized`, склад 0)
3. **Товар 3** — копія товару 1 через конструктор копіювання; перевірка `ReferenceEquals` показує, що це окремий об'єкт

## Приклад виводу

<img width="792" height="269" alt="image" src="https://github.com/user-attachments/assets/b2e7373a-e89a-4b6b-956c-4f7a6856b79c" />
