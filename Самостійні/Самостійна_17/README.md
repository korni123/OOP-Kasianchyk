# Самостійна робота №17

**Тема:** LINQ pipelines: ETL та аналітика.

Проєкт: `IndependentWork11`. Опис пайплайнів і аналіз продуктивності — у файлі [REPORT.md](REPORT.md).

## Сутності

- `Product`: `Id`, `Name`, `Category`, `UnitPrice`
- `Customer`: `Id`, `Name`, `City`
- `Sale`: `Id`, `ProductId`, `Quantity`, `SaleDate`, `CustomerId`

## Тестові дані

`DataGenerator` створює дані через `Random` з фіксованим seed (42), тому результати однакові при кожному запуску: 60 продуктів у 6 категоріях, 80 клієнтів з 8 міст, 500 продажів з 1 січня 2025 до листопада 2026.

## Пайплайни (клас Pipelines)

1. `TopProductsByCategory` — 3 найпопулярніші продукти в кожній категорії
2. `RevenueByCity` — загальний дохід за містами клієнтів
3. `SalesByMonth` — кількість проданих одиниць і дохід за місяцями та роками
4. `TopCustomersBySpending` (додатково) — 5 клієнтів, які витратили найбільше

Час кожного пайплайну вимірюється окремим `Stopwatch` і виводиться разом з результатом.

## Запуск

```
cd Самостійні/Самостійна
dotnet run --project IndependentWork11 -c Release
```

## Приклад виводу

(скріншот)
