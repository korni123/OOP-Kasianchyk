namespace lab10v9;

public class Song
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int ArtistId { get; set; }
    public int DurationSeconds { get; set; }

    public Song(int id, string title, int artistId, int durationSeconds)
    {
        Id = id;
        Title = title;
        ArtistId = artistId;
        DurationSeconds = durationSeconds;
    }
}
