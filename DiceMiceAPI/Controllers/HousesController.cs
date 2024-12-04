using DiceMiceAPI.Helpers;
using DiceMiceAPI.DTOs;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiceMiceAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class HousesController(ApplicationDbContext context) : ControllerBase
{
  private readonly ApplicationDbContext _context = context;

  // GET: api/Houses
  [HttpGet]
  public async Task<ActionResult<IEnumerable<HouseDto>>> GetHouses()
  {
    var houses = await _context.Houses
        .Include(h => h.HouseSeatCounty)
        .Include(h => h.User)
        .Select(h => new HouseDto
        {
          Id = h.Id,
          Name = h.Name,
          Bio = h.Bio,
          Motto = h.Motto,
          HeadOfHouse = h.HeadOfHouse,
          GoldAmount = h.GoldAmount,
          HouseSeatCountyName = h.HouseSeatCounty != null ? h.HouseSeatCounty.Name : "",
          UserName = h.User != null ? h.User.Username : ""
        })
        .ToListAsync();

    return Ok(houses);
  }

  // GET: api/Houses/5
  [HttpGet("{id}")]
  public async Task<ActionResult<HouseDto>> GetHouse(int id)
  {
    var house = await _context.Houses
        .Include(h => h.HouseSeatCounty)
        .Include(h => h.User)
        .FirstOrDefaultAsync(h => h.Id == id);

    if (house == null)
    {
      return NotFound();
    }

    var houseDto = new HouseDto
    {
      Id = house.Id,
      Name = house.Name,
      Bio = house.Bio,
      Motto = house.Motto,
      HeadOfHouse = house.HeadOfHouse,
      GoldAmount = house.GoldAmount,
      HouseSeatCountyName = house.HouseSeatCounty?.Name,
      UserName = house.User != null ? house.User.Username : ""
    };

    return Ok(houseDto);
  }

  // POST: api/Houses
  [HttpPost]
  public async Task<ActionResult<HouseDto>> CreateHouse(HouseCreateDto houseDto)
  {
    var house = new House
    {
      Name = houseDto.Name,
      Bio = houseDto.Bio,
      Motto = houseDto.Motto,
      HeadOfHouse = houseDto.HeadOfHouse,
      GoldAmount = houseDto.GoldAmount,
      HouseSeatCountyId = houseDto.HouseSeatCountyId,
      UserId = houseDto.UserId
    };

    _context.Houses.Add(house);
    await _context.SaveChangesAsync();

    var createdHouseDto = new HouseDto
    {
      Id = house.Id,
      Name = house.Name,
      Bio = house.Bio,
      Motto = house.Motto,
      HeadOfHouse = house.HeadOfHouse,
      GoldAmount = house.GoldAmount,
      HouseSeatCountyName = (await _context.Counties.FindAsync(house.HouseSeatCountyId))?.Name,
      UserName = (await _context.Users.FindAsync(house.UserId))?.Username
    };

    return CreatedAtAction(nameof(GetHouse), new { id = house.Id }, createdHouseDto);
  }

  // PUT: api/Houses/5
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateHouse(int id, HouseUpdateDto houseDto)
  {
    if (id != houseDto.Id)
    {
      return BadRequest();
    }

    var house = await _context.Houses.FindAsync(id);
    if (house == null)
    {
      return NotFound();
    }

    house.Name = houseDto.Name;
    house.Bio = houseDto.Bio;
    house.Motto = houseDto.Motto;
    house.HeadOfHouse = houseDto.HeadOfHouse;
    house.GoldAmount = houseDto.GoldAmount;
    house.HouseSeatCountyId = houseDto.HouseSeatCountyId;
    house.UserId = houseDto.UserId;

    _context.Entry(house).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!_context.Houses.Any(h => h.Id == id))
      {
        return NotFound();
      }
      throw;
    }

    return NoContent();
  }

  // DELETE: api/Houses/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteHouse(int id)
  {
    var house = await _context.Houses.FindAsync(id);
    if (house == null)
    {
      return NotFound();
    }

    _context.Houses.Remove(house);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}
