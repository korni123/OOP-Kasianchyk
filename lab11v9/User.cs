namespace lab11v9;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Role> Roles { get; set; }

    public User(int id, string name, List<Role> roles)
    {
        Id = id;
        Name = name;
        Roles = roles;
    }
}
