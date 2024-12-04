using DiceMiceAPI.Helpers;
using DiceMiceAPI.DTOs;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiceMiceAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SkillLevelsController : ControllerBase
{
  private readonly ApplicationDbContext _context;

  public SkillLevelsController(ApplicationDbContext context)
  {
    _context = context;
  }

  // GET: api/SkillLevels
  [HttpGet]
  public async Task<ActionResult<IEnumerable<SkillLevelReadDto>>> GetSkillLevels()
  {
    var skillLevels = await _context.SkillLevels.ToListAsync();

    var skillLevelDtos = skillLevels.Select(skillLevel => new SkillLevelReadDto
    {
      Id = skillLevel.Id,
      Name = skillLevel.Name,
      Value = skillLevel.Value
    });

    return Ok(skillLevelDtos);
  }

  // GET: api/SkillLevels/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<SkillLevelReadDto>> GetSkillLevel(int id)
  {
    var skillLevel = await _context.SkillLevels.FindAsync(id);

    if (skillLevel == null)
      return NotFound();

    var skillLevelDto = new SkillLevelReadDto
    {
      Id = skillLevel.Id,
      Name = skillLevel.Name,
      Value = skillLevel.Value
    };

    return Ok(skillLevelDto);
  }

  // TODO: Not working, some kind of DB issue need to look into
  // POST: api/SkillLevels
  [HttpPost]
  public async Task<ActionResult<SkillLevelReadDto>> CreateSkillLevel(SkillLevelCreateDto skillLevelDto)
  {
    var skillLevel = new SkillLevel
    {
      Name = skillLevelDto.Name,
      Value = skillLevelDto.Value
    };

    _context.SkillLevels.Add(skillLevel);
    await _context.SaveChangesAsync();

    var createdSkillLevelDto = new SkillLevelReadDto
    {
      Id = skillLevel.Id,
      Name = skillLevel.Name,
      Value = skillLevel.Value
    };

    return CreatedAtAction(nameof(GetSkillLevel), new { id = skillLevel.Id }, createdSkillLevelDto);
  }

  // PUT: api/SkillLevels/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateSkillLevel(int id, SkillLevelUpdateDto skillLevelDto)
  {
    var skillLevel = await _context.SkillLevels.FindAsync(id);

    if (skillLevel == null)
      return NotFound();

    skillLevel.Name = skillLevelDto.Name;
    skillLevel.Value = skillLevelDto.Value;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/SkillLevels/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteSkillLevel(int id)
  {
    var skillLevel = await _context.SkillLevels.FindAsync(id);

    if (skillLevel == null)
      return NotFound();

    _context.SkillLevels.Remove(skillLevel);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}