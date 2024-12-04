using DiceMiceAPI.Helpers;
using DiceMiceAPI.DTOs;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiceMiceAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ClassSkillsController : ControllerBase
{
  private readonly ApplicationDbContext _context;

  public ClassSkillsController(ApplicationDbContext context)
  {
    _context = context;
  }

  // GET: api/ClassSkills
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ClassSkillReadDto>>> GetClassSkills()
  {
    var classSkills = await _context.ClassSkills
        .Include(cs => cs.Class)
        .Include(cs => cs.Skill)
        .ToListAsync();

    var classSkillDtos = classSkills.Select(cs => new ClassSkillReadDto
    {
      Id = cs.Id,
      ClassId = cs.ClassId,
      ClassName = cs.Class.Name,
      SkillId = cs.SkillId,
      SkillName = cs.Skill.Name
    });

    return Ok(classSkillDtos);
  }

  // GET: api/ClassSkills/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<ClassSkillReadDto>> GetClassSkill(int id)
  {
    var classSkill = await _context.ClassSkills
        .Include(cs => cs.Class)
        .Include(cs => cs.Skill)
        .FirstOrDefaultAsync(cs => cs.Id == id);

    if (classSkill == null)
      return NotFound();

    var classSkillDto = new ClassSkillReadDto
    {
      Id = classSkill.Id,
      ClassId = classSkill.ClassId,
      ClassName = classSkill.Class.Name,
      SkillId = classSkill.SkillId,
      SkillName = classSkill.Skill.Name
    };

    return Ok(classSkillDto);
  }

  // POST: api/ClassSkills
  [HttpPost]
  public async Task<ActionResult<ClassSkillReadDto>> CreateClassSkill(ClassSkillCreateDto classSkillDto)
  {
    if (!_context.Classes.Any(c => c.Id == classSkillDto.ClassId))
      return BadRequest($"ClassId {classSkillDto.ClassId} does not exist.");

    if (!_context.Skills.Any(s => s.Id == classSkillDto.SkillId))
      return BadRequest($"SkillId {classSkillDto.SkillId} does not exist.");

    var classSkill = new ClassSkill
    {
      ClassId = classSkillDto.ClassId,
      SkillId = classSkillDto.SkillId
    };

    _context.ClassSkills.Add(classSkill);
    await _context.SaveChangesAsync();

    var createdClassSkillDto = new ClassSkillReadDto
    {
      Id = classSkill.Id,
      ClassId = classSkill.ClassId,
      ClassName = (await _context.Classes.FindAsync(classSkill.ClassId))?.Name ?? string.Empty,
      SkillId = classSkill.SkillId,
      SkillName = (await _context.Skills.FindAsync(classSkill.SkillId))?.Name ?? string.Empty
    };

    return CreatedAtAction(nameof(GetClassSkill), new { id = classSkill.Id }, createdClassSkillDto);
  }

  // PUT: api/ClassSkills/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateClassSkill(int id, ClassSkillUpdateDto classSkillDto)
  {
    var classSkill = await _context.ClassSkills.FindAsync(id);

    if (classSkill == null)
      return NotFound();

    if (!_context.Classes.Any(c => c.Id == classSkillDto.ClassId))
      return BadRequest($"ClassId {classSkillDto.ClassId} does not exist.");

    if (!_context.Skills.Any(s => s.Id == classSkillDto.SkillId))
      return BadRequest($"SkillId {classSkillDto.SkillId} does not exist.");

    classSkill.ClassId = classSkillDto.ClassId;
    classSkill.SkillId = classSkillDto.SkillId;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/ClassSkills/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteClassSkill(int id)
  {
    var classSkill = await _context.ClassSkills.FindAsync(id);

    if (classSkill == null)
      return NotFound();

    _context.ClassSkills.Remove(classSkill);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}