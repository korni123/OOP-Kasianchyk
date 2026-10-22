using System;
using System.Collections.Generic;

public class LibraryItem
{
    private string _uniqueId;
    private string _title;
    private int _publicationYear;

    public string UniqueId
    {
        get { return _uniqueId; }
    }

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public int PublicationYear
    {
        get { return _publicationYear; }
        set { _publicationYear = value; }
    }

    public LibraryItem(string uniqueId, string title, int publicationYear)
    {
        _uniqueId = uniqueId;
        _title = title;
        _publicationYear = publicationYear;
    }

    public virtual void DisplayDetails()
    {
        Console.WriteLine($"[{_uniqueId}] \"{_title}\", рік видання: {_publicationYear}");
    }
}

public class Book : LibraryItem
{
    private string _author;

    public string Author
    {
        get { return _author; }
        set { _author = value; }
    }

    public Book(string uniqueId, string title, int publicationYear, string author)
        : base(uniqueId, title, publicationYear)
    {
        _author = author;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"    Книга, автор: {_author}");
    }
}

public class Journal : LibraryItem
{
    private int _issueNumber;

    public int IssueNumber
    {
        get { return _issueNumber; }
        set { _issueNumber = value; }
    }

    public Journal(string uniqueId, string title, int publicationYear, int issueNumber)
        : base(uniqueId, title, publicationYear)
    {
        _issueNumber = issueNumber;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"    Журнал, випуск №{_issueNumber}");
    }
}

public class AudioBook : Book
{
    private string _narrator;

    public string Narrator
    {
        get { return _narrator; }
        set { _narrator = value; }
    }

    public AudioBook(string uniqueId, string title, int publicationYear, string author, string narrator)
        : base(uniqueId, title, publicationYear, author)
    {
        _narrator = narrator;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"    Аудіокнига, читає: {_narrator}");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Book та Journal ===");
        Book book = new Book("BK-001", "Кобзар", 1840, "Тарас Шевченко");
        Journal journal = new Journal("JR-014", "Наука і життя", 2024, 7);
        book.DisplayDetails();
        journal.DisplayDetails();

        Console.WriteLine();
        Console.WriteLine("=== AudioBook ===");
        AudioBook audioBook = new AudioBook("AB-003", "Тіні забутих предків", 1911, "Михайло Коцюбинський", "Остап Ступка");
        audioBook.DisplayDetails();

        Console.WriteLine();
        Console.WriteLine("=== Уся ієрархія через List<LibraryItem> ===");
        List<LibraryItem> items = new List<LibraryItem> { book, journal, audioBook };
        foreach (LibraryItem item in items)
        {
            Console.WriteLine($"{item.GetType().Name}, UniqueId = {item.UniqueId}");
            item.DisplayDetails();
        }
    }
}
