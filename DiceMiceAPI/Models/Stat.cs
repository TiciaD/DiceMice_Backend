using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models
{
  public class Stat
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public ICollection<County> Counties { get; set; } = new List<County>(); // Navigation property for one-to-many relationship
  }
}
