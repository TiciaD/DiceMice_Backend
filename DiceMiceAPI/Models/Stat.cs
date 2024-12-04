using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models
{
  public class Stat
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty; // Optional description of the stat
    public bool IsRollBased { get; set; } = false; // Whether this stat is roll-based
  }
}
