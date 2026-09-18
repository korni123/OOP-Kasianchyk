using System;

public class Movie
{
    private string _title;
    private string _genre;
    private double _rating;

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public string Genre
    {
        get { return _genre; }
        set { _genre = value; }
    }

    public double Rating
    {
        get { return _rating; }
        set
        {
            if (value >= 0 && value <= 10)
                _rating = value;
            else
                Console.WriteLine("Рейтинг має бути в діапазоні 0-10.");
        }
    }

    public Movie() : this("Untitled", "Unknown", 5.0)
    {
    }

    public Movie(string title, string genre, double rating)
    {
        Title = title;
        Genre = genre;
        Rating = rating;
    }

    ~Movie()
    {
        Console.WriteLine($"Об'єкт фільму \"{_title}\" знищено.");
    }

    public void Play()
    {
        Console.WriteLine($"Відтворюється фільм \"{_title}\" (жанр: {_genre}). Рейтинг: {_rating}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Creating objects");

        Movie m1 = new Movie(); 
        Movie m2 = new Movie("Матриця", "Наукова фантастика", 8.7); 
        Movie m3 = new Movie("Початок", "Трилер", 8.8);

        m1.Play();
        m2.Play();
        m3.Play();

        Console.WriteLine("Objects created");
        Console.WriteLine("End of Main, preparing for GC");

        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}