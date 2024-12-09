namespace DiceMiceAPI.Models
{
  public class RefreshRequest
  {
    public string RefreshToken { get; set; } = string.Empty; // The refresh token issued to the user
    public string UserId { get; set; } = string.Empty;     // User ID or other identifier for additional security
  }
}