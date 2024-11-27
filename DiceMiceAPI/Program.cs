using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using DiceMiceAPI.Data;
using DiceMiceAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add forwarded headers configuration for proxies like Render
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
  options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
  options.KnownNetworks.Clear(); // Clear default network restrictions
  options.KnownProxies.Clear();  // Clear default proxy restrictions
});

// Use the port from the environment variable
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

// Check if running in production and configure URLs
if (builder.Environment.IsProduction())
{
  // Use HTTP internally; Render handles HTTPS termination
  builder.WebHost.ConfigureKestrel(serverOptions =>
  {
    serverOptions.ListenAnyIP(int.Parse(port)); // Bind to port  for HTTP
  });
}
else
{
  // Default behavior for development, including HTTPS
  builder.WebHost.UseUrls(builder.Configuration.GetValue<string>("App:Urls") ?? "https://localhost:5001");
}

// Add Database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
    {
      options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:EncryptionKey") ?? ""))
      };
    })
    .AddOAuth("Discord", options =>
    {
      options.AuthorizationEndpoint = "https://discord.com/oauth2/authorize";
      options.Scope.Add("identify");
      options.Scope.Add("email");

      options.CallbackPath = new PathString("/auth/redirect");

      options.ClientId = builder.Configuration.GetValue<string>("Discord:ClientId") ?? "";
      options.ClientSecret = builder.Configuration.GetValue<string>("Discord:ClientSecret") ?? "";

      options.TokenEndpoint = "https://discord.com/api/oauth2/token";
      options.UserInformationEndpoint = "https://discord.com/api/users/@me";

      options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
      options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");

      options.AccessDeniedPath = new PathString("/auth/accessdenied");

      options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
      {
        OnCreatingTicket = async context =>
        {
          var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
          request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

          var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);

          if (!response.IsSuccessStatusCode)
          {
            throw new HttpRequestException($"Failed to retrieve user information ({response.StatusCode})");
          }

          response.EnsureSuccessStatusCode();

          var userJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;

          var discordId = userJson.GetProperty("id").GetString();
          var email = userJson.TryGetProperty("email", out var emailProperty) ? emailProperty.GetString() : null;
          var avatar = userJson.TryGetProperty("avatar", out var avatarProperty) ? avatarProperty.GetString() : null;

          var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

          // Check if the user already exists
          var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.DiscordId == discordId);

          if (existingUser == null)
          {
            // Fetch the "Basic" role
            var basicRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == "BASIC") ?? throw new InvalidOperationException("The 'Basic' role does not exist in the database.");

            // Create and save the new user with the "Basic" role
            var newUser = new User
            {
              DiscordId = discordId ?? string.Empty,
              Email = email ?? string.Empty,
              Avatar = avatar ?? string.Empty,
              RoleId = basicRole.Id // Assign the default role
            };

            dbContext.Users.Add(newUser);
            await dbContext.SaveChangesAsync();
          }
          context.RunClaimActions(userJson);
        }
      };

    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}


if (app.Environment.IsProduction())
{
  app.UseForwardedHeaders(); // Apply forwarded headers
}
else
{
  app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseMiddleware<RoleMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
