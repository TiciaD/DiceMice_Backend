using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models;
public class Class
{
  [Key]
  public int Id { get; set; }

  [Required]
  public string Name { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;
}