# Самостійна робота №4

**Тема:** Розширення ієрархії та наслідки змін базового класу.

**Проєкт:** `IndependentWork4` — ієрархія `LibraryItem → Book, Journal`, `Book → AudioBook`

## Опис ієрархії класів

### LibraryItem (базовий клас)

- **Поля:** `_uniqueId`, `_title` (string), `_publicationYear` (int) — приватні
- **Властивості:** `UniqueId` (тільки для читання), `Title`, `PublicationYear`
- **Конструктор:** `LibraryItem(string uniqueId, string title, int publicationYear)`
- **Віртуальний метод:** `virtual void DisplayDetails()` — базова інформація

### Book (похідний від LibraryItem)

- **Поле:** `_author` (string), **властивість:** `Author`
- **Конструктор:** `Book(string uniqueId, string title, int publicationYear, string author) : base(...)`
- **Перевизначення:** `override void DisplayDetails()` — додає автора

### Journal (похідний від LibraryItem)

- **Поле:** `_issueNumber` (int), **властивість:** `IssueNumber`
- **Конструктор:** `Journal(string uniqueId, string title, int publicationYear, int issueNumber) : base(...)`
- **Перевизначення:** `override void DisplayDetails()` — додає номер випуску

### AudioBook (похідний від Book)

- **Поле:** `_narrator` (string), **властивість:** `Narrator`
- **Конструктор:** `AudioBook(string uniqueId, string title, int publicationYear, string author, string narrator) : base(...)`
- **Перевизначення:** `override void DisplayDetails()` — додає оповідача

## Демонстрація в Main

1. **Book і Journal** — створено по одному об'єкту, викликано `DisplayDetails()`
2. **AudioBook** — виводить дані з усіх трьох рівнів ієрархії
3. **Поліморфізм** — усі об'єкти в `List<LibraryItem>`, для кожного викликається своя версія `DisplayDetails()`

## Звіт

Які зміни довелося внести в похідні класи після додавання `UniqueId`, чому, та переваги й недоліки наслідування — у файлі [REPORT.md](REPORT.md).

## Приклад виводу

<img width="571" height="440" alt="image" src="https://github.com/user-attachments/assets/ffb44159-52c6-4050-a6e0-26581ea790f9" />
