# Самостійна робота №3

**Тема:** Аналіз інкапсуляції в open-source проєктах. Практики валідації полів.

**Обраний проєкт:** [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)

## Проаналізовані класи

- **JsonTextWriter** — приватні та `readonly` поля, властивості з валідацією в `set` (`Indentation`, `QuoteChar`), перевірка аргументу в конструкторі
- **JsonSerializerSettings** — `internal` поля з nullable-значеннями, автоматичні властивості, валідація `MaxDepth`
- **JObject / JToken** — індексатор `this[string propertyName]`, властивість з `internal set`, перевантажені оператори перетворення типів

## Звіт

Повний аналіз з фрагментами коду, посиланнями на файли та висновками — у файлі [REPORT.md](REPORT.md).
