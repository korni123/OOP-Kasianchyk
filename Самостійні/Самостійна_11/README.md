# Самостійна робота №11

**Тема:** LINQ pipelines: ETL та аналітика.

**Проєкт:** `IndependentWork11`

## Опис класів

- **Sale** — `Id`, `ProductId`, `Quantity`, `SaleDate`, `CustomerId`
- **Product** — `Id`, `Name`, `Category`, `UnitPrice`
- **Customer** — `Id`, `Name`, `City`
- **DataGenerator** — генерує 60 товарів, 80 клієнтів і 600 продажів (`Random` з фіксованим seed = 42)

## Пайплайни

1. **TopProductsByCategory()** — топ-3 продукти в кожній категорії за кількістю продажів (`GroupBy`, `Join`, `OrderByDescending`, `Take`)
2. **RevenueByCity()** — загальний дохід за містами клієнтів (два `Join`, `GroupBy`, `Sum`, `OrderByDescending`)
3. **SalesByYearAndMonth()** — кількість і дохід за кожен місяць (`GroupBy` за роком і місяцем, `OrderBy`, `ThenBy`)
4. **TopCustomersWithFavoriteCategory()** (власний) — топ-5 клієнтів за витратами та їхня улюблена категорія (`Join`, `GroupBy`, `Aggregate`, `Take`)

Для кожного пайплайну час вимірюється окремим `Stopwatch`.

## Демонстрація в Main

1. Генеруються тестові дані
2. Виконуються чотири пайплайни, після кожного виводиться результат і час виконання
3. В кінці — таблиця часу: перший запуск і середнє після прогріву

## Звіт

Опис операторів кожного пайплайну, аналіз вимірювань і висновки — у файлі [REPORT.md](REPORT.md).

## Приклад виводу

