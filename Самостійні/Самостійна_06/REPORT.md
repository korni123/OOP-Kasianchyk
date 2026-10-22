# Звіт до самостійної роботи №6

## Сценарій 1: Система сповіщень

### Обраний підхід: Інтерфейс `INotifier`

### Обґрунтування вибору:

1. **Немає спільного коду.** Email, SMS і Push відправляються зовсім по-різному, спільний у них тільки метод `Send(string message)`.
2. **Різні базові типи.** Класи можуть мати різних батьків, а інтерфейсів клас може реалізувати скільки завгодно.
3. **Слабке зв'язування.** Код у `Main` залежить тільки від `INotifier`, тому новий канал (наприклад, Telegram) додається новим класом без змін старого коду.

Абстрактний клас тут менш доречний: він не дав би спільного коду, але забрав би можливість успадкуватися від іншого класу.

### Реалізація:

```csharp
public interface INotifier
{
    void Send(string message);
}

public class SmsNotifier : INotifier
{
    private string _phone;

    public SmsNotifier(string phone)
    {
        _phone = phone;
    }

    public void Send(string message)
    {
        Console.WriteLine($"[SMS] На номер {_phone}: {message}");
    }
}

List<INotifier> notifiers = new List<INotifier>();
notifiers.Add(new EmailNotifier("student@college.rv.ua"));
notifiers.Add(new SmsNotifier("+380971234567"));
notifiers.Add(new PushNotifier("android-7f3a"));

foreach (INotifier notifier in notifiers)
{
    notifier.Send("Завтра пара з ООП переноситься на 10:00");
}
```

## Сценарій 2: Обробка файлів

### Обраний підхід: Абстрактний клас `FileProcessor`

### Обґрунтування вибору:

1. **Є спільний код.** Відкриття і закриття файлу однакові для всіх обробників — написані один раз у базовому класі.
2. **Фіксований порядок кроків.** Метод `Process()` завжди виконує `Open()` → `ProcessContent()` → `Close()`, і похідний клас не може це порушити.
3. **Спільні дані.** Шлях до файлу і вміст зберігаються в базовому класі.

Інтерфейс тут менш доречний: кожен клас мав би сам писати відкриття і закриття файлу — це дублювання коду.

### Реалізація:

```csharp
public abstract class FileProcessor
{
    protected string FilePath { get; private set; } = "";
    protected string Content { get; private set; } = "";

    public void Process(string filePath)
    {
        FilePath = filePath;
        Open();
        ProcessContent();
        Close();
    }

    private void Open() { Content = File.ReadAllText(FilePath); ... }
    private void Close() { Content = ""; ... }

    protected abstract void ProcessContent();
}

public class TextFileProcessor : FileProcessor
{
    protected override void ProcessContent()
    {
        string[] lines = Content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Console.WriteLine($"  Текстовий файл: рядків - {lines.Length}");
    }
}

List<FileProcessor> processors = new List<FileProcessor>();
processors.Add(new TextFileProcessor());
processors.Add(new XmlFileProcessor());
processors.Add(new CsvFileProcessor());
```

## Загальні висновки

- **Інтерфейс** обираємо, коли спільний тільки «контракт» (що робити), а реалізації зовсім різні, або класи належать до різних ієрархій.
- **Абстрактний клас** обираємо, коли є спільний код, спільні дані або однаковий порядок кроків, де змінюється лише частина.
- Можна поєднувати: абстрактний клас може реалізовувати інтерфейс.
- В обох сценаріях працює поліморфізм: в одному циклі для колекції базового типу викликається потрібна реалізація кожного класу.
