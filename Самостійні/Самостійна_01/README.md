# Самостійна робота №1

**Тема:** Базовий синтаксис C# і оголошення класів.

**Проєкт:** `IndependentWork1` — класи `Smartphone`, `Movie`, `Room`

## Опис класів

### Smartphone

- **Поля:** `_model` (string), `_batteryLevel` (int) — приватні
- **Властивості:** `Model`, `BatteryLevel` (тільки для читання)
- **Конструктор:** `Smartphone(string model, int batteryLevel)`
- **Методи:** `Charge(int percent)`, `Use(int minutes)`, `IsLowBattery()` — перевірка заряду (< 20%), `PrintInfo()`

### Movie

- **Поля:** `_title` (string), `_durationMinutes` (int), `_rating` (double) — приватні
- **Властивості:** `Title`, `Rating` (з перевіркою діапазону 0–10), `DurationMinutes` (тільки для читання)
- **Конструктор:** `Movie(string title, int durationMinutes, double rating)`
- **Методи:** `GetDurationText()` — тривалість у форматі «год/хв», `IsRecommended()` — рейтинг ≥ 7.5

### Room

- **Поля:** `_name` (string), `_length`, `_width` (double) — приватні
- **Властивості:** `Name`, `Length`, `Width` (тільки для читання)
- **Конструктор:** `Room(string name, double length, double width)`
- **Методи:** `GetArea()` — площа, `GetPaintCost(double wallHeight, double pricePerMeter)` — вартість фарбування стін

## Демонстрація в Main

1. **Smartphone** — після використання заряд падає і `IsLowBattery()` повертає True, після зарядки — False (заряд не більше 100%)
2. **Movie** — виводиться тривалість і рекомендація, спроба поставити рейтинг 15 відхиляється властивістю
3. **Room** — обчислюється площа кімнати та вартість фарбування стін

## Приклад виводу

<img width="623" height="381" alt="image" src="https://github.com/user-attachments/assets/92627453-04f4-44f9-9aac-8237222b99b3" />
