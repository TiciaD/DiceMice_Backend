using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.DTOs;
public class ClassCreateDto
{
  [Required]
  public string Name { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;
}

public class ClassReadDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
}

public class ClassUpdateDto
{
  [Required]
  public int Id { get; set; }
  [Required]
  public string Name { get; set; } = string.Empty;

  public string Description { get; set; } = string.Empty;
}
