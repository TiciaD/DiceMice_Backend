using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiceMiceAPI.Models
{
  public class County
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    // Foreign key to Stat table
    [ForeignKey(nameof(AssociatedStat))]
    public int AssociatedStatId { get; set; }
    public Stat? AssociatedStat { get; set; }

    public ICollection<House> Houses { get; set; } = new List<House>(); // Navigation property for one-to-many relationship
  }
}
