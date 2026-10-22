# Самостійна робота №6

**Тема:** Інтерфейс vs абстрактний клас — практичні критерії.

**Проєкт:** `IndependentWork6`

## Сценарій 1: Система сповіщень — інтерфейс

- **INotifier** — інтерфейс з методом `void Send(string message)`
- **EmailNotifier** — поле `_email`, відправка листа
- **SmsNotifier** — поле `_phone`, обрізає довгий текст до 30 символів
- **PushNotifier** — поле `_deviceId`, push-сповіщення на пристрій

## Сценарій 2: Обробка файлів — абстрактний клас

- **FileProcessor** — абстрактний клас: властивості `FilePath`, `Content`; метод `Process(path)` виконує кроки `Open()` → `ProcessContent()` → `Close()`; `abstract void ProcessContent()`
- **TextFileProcessor** — рахує рядки та слова
- **XmlFileProcessor** — розбирає XML через `XDocument`
- **CsvFileProcessor** — рахує записи та суму стовпця Price

## Демонстрація в Main

1. **Сповіщення** — колекція `List<INotifier>` з трьома каналами, для кожного викликається `Send()`
2. **Файли** — тестові файли створюються в коді, колекція `List<FileProcessor>`, для кожного обробника викликається `Process()`

## Звіт

Обґрунтування вибору інтерфейсу / абстрактного класу для кожного сценарію та загальні критерії вибору — у файлі [REPORT.md](REPORT.md).

## Приклад виводу

<img width="761" height="423" alt="image" src="https://github.com/user-attachments/assets/c1839ba2-c851-460e-89fa-b5ec510582ba" />
