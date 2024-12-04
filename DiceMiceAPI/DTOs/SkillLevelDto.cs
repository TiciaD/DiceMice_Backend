using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.DTOs;
public class SkillLevelReadDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public int Value { get; set; }
}

public class SkillLevelCreateDto
{
  [Required]
  public string Name { get; set; } = string.Empty;

  [Required]
  public int Value { get; set; }
}

public class SkillLevelUpdateDto
{
  [Required]
  public string Name { get; set; } = string.Empty;

  [Required]
  public int Value { get; set; }
}