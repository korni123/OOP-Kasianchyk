# Самостійна робота №5

**Тема:** Приклади поліморфізму в .NET.

**Проєкт:** `IndependentWork5`

## Опис класів

- **Product, Customer, Order** — власні класи з перевизначеним `override ToString()`
- **Note** — клас без перевизначення `ToString()` (для порівняння)
- **MyCustomCollection** — реалізує `IEnumerable<string>`, всередині приватний `List<string>`; метод `Add`, властивість `Count`, `GetEnumerator()` через `yield return`
- **ProcessStream(Stream stream)** — метод, що приймає будь-який потік, читає перші байти і вміст

## Демонстрація в Main

1. **Object** — об'єкти різних типів у `List<object>`, при виводі викликається `ToString()` фактичного типу
2. **IEnumerable** — `MyCustomCollection` перебирається через `foreach`, а метод `PrintAll` однаково працює з нею, `List<string>` і масивом
3. **Stream** — `ProcessStream` викликається для `FileStream` (тестовий файл створюється в коді) і `MemoryStream`

## Звіт

Як проявляється поліморфізм у кожному сценарії та які переваги він дає — у файлі [REPORT.md](REPORT.md).

## Приклад виводу

<img width="579" height="477" alt="image" src="https://github.com/user-attachments/assets/39ba3864-1f2c-4f33-b3a0-310af10237bf" />
