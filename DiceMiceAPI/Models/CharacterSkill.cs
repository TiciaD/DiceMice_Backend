using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models;
public class CharacterSkill
{
  [Key]
  public int Id { get; set; }
  public int CharacterId { get; set; } // FK to Character
  public Character Character { get; set; } = null!;

  public int SkillId { get; set; } // FK to Skill
  public Skill Skill { get; set; } = null!;

  public int SkillLevelId { get; set; } // FK to SkillLevel
  public SkillLevel CurrentSkillLevel { get; set; } = null!; // Character's current skill level at specified Skill
}