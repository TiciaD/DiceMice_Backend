using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models;
public class Skill
{
  [Key]
  public int Id { get; set; }

  [Required]
  public string Name { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;

  // Foreign key and navigation property for Associated Stat
  [Required]
  public int AssociatedStatId { get; set; }
  public Stat AssociatedStat { get; set; } = null!; // Required navigation
}