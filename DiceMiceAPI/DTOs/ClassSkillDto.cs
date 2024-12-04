using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.DTOs;
public class ClassSkillReadDto
{
  public int Id { get; set; }
  public int ClassId { get; set; }
  public string ClassName { get; set; } = string.Empty;
  public int SkillId { get; set; }
  public string SkillName { get; set; } = string.Empty;
}

public class ClassSkillCreateDto
{
  [Required]
  public int ClassId { get; set; }
  [Required]
  public int SkillId { get; set; }
}

public class ClassSkillUpdateDto
{
  [Required]
  public int ClassId { get; set; }
  [Required]
  public int SkillId { get; set; }
}