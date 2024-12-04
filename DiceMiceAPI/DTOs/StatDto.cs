using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.DTOs;
public class StatDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public bool IsRollBased { get; set; } = false;
  public string DieRoll { get; set; } = string.Empty;
  public List<string> CountyNames { get; set; } = [];
}

public class StatCreateDto
{
  [Required]
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public bool IsRollBased { get; set; } = false;
  public string DieRoll { get; set; } = string.Empty;
}

public class StatUpdateDto
{
  [Required]
  public int Id { get; set; }
  [Required]
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public bool IsRollBased { get; set; } = false;
  public string DieRoll { get; set; } = string.Empty;
}
