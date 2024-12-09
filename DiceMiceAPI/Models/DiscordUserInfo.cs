using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DiceMiceAPI.Models
{
  public class DiscordUserInfo
  {
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
    // [JsonPropertyName("discriminator")]
    // public string Discriminator { get; set; } = string.Empty;
    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = string.Empty;
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    // [JsonPropertyName("verified")]
    // public bool Verified { get; set; } = false;
  }
}