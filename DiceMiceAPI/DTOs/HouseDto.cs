using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.DTOs;
public class HouseDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Bio { get; set; } = string.Empty;
  public string Motto { get; set; } = string.Empty;
  public string HeadOfHouse { get; set; } = string.Empty;
  public int GoldAmount { get; set; }
  public string? HouseSeatCountyName { get; set; }
  public string? UserName { get; set; }
}

public class HouseCreateDto
{
  [Required]
  public string Name { get; set; } = string.Empty;
  public string Bio { get; set; } = string.Empty;
  public string Motto { get; set; } = string.Empty;
  public string HeadOfHouse { get; set; } = string.Empty;
  public int GoldAmount { get; set; }
  public int? HouseSeatCountyId { get; set; }
  public int? UserId { get; set; }
}

public class HouseUpdateDto
{
  [Required]
  public int Id { get; set; }
  [Required]
  public string Name { get; set; } = string.Empty;
  public string Bio { get; set; } = string.Empty;
  public string Motto { get; set; } = string.Empty;
  public string HeadOfHouse { get; set; } = string.Empty;
  public int GoldAmount { get; set; }
  public int? HouseSeatCountyId { get; set; }
  public int? UserId { get; set; }
}
