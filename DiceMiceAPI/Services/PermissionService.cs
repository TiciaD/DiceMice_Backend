using DiceMiceAPI.Models;

public class PermissionService
{
  public static bool HasPermission(User user, Permission requiredPermission)
  {
    return (user.Role.Permissions & requiredPermission) == requiredPermission;
  }
}