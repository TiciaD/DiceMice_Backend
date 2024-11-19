using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiceMiceAPI.Controllers;
[Authorize(AuthenticationSchemes = "Discord")]
[Route("api/[controller]")]
[ApiController]
public class CharacterController : ControllerBase
{
  [HttpGet("GetCharacter")]
  public IActionResult GetCharacter()
  {
    return Ok("Authorized.");
  }
}