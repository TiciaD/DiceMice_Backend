using DiceMiceAPI.Helpers;
using DiceMiceAPI.DTOs;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiceMiceAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SkillsController : ControllerBase
{
  private readonly ApplicationDbContext _context;

  public SkillsController(ApplicationDbContext context)
  {
    _context = context;
  }

  // GET: api/Skills
  [HttpGet]
  public async Task<ActionResult<IEnumerable<SkillReadDto>>> GetSkills()
  {
    var skills = await _context.Skills
        .Include(s => s.AssociatedStat)
        .ToListAsync();

    var skillDtos = skills.Select(skill => new SkillReadDto
    {
      Id = skill.Id,
      Name = skill.Name,
      Description = skill.Description,
      AssociatedStatId = skill.AssociatedStatId,
      AssociatedStatName = skill.AssociatedStat.Name
    });

    return Ok(skillDtos);
  }

  // GET: api/Skills/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<SkillReadDto>> GetSkill(int id)
  {
    var skill = await _context.Skills
        .Include(s => s.AssociatedStat)
        .FirstOrDefaultAsync(s => s.Id == id);

    if (skill == null)
      return NotFound();

    var skillDto = new SkillReadDto
    {
      Id = skill.Id,
      Name = skill.Name,
      Description = skill.Description,
      AssociatedStatId = skill.AssociatedStatId,
      AssociatedStatName = skill.AssociatedStat.Name
    };

    return Ok(skillDto);
  }

  // POST: api/Skills
  [HttpPost]
  public async Task<ActionResult<SkillReadDto>> CreateSkill(SkillCreateDto skillDto)
  {
    if (!_context.Stats.Any(stat => stat.Id == skillDto.AssociatedStatId))
      return BadRequest($"AssociatedStatId {skillDto.AssociatedStatId} does not exist.");

    var skill = new Skill
    {
      Name = skillDto.Name,
      Description = skillDto.Description,
      AssociatedStatId = skillDto.AssociatedStatId
    };

    _context.Skills.Add(skill);
    await _context.SaveChangesAsync();

    var createdSkillDto = new SkillReadDto
    {
      Id = skill.Id,
      Name = skill.Name,
      Description = skill.Description,
      AssociatedStatId = skill.AssociatedStatId,
      AssociatedStatName = (await _context.Stats.FindAsync(skill.AssociatedStatId))?.Name ?? string.Empty
    };

    return CreatedAtAction(nameof(GetSkill), new { id = skill.Id }, createdSkillDto);
  }

  // PUT: api/Skills/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateSkill(int id, SkillUpdateDto skillDto)
  {
    var skill = await _context.Skills.FindAsync(id);

    if (skill == null)
      return NotFound();

    if (!_context.Stats.Any(stat => stat.Id == skillDto.AssociatedStatId))
      return BadRequest($"AssociatedStatId {skillDto.AssociatedStatId} does not exist.");

    skill.Name = skillDto.Name;
    skill.Description = skillDto.Description;
    skill.AssociatedStatId = skillDto.AssociatedStatId;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/Skills/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteSkill(int id)
  {
    var skill = await _context.Skills.FindAsync(id);

    if (skill == null)
      return NotFound();

    _context.Skills.Remove(skill);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}