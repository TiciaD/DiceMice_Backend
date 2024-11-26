using DiceMiceAPI.Data;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiceMiceAPI.Controllers;
[Authorize(AuthenticationSchemes = "Discord")]
[Route("api/[controller]")]
[ApiController]
public class CharacterController : ControllerBase
{

  private readonly ApplicationDbContext _context;

  public CharacterController(ApplicationDbContext context)
  {
    _context = context;
  }

  [HttpGet("GetCharacter")]
  public IActionResult GetCharacter()
  {
    var currentUser = HttpContext.Items["CurrentUser"] as User;
    if (currentUser == null)
    {
      return Unauthorized();
    }

    return Ok("Authorized.");
  }
}