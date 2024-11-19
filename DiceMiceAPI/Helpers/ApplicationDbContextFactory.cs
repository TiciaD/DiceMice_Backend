using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DiceMiceAPI.Data
{
  public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    public ApplicationDbContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

      // Load the connection string from appsettings.json
      IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.Development.json")
          .Build();

      var connectionString = configuration.GetConnectionString("DefaultConnection");
      Console.WriteLine($"Connection String: {connectionString}");

      optionsBuilder.UseNpgsql(connectionString);

      return new ApplicationDbContext(optionsBuilder.Options);
    }
  }
}
