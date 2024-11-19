using Microsoft.EntityFrameworkCore;
using DiceMiceAPI.Models;

namespace DiceMiceAPI.Data
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } // Add DbSet for the User model
  }
}
