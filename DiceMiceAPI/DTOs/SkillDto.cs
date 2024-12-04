using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.DTOs;
public class SkillReadDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public int AssociatedStatId { get; set; }
  public string AssociatedStatName { get; set; } = string.Empty;
}

public class SkillCreateDto
{
  [Required]
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  [Required]
  public int AssociatedStatId { get; set; }
}

public class SkillUpdateDto
{
  [Required]
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  [Required]
  public int AssociatedStatId { get; set; }
}