using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DiceMiceAPI.Helpers;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DiceMiceAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController(ApplicationDbContext dbContext, IConfiguration configuration) : ControllerBase
{
  private readonly IConfiguration _configuration = configuration;
  private readonly HttpClient _httpClient = new HttpClient();
  private readonly ApplicationDbContext _dbContext = dbContext;

  [HttpGet("login")]
  public IActionResult Login()
  {
    var clientId = _configuration["Discord:ClientId"] ?? "";
    var redirectUri = _configuration["Discord:RedirectUri"] ?? "";
    // Redirect to Discord for OAuth
    var discordOAuthUrl = $"https://discord.com/oauth2/authorize?client_id={clientId}&scope=identify%20email&response_type=code&redirect_uri={redirectUri}";
    return Redirect(discordOAuthUrl);
  }

  [HttpGet("redirect")]
  public async Task<IActionResult> RedirectFromDiscord(string code)
  {
    // Exchange code for access token and fetch user info
    var discordUser = await GetDiscordUserInfo(code);
    if (discordUser == null)
    {
      return Unauthorized("Failed to fetch Discord user info.");
    }

    var discordId = discordUser.Id;
    var email = discordUser.Email;
    var avatar = discordUser.Avatar;
    var username = discordUser.Username;

    // Save user info to the database
    var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.DiscordId == discordId);
    if (existingUser == null)
    {
      // Fetch and assign the "Basic" role
      var basicRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == "BASIC")
          ?? throw new InvalidOperationException("The 'Basic' role does not exist in the database.");

      var newUser = new User
      {
        DiscordId = discordUser.Id ?? string.Empty,
        Email = discordUser.Email ?? string.Empty,
        Avatar = discordUser.Avatar ?? string.Empty,
        RoleId = basicRole.Id,
        Username = discordUser.Username ?? string.Empty
      };

      var refreshToken = GenerateRefreshToken();
      newUser.RefreshToken = refreshToken;
      newUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

      _dbContext.Users.Add(newUser);
      await _dbContext.SaveChangesAsync();
      existingUser = newUser; // Set for JWT generation
    }
    else
    {
      // Update the refresh token and expiry for existing users
      existingUser.RefreshToken = GenerateRefreshToken();
      existingUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
      await _dbContext.SaveChangesAsync();
    }

    // Generate JWT token for the user
    var jwtToken = GenerateJwtToken(discordUser);

    // Redirect to the frontend with tokens in the query string
    var redirectUrl = $"{_configuration["App:FrontendUrl"]}/auth/callback" +
                      $"?token={Uri.EscapeDataString(jwtToken)}" +
                      $"&refreshToken={Uri.EscapeDataString(existingUser.RefreshToken)}";

    return Redirect(redirectUrl);
  }

  [HttpPost("refresh")]
  public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
  {
    if (request.RefreshToken == null)
    {
      return BadRequest("Refresh token is required.");
    }

    var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
    if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
    {
      return Unauthorized("Invalid or expired refresh token.");
    }

    // Generate new Access Token and Refresh Token
    var newAccessToken = GenerateJwtToken(user);
    var newRefreshToken = GenerateRefreshToken();

    // Update user with new refresh token
    user.RefreshToken = newRefreshToken;
    user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // Example: 7-day expiry
    await _dbContext.SaveChangesAsync();

    // Return the new tokens
    return Ok(new
    {
      AccessToken = newAccessToken,
      RefreshToken = newRefreshToken
    });
  }

  [Authorize]
  [HttpGet("me")]
  public async Task<IActionResult> GetCurrentUser()
  {
    // Retrieve the user's Discord ID from the JWT claims
    var discordId = User.FindFirst("id")?.Value;

    if (discordId == null)
    {
      return Unauthorized("Invalid token: no Discord ID found.");
    }

    // Fetch the user from the database
    var user = await _dbContext.Users
        .Include(u => u.Role) // Include role information if needed
        .FirstOrDefaultAsync(u => u.DiscordId == discordId);

    if (user == null)
    {
      return Unauthorized("User not found.");
    }

    // Return the user information
    return Ok(new
    {
      User = new
      {
        user.Username,
        user.Email,
        user.Avatar,
        Role = user.Role?.RoleName // Return role if included
      }
    });
  }

  private async Task<DiscordUserInfo?> GetDiscordUserInfo(string code)
  {
    // Step 1: Exchange code for an access token
    var tokenResponse = await ExchangeCodeForAccessToken(code);

    if (tokenResponse == null) return null;

    // Step 2: Use access token to fetch user info
    var userInfo = await FetchDiscordUserInfo(tokenResponse.AccessToken);
    return userInfo;
  }

  private async Task<TokenResponse?> ExchangeCodeForAccessToken(string code)
  {
    var discordTokenUrl = "https://discord.com/api/oauth2/token";
    var clientId = _configuration["Discord:ClientId"] ?? "";
    var clientSecret = _configuration["Discord:ClientSecret"] ?? "";
    var redirectUri = _configuration["Discord:RedirectUri"] ?? "";

    var content = new FormUrlEncodedContent(
    [
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("redirect_uri", redirectUri),
        ]);

    var response = await _httpClient.PostAsync(discordTokenUrl, content);
    Console.WriteLine("discord/api/oauth2/token response {0}", response.ToString());
    if (!response.IsSuccessStatusCode) return null;

    var responseContent = await response.Content.ReadAsStringAsync();

    return JsonSerializer.Deserialize<TokenResponse>(responseContent);
  }

  private async Task<DiscordUserInfo?> FetchDiscordUserInfo(string accessToken)
  {
    var discordUserUrl = "https://discord.com/api/users/@me";

    var request = new HttpRequestMessage(HttpMethod.Get, discordUserUrl);
    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    var response = await _httpClient.SendAsync(request);
    Console.WriteLine("discord/api/users/@me response {0}", response.ToString());
    if (!response.IsSuccessStatusCode) return null;

    var responseContent = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<DiscordUserInfo>(responseContent);
  }

  private string GenerateJwtToken(User user)
  {
    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:EncryptionKey"] ?? "");
    var issuer = _configuration["Jwt:Issuer"];

    var claims = new[]
    {
            new Claim("id", user.DiscordId.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Username.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: null,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256
        )
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private string GenerateJwtToken(DiscordUserInfo user)
  {
    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:EncryptionKey"] ?? "");
    var issuer = _configuration["Jwt:Issuer"];

    var claims = new[]
    {
        new Claim("id", user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, user.Username.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    };

    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: null,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256
        )
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private static string GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(randomNumber);
    }
    return Convert.ToBase64String(randomNumber);
  }

}
