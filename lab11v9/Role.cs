namespace lab11v9;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Permission> Permissions { get; set; }

    public Role(int id, string name, List<Permission> permissions)
    {
        Id = id;
        Name = name;
        Permissions = permissions;
    }
}
