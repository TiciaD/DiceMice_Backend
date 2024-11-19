using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DiceMiceAPI.Data
{
  public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    public ApplicationDbContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

      // Determine the environment (development or production)
      var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

      // Load the appropriate configuration file based on the environment
      IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile($"appsettings.{environment}.json") // Load the correct appsettings file based on the environment
          .Build();


      var connectionString = configuration.GetConnectionString("DefaultConnection");
      Console.WriteLine($"Connection String: {connectionString}");

      optionsBuilder.UseNpgsql(connectionString);

      return new ApplicationDbContext(optionsBuilder.Options);
    }
  }
}
