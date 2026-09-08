using System;

public class Movie
{
    private string title;
    private string genre;
    private double rating;

    public double Rating
    {
        get { return rating; }
        set
        {
            if (value >= 0 && value <= 10)
                rating = value;
            else
                Console.WriteLine("Рейтинг має бути в діапазоні 0-10.");
        }
    }

    public Movie(string title, string genre, double rating)
    {
        this.title = title;
        this.genre = genre;
        this.Rating = rating;
    }

    ~Movie()
    {
        Console.WriteLine($"Об'єкт фільму \"{title}\" знищено.");
    }

    public void Play()
    {
        Console.WriteLine($"Відтворюється фільм \"{title}\" (жанр: {genre}). Рейтинг: {rating}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Movie m1 = new Movie("Матриця", "Наукова фантастика", 8.7);
        Movie m2 = new Movie("Начало", "Трилер", 8.8);
        Movie m3 = new Movie("Втеча з Шоушенка", "Драма", 9.3);

        m1.Play();
        m2.Play();
        m3.Play();

        m1.Rating = 9.0;
        Console.WriteLine($"Оновлений рейтинг: {m1.Rating}");
    }
}