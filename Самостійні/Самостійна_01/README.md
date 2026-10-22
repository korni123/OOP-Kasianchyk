# Самостійна робота №1

**Тема:** Базовий синтаксис C# і оголошення класів.

**Проєкт:** `IndependentWork1`

## Класи

**Smartphone** — смартфон із рівнем заряду батареї
- поля: `_model` (string), `_batteryLevel` (int)
- властивості: `Model` (get/set), `BatteryLevel` (тільки читання)
- конструктор: `Smartphone(string model, int batteryLevel)`
- методи: `Charge(int percent)`, `Use(int minutes)`, `IsLowBattery()` — перевірка стану (< 20%), `PrintInfo()`

**Movie** — фільм
- поля: `_title` (string), `_durationMinutes` (int), `_rating` (double)
- властивості: `Title` (get/set), `Rating` (get/set з перевіркою 0–10), `DurationMinutes` (тільки читання)
- конструктор: `Movie(string title, int durationMinutes, double rating)`
- методи: `GetDurationText()` — переводить хвилини в «год/хв», `IsRecommended()` — рейтинг ≥ 7.5

**Room** — кімната
- поля: `_name` (string), `_length`, `_width` (double)
- властивості: `Name` (get/set), `Length`, `Width` (тільки читання)
- конструктор: `Room(string name, double length, double width)`
- методи: `GetArea()` — площа, `GetPaintCost(double wallHeight, double pricePerMeter)` — вартість фарбування стін

## Що демонструє Main

- створюється по одному об'єкту кожного класу через конструктор;
- для смартфона: використання → заряд падає, `IsLowBattery()` = True, після зарядки — False (заряд не перевищує 100%);
- для фільму: тривалість у годинах, рекомендація, спроба встановити рейтинг 15 відхиляється властивістю;
- для кімнати: площа та вартість фарбування стін.

## Запуск

```
cd Самостійні/Самостійна_01/IndependentWork1
dotnet run
```

## Приклад виводу
<img width="623" height="381" alt="image" src="https://github.com/user-attachments/assets/92627453-04f4-44f9-9aac-8237222b99b3" />

