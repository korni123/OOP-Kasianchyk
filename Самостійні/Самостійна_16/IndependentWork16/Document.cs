namespace IndependentWork16;

public class Document
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";

    public override string ToString() => $"[{Id}] {Title}";
}
