using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.DTOs;
public class CharacterReadDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Bio { get; set; } = string.Empty;
  public int CountyId { get; set; }
  public string CountyName { get; set; } = string.Empty;
  public string Trait { get; set; } = string.Empty;
  public int Level { get; set; }
  public int ExperiencePoints { get; set; }
  public int ClassId { get; set; }
  public string ClassName { get; set; } = string.Empty;
  public int HouseId { get; set; }
  public string HouseName { get; set; } = string.Empty;
  public int AvailableSkillRanks { get; set; }
}

public class CharacterCreateDto
{
  [Required]
  public string Name { get; set; } = string.Empty;
  public string Bio { get; set; } = string.Empty;
  [Required]
  public int CountyId { get; set; }
  public string Trait { get; set; } = string.Empty;
  [Required]
  public int ClassId { get; set; }
  [Required]
  public int HouseId { get; set; }
  public int Level { get; set; } = 0;
  public int ExperiencePoints { get; set; } = 0;
  public int AvailableSkillRanks { get; set; } = 0;
}

public class CharacterUpdateDto
{
  [Required]
  public string Name { get; set; } = string.Empty;
  public string Bio { get; set; } = string.Empty;
  [Required]
  public int CountyId { get; set; }
  public string Trait { get; set; } = string.Empty;
  [Required]
  public int ClassId { get; set; }
  [Required]
  public int HouseId { get; set; }
  public int Level { get; set; } = 0;
  public int ExperiencePoints { get; set; } = 0;
  public int AvailableSkillRanks { get; set; } = 0;
}