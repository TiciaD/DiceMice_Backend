using System.ComponentModel.DataAnnotations;

namespace DiceMiceAPI.Models;
public class CharacterStat
{
  [Key]
  public int Id { get; set; }
  public int CharacterId { get; set; } // FK to Character
  public Character Character { get; set; } = null!;

  public int StatId { get; set; } // FK to Stat
  public Stat Stat { get; set; } = null!;
}