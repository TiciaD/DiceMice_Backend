using System.Security.Claims;
using DiceMiceAPI.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiceMiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
  [HttpGet("redirect")]
  [Authorize(AuthenticationSchemes = "Discord")]
  public IActionResult Redirect()
  {
    Console.WriteLine("Redirected!");
    // var query = HttpContext.Request.Query.ToString();
    // Console.WriteLine("Incoming Query: " + query); // Log query params
    // return Ok("Redirected.");
    if (User.Identity?.IsAuthenticated ?? false)
    {
      // Authentication was successful, redirect to frontend
      Console.WriteLine($"User authenticated: {User.Identity.Name}");
      return Redirect("https://dicemice-frontend.onrender.com/");
    }
    else
    {
      // Authentication failed, redirect to frontend with error
      Console.WriteLine("Authentication failed.");
      return Redirect("https://dicemice-frontend.onrender.com/login?error=auth_failed");
    }
  }

  [HttpGet("login")]
  public IActionResult Login()
  {
    Console.WriteLine("****************User Identity IsAuthenticated {0}", User.Identity?.IsAuthenticated);
    if (User.Identity?.IsAuthenticated ?? false)
    {
      Console.WriteLine("User already authenticated, redirecting to frontend...");
      return Redirect("https://dicemice-frontend.onrender.com/");
    }
    else
    {
      Console.WriteLine("User not authenticated, initiating Discord OAuth...");
      var redirectUrl = "https://dicemice-frontend.onrender.com";
      var properties = new AuthenticationProperties
      {
        RedirectUri = redirectUrl
      };
      Console.WriteLine("Challenge Issued!");
      return Challenge(properties, "Discord");
    }

  }

  [HttpGet("AccessDenied")]
  public IActionResult AccessDenied()
  {
    Console.WriteLine("Access Denied.");
    return Unauthorized("Authentication Failed.");
  }

  [Authorize(AuthenticationSchemes = "Discord")]
  [HttpGet("status")]
  public async Task<IActionResult> Status()
  {
    if (User.Identity?.IsAuthenticated == true)
    {
      var discordId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var username = User.FindFirst(ClaimTypes.Name)?.Value;

      // Retrieve user info from the database
      var dbContext = HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
      var user = await dbContext.Users.FirstOrDefaultAsync(u => u.DiscordId == discordId);

      if (user == null)
      {
        return Unauthorized(new { IsAuthenticated = false, Message = "User not found in the database." });
      }

      return Ok(new
      {
        IsAuthenticated = true,
        DiscordId = discordId,
        Username = username,
        Email = user.Email,
        Avatar = user.Avatar,
        Role = user.Role.RoleName
      });
    }

    return Unauthorized(new { IsAuthenticated = false });
  }

}