using System;

public class Smartphone
{
    private string _model;
    private int _batteryLevel;

    public string Model
    {
        get { return _model; }
        set { _model = value; }
    }

    public int BatteryLevel
    {
        get { return _batteryLevel; }
    }

    public Smartphone(string model, int batteryLevel)
    {
        _model = model;
        _batteryLevel = batteryLevel;
    }

    public void Charge(int percent)
    {
        _batteryLevel = _batteryLevel + percent;
        if (_batteryLevel > 100) _batteryLevel = 100;
    }

    public void Use(int minutes)
    {
        _batteryLevel = _batteryLevel - minutes / 6;
        if (_batteryLevel < 0) _batteryLevel = 0;
    }

    public bool IsLowBattery()
    {
        return _batteryLevel < 20;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Смартфон {_model}, заряд: {_batteryLevel}%");
    }
}

public class Movie
{
    private string _title;
    private int _durationMinutes;
    private double _rating;

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public double Rating
    {
        get { return _rating; }
        set
        {
            if (value >= 0 && value <= 10) _rating = value;
        }
    }

    public int DurationMinutes
    {
        get { return _durationMinutes; }
    }

    public Movie(string title, int durationMinutes, double rating)
    {
        _title = title;
        _durationMinutes = durationMinutes;
        _rating = rating;
    }

    public string GetDurationText()
    {
        return $"{_durationMinutes / 60} год {_durationMinutes % 60} хв";
    }

    public bool IsRecommended()
    {
        return _rating >= 7.5;
    }
}

public class Room
{
    private string _name;
    private double _length;
    private double _width;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public double Length
    {
        get { return _length; }
    }

    public double Width
    {
        get { return _width; }
    }

    public Room(string name, double length, double width)
    {
        _name = name;
        _length = length;
        _width = width;
    }

    public double GetArea()
    {
        return _length * _width;
    }

    public double GetPaintCost(double wallHeight, double pricePerMeter)
    {
        double wallsArea = 2 * (_length + _width) * wallHeight;
        return wallsArea * pricePerMeter;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Smartphone ===");
        Smartphone phone = new Smartphone("Samsung Galaxy A55", 35);
        phone.PrintInfo();
        phone.Use(120);
        Console.WriteLine("Після 120 хв використання:");
        phone.PrintInfo();
        Console.WriteLine($"Низький заряд: {phone.IsLowBattery()}");
        phone.Charge(90);
        Console.WriteLine("Після зарядки на 90%:");
        phone.PrintInfo();
        Console.WriteLine($"Низький заряд: {phone.IsLowBattery()}");

        Console.WriteLine();
        Console.WriteLine("=== Movie ===");
        Movie movie = new Movie("Інтерстеллар", 169, 8.7);
        Console.WriteLine($"Фільм: {movie.Title}");
        Console.WriteLine($"Тривалість: {movie.GetDurationText()}");
        Console.WriteLine($"Рейтинг: {movie.Rating}, рекомендовано: {movie.IsRecommended()}");
        movie.Rating = 15;
        Console.WriteLine($"Спроба поставити рейтинг 15 -> рейтинг лишився {movie.Rating}");

        Console.WriteLine();
        Console.WriteLine("=== Room ===");
        Room room = new Room("Вітальня", 5.2, 3.8);
        Console.WriteLine($"Кімната: {room.Name}, {room.Length} x {room.Width} м");
        Console.WriteLine($"Площа: {room.GetArea():F2} м²");
        Console.WriteLine($"Вартість фарбування стін (висота 2.7 м, 120 грн/м²): {room.GetPaintCost(2.7, 120):F2} грн");
    }
}
