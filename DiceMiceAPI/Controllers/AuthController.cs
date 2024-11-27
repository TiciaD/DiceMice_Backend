using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DiceMiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
  private readonly IConfiguration _configuration;

  public AuthController(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  [HttpGet("redirect")]
  public IActionResult Redirect()
  {
    return Redirect("https://dicemice-frontend.onrender.com/");
  }

  [HttpGet("login")]
  public IActionResult Login()
  {
    var redirectUrl = new PathString("/auth/redirect");
    var properties = new AuthenticationProperties
    {
      RedirectUri = redirectUrl
    };
    return Challenge(properties, "Discord");
  }

  [HttpGet("AccessDenied")]
  public IActionResult AccessDenied()
  {
    Console.WriteLine("Access Denied.");
    return Unauthorized("Authentication Failed.");
  }

  [HttpGet("status")]
  public IActionResult Status()
  {
    if (User.Identity?.IsAuthenticated == true)
    {
      var discordId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var username = User.FindFirst(ClaimTypes.Name)?.Value;

      return Ok(new
      {
        IsAuthenticated = true,
        DiscordId = discordId,
        Username = username,
      });
    }

    return Unauthorized(new { IsAuthenticated = false });
  }

}