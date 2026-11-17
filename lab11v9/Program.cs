using lab11v9;

Permission read = new Permission(1, "Read");
Permission write = new Permission(2, "Write");
Permission delete = new Permission(3, "Delete");
Permission publish = new Permission(4, "Publish");
Permission manageUsers = new Permission(5, "ManageUsers");
Permission viewReports = new Permission(6, "ViewReports");

Role guest = new Role(1, "Guest", new List<Permission> { read });
Role editor = new Role(2, "Editor", new List<Permission> { read, write, publish });
Role moderator = new Role(3, "Moderator", new List<Permission> { read, delete });
Role analyst = new Role(4, "Analyst", new List<Permission> { read, viewReports });
Role admin = new Role(5, "Admin", new List<Permission> { read, write, delete, manageUsers });

List<User> users = new List<User>
{
    new User(1, "Anna", new List<Role> { admin }),
    new User(2, "Bohdan", new List<Role> { editor }),
    new User(3, "Vira", new List<Role> { guest }),
    new User(4, "Hryhorii", new List<Role> { moderator, analyst }),
    new User(5, "Daryna", new List<Role> { editor, analyst }),
    new User(6, "Yevhen", new List<Role> { guest }),
    new User(7, "Zoriana", new List<Role> { moderator }),
    new User(8, "Illia", new List<Role> { analyst }),
    new User(9, "Kateryna", new List<Role> { editor, moderator }),
    new User(10, "Leonid", new List<Role> { guest, analyst }),
    new User(11, "Marta", new List<Role> { admin, editor }),
    new User(12, "Nazar", new List<Role> { guest })
};

Console.WriteLine($"Users: {users.Count}");
Console.WriteLine();

Console.WriteLine("=== SelectMany: all permissions of all roles of all users ===");
List<Permission> allPermissions = users
    .SelectMany(u => u.Roles)
    .SelectMany(r => r.Permissions)
    .ToList();
Console.WriteLine($"Total permissions in one list: {allPermissions.Count}");
Console.WriteLine(string.Join(", ", allPermissions.Select(p => p.Name)));
Console.WriteLine();

Console.WriteLine("Unique permissions:");
Console.WriteLine(string.Join(", ", allPermissions.Select(p => p.Name).Distinct()));
Console.WriteLine();

Console.WriteLine("=== Pagination: 5 users per page ===");
int pageSize = 5;
int totalPages = (int)Math.Ceiling(users.Count / (double)pageSize);
for (int page = 1; page <= totalPages; page++)
{
    List<User> pageUsers = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    Console.WriteLine($"Page {page} of {totalPages}:");
    foreach (User user in pageUsers)
    {
        Console.WriteLine($"  {user.Id}. {user.Name} ({string.Join(", ", user.Roles.Select(r => r.Name))})");
    }
}
Console.WriteLine();

Console.WriteLine("=== Extension method: HasPermission ===");
string[] permissionsToCheck = { "Delete", "Publish", "ManageUsers" };
foreach (string permission in permissionsToCheck)
{
    List<string> names = users.Where(u => u.HasPermission(permission)).Select(u => u.Name).ToList();
    Console.WriteLine($"{permission}: {string.Join(", ", names)}");
}
Console.WriteLine();
Console.WriteLine("Does Vira have Write? " + users[2].HasPermission("Write"));
Console.WriteLine("Does Daryna have Publish? " + users[4].HasPermission("Publish"));
