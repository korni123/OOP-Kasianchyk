namespace lab11v9;

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
