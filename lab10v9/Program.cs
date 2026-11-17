using System.Diagnostics;
using lab10v9;

List<Artist> artists = new List<Artist>
{
    new Artist(1, "Queen", "Rock"),
    new Artist(2, "Nirvana", "Rock"),
    new Artist(3, "Adele", "Pop"),
    new Artist(4, "Daft Punk", "Electronic"),
    new Artist(5, "Miles Davis", "Jazz"),
    new Artist(6, "Okean Elzy", "Rock")
};

List<Song> songs = new List<Song>
{
    new Song(1, "Bohemian Rhapsody", 1, 354),
    new Song(2, "Another One Bites the Dust", 1, 215),
    new Song(3, "Don't Stop Me Now", 1, 209),
    new Song(4, "Radio Ga Ga", 1, 348),
    new Song(5, "Smells Like Teen Spirit", 2, 301),
    new Song(6, "Come as You Are", 2, 219),
    new Song(7, "Lithium", 2, 257),
    new Song(8, "About a Girl", 2, 169),
    new Song(9, "Hello", 3, 295),
    new Song(10, "Someone Like You", 3, 285),
    new Song(11, "Skyfall", 3, 286),
    new Song(12, "Rolling in the Deep", 3, 228),
    new Song(13, "Get Lucky", 4, 369),
    new Song(14, "One More Time", 4, 320),
    new Song(15, "Harder Better Faster Stronger", 4, 224),
    new Song(16, "So What", 5, 562),
    new Song(17, "Blue in Green", 5, 337),
    new Song(18, "All Blues", 5, 691),
    new Song(19, "Vysoko", 6, 251),
    new Song(20, "Obiymy", 6, 280),
    new Song(21, "Ya ne zdamsya bez boyu", 6, 235),
    new Song(22, "Ne pytay", 6, 194)
};

List<(string Name, double Ms)> timings = new List<(string, double)>();

void Run(string name, Func<List<string>> query)
{
    Stopwatch sw = Stopwatch.StartNew();
    List<string> result = query();
    sw.Stop();
    Console.WriteLine(name);
    foreach (string line in result)
    {
        Console.WriteLine("  " + line);
    }
    double ms = sw.Elapsed.TotalMilliseconds;
    Console.WriteLine($"  Time: {ms:F4} ms");
    Console.WriteLine();
    timings.Add((name, ms));
}

Console.WriteLine($"Artists: {artists.Count}, songs: {songs.Count}");
Console.WriteLine();

Run("1. Rock songs longer than 200 s (query syntax)", () =>
    (from s in songs
     join a in artists on s.ArtistId equals a.Id
     where a.Genre == "Rock" && s.DurationSeconds > 200
     select $"{s.Title} - {a.Name} ({s.DurationSeconds} s)").ToList());

Run("1. Rock songs longer than 200 s (method syntax)", () =>
    songs.Join(artists, s => s.ArtistId, a => a.Id, (s, a) => new { s, a })
         .Where(x => x.a.Genre == "Rock" && x.s.DurationSeconds > 200)
         .Select(x => $"{x.s.Title} - {x.a.Name} ({x.s.DurationSeconds} s)")
         .ToList());

Run("2. Songs with artists (query syntax)", () =>
    (from s in songs
     join a in artists on s.ArtistId equals a.Id
     select $"{s.Title} - {a.Name}").ToList());

Run("2. Songs with artists (method syntax)", () =>
    songs.Join(artists, s => s.ArtistId, a => a.Id, (s, a) => $"{s.Title} - {a.Name}")
         .ToList());

Run("3. Total duration per artist (query syntax)", () =>
    (from a in artists
     join s in songs on a.Id equals s.ArtistId into artistSongs
     select $"{a.Name}: {artistSongs.Sum(x => x.DurationSeconds)} s").ToList());

Run("3. Total duration per artist (method syntax)", () =>
    artists.GroupJoin(songs, a => a.Id, s => s.ArtistId,
                      (a, artistSongs) => $"{a.Name}: {artistSongs.Sum(x => x.DurationSeconds)} s")
           .ToList());

Run("4. Shortest song (query syntax)", () =>
{
    Song shortest = (from s in songs orderby s.DurationSeconds select s).First();
    return new List<string> { $"{shortest.Title} ({shortest.DurationSeconds} s)" };
});

Run("4. Shortest song (method syntax)", () =>
{
    Song shortest = songs.MinBy(s => s.DurationSeconds)!;
    return new List<string> { $"{shortest.Title} ({shortest.DurationSeconds} s)" };
});

Console.WriteLine("Summary (slowest first):");
foreach (var t in timings.OrderByDescending(x => x.Ms))
{
    Console.WriteLine($"  {t.Ms:F4} ms - {t.Name}");
}
