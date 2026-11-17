namespace lab11v9;

public static class UserExtensions
{
    public static bool HasPermission(this User user, string permissionName)
    {
        return user.Roles.Any(r => r.Permissions.Any(p => p.Name == permissionName));
    }
}
