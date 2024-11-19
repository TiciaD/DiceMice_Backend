using Microsoft.AspNetCore.Mvc;

namespace DiceMiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
  [HttpGet("Redirect")]
  public IActionResult Redirect()
  {
    Console.WriteLine("Redirected.");
    return Ok("Redirected.");
  }

  [HttpGet("AccessDenied")]
  public IActionResult AccessDenied()
  {
    Console.WriteLine("Access Denied.");
    return Unauthorized("Authentication Failed.");
  }

  [HttpGet("Status")]
  public IActionResult Status()
  {
    Console.WriteLine("Status Check");
    return Ok("Successful Status Check.");
  }
}