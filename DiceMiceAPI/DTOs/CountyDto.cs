using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.DTOs;
public class CountyDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Bio { get; set; } = string.Empty;
  public string AssociatedStatName { get; set; } = string.Empty;
  public List<string> HouseNames { get; set; } = [];
}

public class CountyCreateDto
{
  [Required]
  public string Name { get; set; } = string.Empty;
  public string Bio { get; set; } = string.Empty;
  [Required]
  public int AssociatedStatId { get; set; }
}

public class CountyUpdateDto
{
  [Required]
  public int Id { get; set; }
  [Required]
  public string Name { get; set; } = string.Empty;
  public string Bio { get; set; } = string.Empty;
  [Required]
  public int AssociatedStatId { get; set; }
}