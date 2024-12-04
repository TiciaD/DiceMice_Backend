using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models;
public class ClassSkill
{
  [Key]
  public int Id { get; set; }
  public int ClassId { get; set; } // FK to Class
  public Class Class { get; set; } = null!;

  public int SkillId { get; set; } // FK to Skill
  public Skill Skill { get; set; } = null!; // Skills the class is proficient at
}
