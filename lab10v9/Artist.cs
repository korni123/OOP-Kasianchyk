namespace lab10v9;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }

    public Artist(int id, string name, string genre)
    {
        Id = id;
        Name = name;
        Genre = genre;
    }
}
