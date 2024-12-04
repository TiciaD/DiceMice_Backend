using DiceMiceAPI.Helpers;
using DiceMiceAPI.DTOs;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiceMiceAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CountiesController(ApplicationDbContext context) : ControllerBase
{
  private readonly ApplicationDbContext _context = context;

  // GET: api/Counties
  [HttpGet]
  public async Task<ActionResult<IEnumerable<CountyDto>>> GetCounties()
  {
    var counties = await _context.Counties
        .Include(c => c.AssociatedStat)
        .Include(c => c.Houses)
        .Select(c => new CountyDto
        {
          Id = c.Id,
          Name = c.Name,
          Bio = c.Bio,
          AssociatedStatName = c.AssociatedStat != null ? c.AssociatedStat.Name : "",
          HouseNames = c.Houses.Select(h => h.Name).ToList()
        })
        .ToListAsync();

    return Ok(counties);
  }

  // GET: api/Counties/5
  [HttpGet("{id}")]
  public async Task<ActionResult<CountyDto>> GetCounty(int id)
  {
    var county = await _context.Counties
        .Include(c => c.AssociatedStat)
        .Include(c => c.Houses)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (county == null)
    {
      return NotFound();
    }

    var countyDto = new CountyDto
    {
      Id = county.Id,
      Name = county.Name,
      Bio = county.Bio,
      AssociatedStatName = county.AssociatedStat?.Name ?? "",
      HouseNames = county.Houses.Select(h => h.Name).ToList()
    };

    return Ok(countyDto);
  }

  // POST: api/Counties
  [HttpPost]
  public async Task<ActionResult<CountyDto>> CreateCounty(CountyCreateDto countyDto)
  {
    var county = new County
    {
      Name = countyDto.Name,
      Bio = countyDto.Bio,
      AssociatedStatId = countyDto.AssociatedStatId
    };

    _context.Counties.Add(county);
    await _context.SaveChangesAsync();

    var createdCountyDto = new CountyDto
    {
      Id = county.Id,
      Name = county.Name,
      Bio = county.Bio,
      AssociatedStatName = (await _context.Stats.FindAsync(county.AssociatedStatId))?.Name ?? ""
    };

    return CreatedAtAction(nameof(GetCounty), new { id = county.Id }, createdCountyDto);
  }

  // PUT: api/Counties/5
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateCounty(int id, CountyUpdateDto countyDto)
  {
    if (id != countyDto.Id)
    {
      return BadRequest();
    }

    var county = await _context.Counties.FindAsync(id);
    if (county == null)
    {
      return NotFound();
    }

    county.Name = countyDto.Name;
    county.Bio = countyDto.Bio;
    county.AssociatedStatId = countyDto.AssociatedStatId;

    _context.Entry(county).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!_context.Counties.Any(c => c.Id == id))
      {
        return NotFound();
      }
      throw;
    }

    return NoContent();
  }

  // DELETE: api/Counties/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteCounty(int id)
  {
    var county = await _context.Counties.FindAsync(id);
    if (county == null)
    {
      return NotFound();
    }

    _context.Counties.Remove(county);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}