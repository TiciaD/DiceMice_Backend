using System.Security.Claims;
using DiceMiceAPI.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace DiceMiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
  [HttpGet("redirect")]
  [Authorize(AuthenticationSchemes = "Discord")]
  public async Task<IActionResult> Redirect()
  {
    Console.WriteLine("Redirected!");
    // var query = HttpContext.Request.Query.ToString();
    // Console.WriteLine("Incoming Query: " + query); // Log query params
    // return Ok("Redirected.");
    if (User.Identity?.IsAuthenticated ?? false)
    {
      var discordId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
      var username = User.FindFirst(ClaimTypes.Name)?.Value ?? "";

      // Fetch user info from the database (optional)
      var dbContext = HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
      var user = await dbContext.Users.FirstOrDefaultAsync(u => u.DiscordId == discordId);

      // Create a query string with user info to send to the frontend
      var frontendUrl = "https://dicemice-frontend.onrender.com/";
      var queryString = new Dictionary<string, string>
        {
            { "discordId", discordId },
            { "username", username },
            { "email", user?.Email ?? "" },  // Example of additional info
            { "avatar", user?.Avatar ?? "" }
        };

      // Build the redirect URL
      var redirectUrl = QueryHelpers.AddQueryString(frontendUrl, queryString);

      // Redirect to the frontend with the user data in the query parameters
      return Redirect(redirectUrl);
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
      Console.WriteLine("User already authenticated, returning user info...");
      var discordId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var username = User.FindFirst(ClaimTypes.Name)?.Value;

      // Fetch the user info from the database if needed
      var dbContext = HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
      var user = dbContext.Users.FirstOrDefault(u => u.DiscordId == discordId);

      return Ok(new
      {
        IsAuthenticated = true,
        DiscordId = discordId,
        Username = username,
        Email = user.Email,
        Avatar = user.Avatar,
        Role = user.Role.RoleName // Or any other data you need to return
      });
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

      // Response.Headers.Add("Access-Control-Allow-Origin", "https://dicemice-frontend.onrender.com");
      // Response.Headers.Add("Access-Control-Allow-Credentials", "true");

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

    // Response.Headers.Add("Access-Control-Allow-Origin", "https://dicemice-frontend.onrender.com");
    // Response.Headers.Add("Access-Control-Allow-Credentials", "true");

    return Unauthorized(new { IsAuthenticated = false });
  }

}
