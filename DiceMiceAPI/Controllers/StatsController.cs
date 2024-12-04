using DiceMiceAPI.Helpers;
using DiceMiceAPI.DTOs;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiceMiceAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class StatsController(ApplicationDbContext context) : ControllerBase
{
  private readonly ApplicationDbContext _context = context;

  // GET: api/Stats
  [HttpGet]
  public async Task<ActionResult<IEnumerable<StatDto>>> GetStats()
  {
    var stats = await _context.Stats
        .Select(s => new StatDto
        {
          Id = s.Id,
          Name = s.Name,
          Description = s.Description,
          IsRollBased = s.IsRollBased,
        })
        .ToListAsync();

    return Ok(stats);
  }

  // GET: api/Stats/5
  [HttpGet("{id}")]
  public async Task<ActionResult<StatDto>> GetStat(int id)
  {
    var stat = await _context.Stats
        .FirstOrDefaultAsync(s => s.Id == id);

    if (stat == null)
    {
      return NotFound();
    }

    var statDto = new StatDto
    {
      Id = stat.Id,
      Name = stat.Name,
      Description = stat.Description,
      IsRollBased = stat.IsRollBased,
    };

    return Ok(statDto);
  }

  // POST: api/Stats
  [HttpPost]
  public async Task<ActionResult<StatDto>> CreateStat(StatCreateDto statDto)
  {
    var stat = new Stat
    {
      Name = statDto.Name,
      Description = statDto.Description,
      IsRollBased = statDto.IsRollBased,
    };

    _context.Stats.Add(stat);
    await _context.SaveChangesAsync();

    var createdStatDto = new StatDto
    {
      Id = stat.Id,
      Name = stat.Name,
      Description = stat.Description,
      IsRollBased = stat.IsRollBased,
    };

    return CreatedAtAction(nameof(GetStat), new { id = stat.Id }, createdStatDto);
  }

  // PUT: api/Stats/5
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateStat(int id, StatUpdateDto statDto)
  {
    if (id != statDto.Id)
    {
      return BadRequest();
    }

    var stat = await _context.Stats.FindAsync(id);
    if (stat == null)
    {
      return NotFound();
    }

    stat.Name = statDto.Name;
    stat.Description = statDto.Description;
    stat.IsRollBased = statDto.IsRollBased;

    _context.Entry(stat).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!_context.Stats.Any(s => s.Id == id))
      {
        return NotFound();
      }
      throw;
    }

    return NoContent();
  }

  // DELETE: api/Stats/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteStat(int id)
  {
    var stat = await _context.Stats.FindAsync(id);
    if (stat == null)
    {
      return NotFound();
    }

    _context.Stats.Remove(stat);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}
