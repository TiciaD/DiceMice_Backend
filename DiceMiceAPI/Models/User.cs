using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models
{
  public class User
  {
    [Key]
    public int Id { get; set; }

    [Required]
    public string DiscordId { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public int RoleId { get; set; } // Foreign key to Role
    public Role Role { get; set; } = null!;
    public House? House { get; set; } // Navigation property
  }
}
