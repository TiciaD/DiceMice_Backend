using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models;
public class SkillLevel
{
  [Key]
  public int Id { get; set; }

  [Required]
  public string Name { get; set; } = string.Empty; // e.g., "UNSKILLED", "SKILLED", "TRAINED"

  [Required]
  public int Value { get; set; } = 0; // e.g., +0, +2, +4
}