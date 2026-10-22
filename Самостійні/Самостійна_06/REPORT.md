# Звіт до самостійної роботи №6

**Тема:** Інтерфейс vs абстрактний клас — практичні критерії.

**Проєкт:** `IndependentWork6`

## Сценарій 1: Система сповіщень

### Обраний підхід: Інтерфейс `INotifier`

### Обґрунтування вибору:

У цьому сценарії краще використовувати інтерфейс `INotifier`, оскільки:

1. **Немає спільної реалізації.** Email, SMS і Push відправляються зовсім по-різному (SMTP, SMS-шлюз, сервіс push-повідомлень). Спільне в них лише «вміти виконати `Send(string message)`» — це саме контракт, а не код.
2. **Різні базові типи.** `EmailNotifier`, `SmsNotifier`, `PushNotifier` можуть мати різних предків (наприклад, `SmsNotifier` може успадковуватися від класу HTTP-клієнта). Клас у C# має тільки одного батька, а інтерфейсів може реалізувати скільки завгодно.
3. **Множинна поведінка.** Той самий клас може бути одночасно `INotifier` і, наприклад, `ILogger` чи `IDisposable`.
4. **Слабке зв'язування.** Клієнтський код залежить тільки від `INotifier` (принцип Dependency Inversion), тому новий канал (Telegram, Viber) додається новим класом без змін у старому коді.

**Чому абстрактний клас тут менш доречний:** абстрактний клас `Notifier` з одним абстрактним методом `Send` не дав би жодного спільного коду, але забрав би у класів можливість мати іншого батька. Це обмеження без користі.

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
        string text = message.Length > 30 ? message.Substring(0, 30) + "..." : message;
        Console.WriteLine($"[SMS] На номер {_phone}: {text}");
    }
}

// Демонстрація в Main
List<INotifier> notifiers = new List<INotifier>();
notifiers.Add(new EmailNotifier("student@college.rv.ua"));
notifiers.Add(new SmsNotifier("+380971234567"));
notifiers.Add(new PushNotifier("android-7f3a"));

foreach (INotifier notifier in notifiers)
{
    notifier.Send("Завтра пара з ООП переноситься на 10:00");
}
```

Реалізації: `EmailNotifier` (адреса, тема), `SmsNotifier` (обрізає довгий текст до 30 символів), `PushNotifier` (ідентифікатор пристрою).

## Сценарій 2: Обробка файлів

### Обраний підхід: Абстрактний клас `FileProcessor`

### Обґрунтування вибору:

1. **Є спільний код.** Відкриття і закриття файлу однакові для всіх обробників — їх написано один раз у базовому класі, а не копіюється в кожен клас.
2. **Фіксований порядок кроків (шаблонний метод).** Публічний метод `Process(path)` завжди виконує: `Open()` → `ProcessContent()` → `Close()`. Нащадок не може «забути» закрити файл або змінити порядок, бо `Open` і `Close` приватні, а `Process` не віртуальний.
3. **Спільний стан.** Шлях до файлу і прочитаний вміст зберігаються в базовому класі (`protected` властивості `FilePath`, `Content`), нащадки тільки читають їх.
4. **Обов'язкова унікальна частина.** `protected abstract void ProcessContent()` змушує кожен обробник реалізувати свою логіку — компілятор не дасть створити клас без неї.

**Чому інтерфейс тут менш доречний:** інтерфейс `IFileProcessor { void Process(string path); }` змусив би кожен клас самостійно писати відкриття/закриття — дублювання коду і ризик помилки. Інтерфейс також не може зберігати стан (поля) і не може гарантувати порядок кроків. (Default-методи інтерфейсів у C# 8+ частково вирішують дублювання, але не дають полів і закритих кроків.)

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

    private void Open()
    {
        Console.WriteLine($"{GetType().Name}: відкриваю файл {Path.GetFileName(FilePath)}");
        Content = File.ReadAllText(FilePath);
    }

    private void Close()
    {
        Content = "";
        Console.WriteLine($"{GetType().Name}: файл закрито");
    }

    protected abstract void ProcessContent();
}

public class XmlFileProcessor : FileProcessor
{
    protected override void ProcessContent()
    {
        XDocument doc = XDocument.Parse(Content);
        Console.WriteLine($"  XML: кореневий елемент <{doc.Root!.Name}>, дочірніх елементів - {doc.Root.Elements().Count()}");
    }
}

// Демонстрація в Main
List<FileProcessor> processors = new List<FileProcessor>();
processors.Add(new TextFileProcessor());
processors.Add(new XmlFileProcessor());
processors.Add(new CsvFileProcessor());
```

Реалізації: `TextFileProcessor` (рахує рядки і слова), `XmlFileProcessor` (розбирає XML через `XDocument`), `CsvFileProcessor` (рахує записи та суму стовпця Price). Тестові файли створюються в коді через `File.WriteAllText`.

## Загальні висновки: практичні критерії вибору

| Ознака сценарію | Вибір |
|---|---|
| Спільний лише «контракт» (що робити), реалізації не мають спільного коду | Інтерфейс |
| Класи з різних ієрархій, потрібна множинна «роль» | Інтерфейс |
| Є спільний код, стан (поля) або конструктор | Абстрактний клас |
| Алгоритм з фіксованими кроками, де змінюється лише частина (шаблонний метод) | Абстрактний клас |
| Відношення «є різновидом» (`XmlFileProcessor` є обробником файлів) | Абстрактний клас |
| Відношення «вміє» (`EmailNotifier` вміє надсилати) | Інтерфейс |

Підходи можна поєднувати: абстрактний клас може реалізовувати інтерфейс (наприклад, `FileProcessor : IFileProcessor`), тоді клієнтський код залежить від інтерфейсу, а спільний код живе в абстрактному класі. В обох сценаріях поліморфізм дозволяє пройтися по колекції базового типу і викликати один метод (`Send` / `Process`), а виконується реалізація конкретного класу.
