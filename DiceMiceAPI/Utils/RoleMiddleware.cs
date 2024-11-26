using System.Security.Claims;
using DiceMiceAPI.Data;
using Microsoft.EntityFrameworkCore;

public class RoleMiddleware
{
  private readonly RequestDelegate _next;

  public RoleMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
  {
    var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userIdClaim))
    {
      Console.WriteLine("No user ID claim found.");
    }
    else
    {
      // Fetch the user from the database
      var user = await dbContext.Users
          .Include(u => u.Role)
          .FirstOrDefaultAsync(u => u.DiscordId == userIdClaim);

      if (user == null)
      {
        Console.WriteLine($"User with DiscordId {userIdClaim} not found in database.");
      }
      else
      {
        Console.WriteLine($"User found: {user.Email}");
        // Store the user in HttpContext.Items
        context.Items["CurrentUser"] = user;
      }
    }

    await _next(context);
  }
}
