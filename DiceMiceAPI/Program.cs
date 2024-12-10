using System.Text;
using DiceMiceAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add distributed memory cache (for session storage)
builder.Services.AddDistributedMemoryCache();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
  // Set the cookie policy options here
  options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

// Add forwarded headers configuration for proxies like Render
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
  options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
  options.KnownNetworks.Clear(); // Clear default network restrictions
  options.KnownProxies.Clear();  // Clear default proxy restrictions
});

// Add session services
builder.Services.AddSession(options =>
{
  options.Cookie.Name = "OAuthSession"; // Customize the cookie name if needed
  options.IdleTimeout = TimeSpan.FromHours(24); // Session timeout duration
  options.Cookie.IsEssential = true; // Essential for session
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:EncryptionKey") ?? ""))
      };
      options.Events = new JwtBearerEvents
      {
        OnChallenge = context =>
        {
          context.HandleResponse(); // Prevent default behavior
          context.Response.StatusCode = StatusCodes.Status401Unauthorized;
          context.Response.ContentType = "application/json";
          return context.Response.WriteAsync("{\"error\":\"Unauthorized access\"}");
        }
      };
    });

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAllOrigins",
      policy =>
      {
        policy
              .WithOrigins(
                "https://dice-mice.vercel.app",
                "https://dicemice-frontend.onrender.com", // Production frontend
                "https://localhost:4200", // Development frontend (Angular default port)
                "http://localhost:3000", // Development frontend (Next default port)
                "https://localhost:3000" // Development frontend (Next https port)
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
      });
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

app.UseCors("AllowAllOrigins");

app.UseCookiePolicy(new CookiePolicyOptions
{
  MinimumSameSitePolicy = SameSiteMode.None, // Enables cross-site cookies
  Secure = CookieSecurePolicy.Always
});
app.UseSession(); // This enables session support
app.UseAuthentication();
app.UseMiddleware<RoleMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
