using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public interface INotifier
{
    void Send(string message);
}

public class EmailNotifier : INotifier
{
    private string _email;

    public EmailNotifier(string email)
    {
        _email = email;
    }

    public void Send(string message)
    {
        Console.WriteLine($"[Email] Кому: {_email} | Тема: Сповіщення | Текст: {message}");
    }
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

public class PushNotifier : INotifier
{
    private string _deviceId;

    public PushNotifier(string deviceId)
    {
        _deviceId = deviceId;
    }

    public void Send(string message)
    {
        Console.WriteLine($"[Push] Пристрій {_deviceId}: 🔔 {message}");
    }
}

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
        Console.WriteLine();
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

public class TextFileProcessor : FileProcessor
{
    protected override void ProcessContent()
    {
        string[] lines = Content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        int words = Content.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        Console.WriteLine($"  Текстовий файл: рядків - {lines.Length}, слів - {words}");
    }
}

public class XmlFileProcessor : FileProcessor
{
    protected override void ProcessContent()
    {
        XDocument doc = XDocument.Parse(Content);
        Console.WriteLine($"  XML: кореневий елемент <{doc.Root!.Name}>, дочірніх елементів - {doc.Root.Elements().Count()}");
        foreach (XElement el in doc.Root.Elements())
        {
            Console.WriteLine($"    {el.Attribute("name")?.Value} - {el.Value} грн");
        }
    }
}

public class CsvFileProcessor : FileProcessor
{
    protected override void ProcessContent()
    {
        string[] lines = Content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Console.WriteLine($"  CSV: заголовок [{lines[0].Trim()}], записів - {lines.Length - 1}");
        decimal sum = 0;
        for (int i = 1; i < lines.Length; i++)
        {
            sum += Convert.ToDecimal(lines[i].Split(';')[1].Trim());
        }
        Console.WriteLine($"  Сума стовпця Price: {sum}");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Сценарій 1: Система сповіщень (інтерфейс INotifier) ===");
        List<INotifier> notifiers = new List<INotifier>();
        notifiers.Add(new EmailNotifier("student@college.rv.ua"));
        notifiers.Add(new SmsNotifier("+380971234567"));
        notifiers.Add(new PushNotifier("android-7f3a"));

        foreach (INotifier notifier in notifiers)
        {
            notifier.Send("Завтра пара з ООП переноситься на 10:00");
        }

        Console.WriteLine();
        Console.WriteLine("=== Сценарій 2: Обробка файлів (абстрактний клас FileProcessor) ===");
        string dir = AppContext.BaseDirectory;
        string txt = Path.Combine(dir, "notes.txt");
        string xml = Path.Combine(dir, "products.xml");
        string csv = Path.Combine(dir, "prices.csv");
        File.WriteAllText(txt, "Перший рядок файлу\nДругий рядок\nТретій рядок тексту");
        File.WriteAllText(xml, "<products><product name=\"Мишка\">800</product><product name=\"Клавіатура\">1500</product></products>");
        File.WriteAllText(csv, "Name;Price\nМонітор;7200\nНавушники;1300\nКабель;150");

        List<FileProcessor> processors = new List<FileProcessor>();
        processors.Add(new TextFileProcessor());
        processors.Add(new XmlFileProcessor());
        processors.Add(new CsvFileProcessor());
        string[] files = { txt, xml, csv };

        for (int i = 0; i < processors.Count; i++)
        {
            processors[i].Process(files[i]);
        }
    }
}
