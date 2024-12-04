using DiceMiceAPI.Helpers;
using DiceMiceAPI.DTOs;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiceMiceAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ClassSkillLevelCapsController : ControllerBase
{
  private readonly ApplicationDbContext _context;

  public ClassSkillLevelCapsController(ApplicationDbContext context)
  {
    _context = context;
  }

  // GET: api/ClassSkillCaps
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ClassSkillLevelCapReadDto>>> GetClassSkillLevelCaps()
  {
    var caps = await _context.ClassSkillCaps
        .Include(c => c.Class)
        .Include(c => c.Skill)
        .Include(c => c.SkillLevel)
        .ToListAsync();

    return Ok(caps.Select(cap => new ClassSkillLevelCapReadDto
    {
      Id = cap.Id,
      Level = cap.Level,
      ClassId = cap.ClassId,
      ClassName = cap.Class.Name,
      SkillId = cap.SkillId,
      SkillName = cap.Skill.Name,
      SkillLevelId = cap.SkillLevelId,
      SkillLevelName = cap.SkillLevel.Name
    }));
  }

  // GET: api/ClassSkillCaps/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<ClassSkillLevelCapReadDto>> GetClassSkillCap(int id)
  {
    var classSkillCap = await _context.ClassSkillCaps
        .Include(csc => csc.Class)
        .Include(csc => csc.Skill)
        .Include(csc => csc.SkillLevel)
        .FirstOrDefaultAsync(csc => csc.Id == id);

    if (classSkillCap == null)
      return NotFound();

    var classSkillCapDto = new ClassSkillLevelCapReadDto
    {
      Id = classSkillCap.Id,
      Level = classSkillCap.Level,
      ClassId = classSkillCap.ClassId,
      ClassName = classSkillCap.Class.Name,
      SkillId = classSkillCap.SkillId,
      SkillName = classSkillCap.Skill.Name,
      SkillLevelId = classSkillCap.SkillLevelId,
      SkillLevelName = classSkillCap.SkillLevel.Name
    };

    return Ok(classSkillCapDto);
  }

  // POST: api/ClassSkillLevelCaps
  [HttpPost]
  public async Task<ActionResult<ClassSkillLevelCapReadDto>> CreateClassSkillLevelCap(ClassSkillLevelCapCreateDto classSkillCapDto)
  {
    if (!_context.Classes.Any(c => c.Id == classSkillCapDto.ClassId))
      return BadRequest($"ClassId {classSkillCapDto.ClassId} does not exist.");

    if (!_context.Skills.Any(s => s.Id == classSkillCapDto.SkillId))
      return BadRequest($"SkillId {classSkillCapDto.SkillId} does not exist.");

    if (!_context.SkillLevels.Any(sl => sl.Id == classSkillCapDto.SkillLevelId))
      return BadRequest($"SkillLevelId {classSkillCapDto.SkillLevelId} does not exist.");

    var classSkillCap = new ClassSkillCap
    {
      Level = classSkillCapDto.Level,
      ClassId = classSkillCapDto.ClassId,
      SkillId = classSkillCapDto.SkillId,
      SkillLevelId = classSkillCapDto.SkillLevelId
    };
    _context.ClassSkillCaps.Add(classSkillCap);
    await _context.SaveChangesAsync();

    var createdClassSkillCapDto = new ClassSkillLevelCapReadDto
    {
      Id = classSkillCap.Id,
      Level = classSkillCap.Level,
      ClassId = classSkillCap.ClassId,
      ClassName = (await _context.Classes.FindAsync(classSkillCap.ClassId))?.Name ?? string.Empty,
      SkillId = classSkillCap.SkillId,
      SkillName = (await _context.Skills.FindAsync(classSkillCap.SkillId))?.Name ?? string.Empty,
      SkillLevelId = classSkillCap.SkillLevelId,
      SkillLevelName = (await _context.SkillLevels.FindAsync(classSkillCap.SkillLevelId))?.Name ?? string.Empty
    };

    return CreatedAtAction(nameof(GetClassSkillCap), new { id = classSkillCap.Id }, createdClassSkillCapDto);
  }

  // PUT: api/ClassSkillCaps/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateClassSkillCap(int id, ClassSkillLevelCapUpdateDto classSkillCapDto)
  {
    var classSkillCap = await _context.ClassSkillCaps.FindAsync(id);

    if (classSkillCap == null)
      return NotFound();

    if (!_context.Classes.Any(c => c.Id == classSkillCapDto.ClassId))
      return BadRequest($"ClassId {classSkillCapDto.ClassId} does not exist.");

    if (!_context.Skills.Any(s => s.Id == classSkillCapDto.SkillId))
      return BadRequest($"SkillId {classSkillCapDto.SkillId} does not exist.");

    if (!_context.SkillLevels.Any(sl => sl.Id == classSkillCapDto.SkillLevelId))
      return BadRequest($"SkillLevelId {classSkillCapDto.SkillLevelId} does not exist.");

    classSkillCap.Level = classSkillCapDto.Level;
    classSkillCap.ClassId = classSkillCapDto.ClassId;
    classSkillCap.SkillId = classSkillCapDto.SkillId;
    classSkillCap.SkillLevelId = classSkillCapDto.SkillLevelId;

    await _context.SaveChangesAsync();

    return NoContent();
  }

  // DELETE: api/ClassSkillCaps/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteClassSkillCap(int id)
  {
    var classSkillCap = await _context.ClassSkillCaps.FindAsync(id);

    if (classSkillCap == null)
      return NotFound();

    _context.ClassSkillCaps.Remove(classSkillCap);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}