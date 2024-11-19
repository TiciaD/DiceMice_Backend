
using Microsoft.AspNetCore.Mvc;

namespace DiceMiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
  public IActionResult AccessDenied()
  {
    return Unauthorized("Authentication Failed.");
  }
}