using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models;
public class Character
{
  [Key]
  public int Id { get; set; }

  [Required]
  public string Name { get; set; } = string.Empty;

  public string Bio { get; set; } = string.Empty;

  public int CountyId { get; set; } // FK to County
  public County OriginCounty { get; set; } = null!;
  public string Trait { get; set; } = string.Empty;
  public int Level { get; set; } = 0;
  public int ExperiencePoints { get; set; } = 0;
  public int ClassId { get; set; } // FK to Class
  public Class Class { get; set; } = null!;

  public int HouseId { get; set; } // FK to House
  public House House { get; set; } = null!;
  public int AvailableSkillRanks { get; set; } = 0;
}