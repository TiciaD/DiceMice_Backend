using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.DTOs;
public class ClassSkillLevelCapReadDto
{
  public int Id { get; set; }
  public int Level { get; set; }
  public int ClassId { get; set; }
  public string ClassName { get; set; } = string.Empty;
  public int SkillId { get; set; }
  public string SkillName { get; set; } = string.Empty;
  public int SkillLevelId { get; set; }
  public string SkillLevelName { get; set; } = string.Empty;
}

public class ClassSkillLevelCapCreateDto
{
  [Required]
  public int Level { get; set; }

  [Required]
  public int ClassId { get; set; }

  [Required]
  public int SkillId { get; set; }

  [Required]
  public int SkillLevelId { get; set; }
}

public class ClassSkillLevelCapUpdateDto
{
  [Required]
  public int Level { get; set; }

  [Required]
  public int ClassId { get; set; }

  [Required]
  public int SkillId { get; set; }

  [Required]
  public int SkillLevelId { get; set; }
}
