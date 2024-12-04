using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiceMiceAPI.Models
{
  public class House
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;

    public string Motto { get; set; } = string.Empty;

    public string HeadOfHouse { get; set; } = string.Empty;

    public int GoldAmount { get; set; } = 0;

    // One-to-One Relationship
    public int? UserId { get; set; } // Nullable to allow a house to exist without an associated user
    public User? User { get; set; } // Navigation property

    // Foreign key to County table
    [ForeignKey(nameof(HouseSeatCounty))]
    public int? HouseSeatCountyId { get; set; }
    public County? HouseSeatCounty { get; set; }
    public ICollection<Character> Characters { get; set; } = new List<Character>(); // Navigation property for one-to-many relationship
  }
}