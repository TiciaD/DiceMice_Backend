using DiceMiceAPI.Helpers;
using DiceMiceAPI.DTOs;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiceMiceAPI.Controllers;
[Authorize(AuthenticationSchemes = "Discord")]
[Route("api/[controller]")]
[ApiController]
public class CharactersController : ControllerBase
{

  private readonly ApplicationDbContext _context;

  public CharactersController(ApplicationDbContext context)
  {
    _context = context;
  }

  // GET: api/Characters
  [HttpGet]
  public async Task<ActionResult<IEnumerable<CharacterReadDto>>> GetCharacters()
  {
    var characters = await _context.Characters
        .Include(c => c.OriginCounty)
        .Include(c => c.Class)
        .Include(c => c.House)
        .ToListAsync();

    var characterDtos = characters.Select(c => new CharacterReadDto
    {
      Id = c.Id,
      Name = c.Name,
      Bio = c.Bio,
      CountyId = c.CountyId,
      CountyName = c.OriginCounty.Name,
      Trait = c.Trait,
      Level = c.Level,
      ExperiencePoints = c.ExperiencePoints,
      ClassId = c.ClassId,
      ClassName = c.Class.Name,
      HouseId = c.HouseId,
      HouseName = c.House.Name,
      AvailableSkillRanks = c.AvailableSkillRanks
    });

    return Ok(characterDtos);
  }

  // GET: api/Characters/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<CharacterReadDto>> GetCharacter(int id)
  {
    var character = await _context.Characters
        .Include(c => c.OriginCounty)
        .Include(c => c.Class)
        .Include(c => c.House)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (character == null)
      return NotFound();

    var characterDto = new CharacterReadDto
    {
      Id = character.Id,
      Name = character.Name,
      Bio = character.Bio,
      CountyId = character.CountyId,
      CountyName = character.OriginCounty.Name,
      Trait = character.Trait,
      Level = character.Level,
      ExperiencePoints = character.ExperiencePoints,
      ClassId = character.ClassId,
      ClassName = character.Class.Name,
      HouseId = character.HouseId,
      HouseName = character.House.Name,
      AvailableSkillRanks = character.AvailableSkillRanks
    };

    return Ok(characterDto);
  }

  // POST: api/Characters
  [HttpPost]
  public async Task<ActionResult<CharacterReadDto>> CreateCharacter(CharacterCreateDto characterDto)
  {
    if (!_context.Counties.Any(c => c.Id == characterDto.CountyId))
      return BadRequest($"CountyId {characterDto.CountyId} does not exist.");

    if (!_context.Classes.Any(c => c.Id == characterDto.ClassId))
      return BadRequest($"ClassId {characterDto.ClassId} does not exist.");

    if (!_context.Houses.Any(h => h.Id == characterDto.HouseId))
      return BadRequest($"HouseId {characterDto.HouseId} does not exist.");

    var character = new Character
    {
      Name = characterDto.Name,
      Bio = characterDto.Bio,
      CountyId = characterDto.CountyId,
      Trait = characterDto.Trait,
      ClassId = characterDto.ClassId,
      HouseId = characterDto.HouseId,
      Level = characterDto.Level,
      ExperiencePoints = characterDto.ExperiencePoints,
      AvailableSkillRanks = characterDto.AvailableSkillRanks
    };

    _context.Characters.Add(character);
    await _context.SaveChangesAsync();

    var createdCharacterDto = new CharacterReadDto
    {
      Id = character.Id,
      Name = character.Name,
      Bio = character.Bio,
      CountyId = character.CountyId,
      CountyName = (await _context.Counties.FindAsync(character.CountyId))?.Name ?? string.Empty,
      Trait = character.Trait,
      Level = character.Level,
      ExperiencePoints = character.ExperiencePoints,
      ClassId = character.ClassId,
      ClassName = (await _context.Classes.FindAsync(character.ClassId))?.Name ?? string.Empty,
      HouseId = character.HouseId,
      HouseName = (await _context.Houses.FindAsync(character.HouseId))?.Name ?? string.Empty,
      AvailableSkillRanks = character.AvailableSkillRanks
    };

    return CreatedAtAction(nameof(GetCharacter), new { id = character.Id }, createdCharacterDto);
  }

  // PUT: api/Characters/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateCharacter(int id, CharacterUpdateDto characterDto)
  {
    var character = await _context.Characters.FindAsync(id);

    if (character == null)
      return NotFound();

    if (!_context.Counties.Any(c => c.Id == characterDto.CountyId))
      return BadRequest($"CountyId {characterDto.CountyId} does not exist.");

    if (!_context.Classes.Any(c => c.Id == characterDto.ClassId))
      return BadRequest($"ClassId {characterDto.ClassId} does not exist.");

    if (!_context.Houses.Any(h => h.Id == characterDto.HouseId))
      return BadRequest($"HouseId {characterDto.HouseId} does not exist.");

    character.Name = characterDto.Name;
    character.Bio = characterDto.Bio;
    character.CountyId = characterDto.CountyId;
    character.Trait = characterDto.Trait;
    character.ClassId = characterDto.ClassId;
    character.HouseId = characterDto.HouseId;
    character.Level = characterDto.Level;
    character.ExperiencePoints = characterDto.ExperiencePoints;
    character.AvailableSkillRanks = characterDto.AvailableSkillRanks;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/Characters/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteCharacter(int id)
  {
    var character = await _context.Characters.FindAsync(id);

    if (character == null)
      return NotFound();

    _context.Characters.Remove(character);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  // [HttpGet("GetCharacter")]
  // public IActionResult GetCharacter()
  // {
  //   var currentUser = HttpContext.Items["CurrentUser"] as User;
  //   if (currentUser == null)
  //   {
  //     return Unauthorized();
  //   }

  //   return Ok("Authorized.");
  // }
}