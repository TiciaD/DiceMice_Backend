using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models;
public class ClassSkillCap
{
  [Key]
  public int Id { get; set; }

  [Required]
  public int Level { get; set; } = 0; // The level at which this cap applies
  public int ClassId { get; set; } // FK to Class
  public Class Class { get; set; } = null!;

  public int SkillId { get; set; } // FK to Skill
  public Skill Skill { get; set; } = null!;

  public int SkillLevelId { get; set; } // FK to SkillLevel
  public SkillLevel SkillLevel { get; set; } = null!; // Skill Level tier
}