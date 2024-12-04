using DiceMiceAPI.Helpers;
using DiceMiceAPI.DTOs;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiceMiceAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClassesController(ApplicationDbContext context) : ControllerBase
{
  private readonly ApplicationDbContext _context = context;

  // GET: api/classes
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ClassReadDto>>> GetClasses()
  {
    var classes = await _context.Classes
        .Select(s => new ClassReadDto
        {
          Id = s.Id,
          Name = s.Name,
          Description = s.Description,
        })
        .ToListAsync();

    return Ok(classes);
  }

  // GET: api/classes/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<ClassReadDto>> GetClass(int id)
  {
    var foundClass = await _context.Classes
    .FirstOrDefaultAsync(c => c.Id == id);

    if (foundClass == null)
    {
      return NotFound();
    }

    var classDto = new ClassReadDto
    {
      Id = foundClass.Id,
      Name = foundClass.Name,
      Description = foundClass.Description,
    };

    return Ok(foundClass);
  }

  // POST: api/classes
  [HttpPost]
  public async Task<ActionResult<ClassReadDto>> CreateClass(ClassCreateDto classDto)
  {
    var newClass = new Class
    {
      Name = classDto.Name,
      Description = classDto.Description,
    };

    _context.Classes.Add(newClass);
    await _context.SaveChangesAsync();

    var createdClassDto = new ClassCreateDto
    {
      Name = newClass.Name,
      Description = newClass.Description,
    };

    return CreatedAtAction(nameof(GetClass), new { id = newClass.Id }, createdClassDto);
  }

  // PUT: api/classes/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateClass(int id, ClassUpdateDto classDto)
  {
    if (id != classDto.Id)
    {
      return BadRequest();
    }

    var foundClass = await _context.Classes.FindAsync(id);
    if (foundClass == null)
    {
      return NotFound();
    }

    foundClass.Name = classDto.Name;
    foundClass.Description = classDto.Description;

    _context.Entry(foundClass).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!_context.Classes.Any(c => c.Id == id))
      {
        return NotFound();
      }
      throw;
    }

    return NoContent();
  }

  // DELETE: api/classes/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteClass(int id)
  {
    var @class = await _context.Classes.FindAsync(id);
    if (@class == null)
    {
      return NotFound();
    }

    _context.Classes.Remove(@class);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}
