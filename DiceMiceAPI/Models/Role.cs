using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models
{
  public class Role
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)] // Limit the role name length
    public string RoleName { get; set; } = string.Empty;

    public Permission Permissions { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>(); // Navigation property
  }
}