using Microsoft.EntityFrameworkCore;
using DiceMiceAPI.Models;

namespace DiceMiceAPI.Data
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<County> Counties { get; set; }
    public DbSet<Stat> Stats { get; set; }
    public DbSet<House> Houses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // User-Role: A User has one Role, and a Role can have many Users
      modelBuilder.Entity<User>()
        .HasOne(u => u.Role)
        .WithMany(r => r.Users) // Assuming Role has a `Users` collection
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes for roles

      // User-House: A User can have one House, and a House must belong to one User
      modelBuilder.Entity<User>()
        .HasOne(u => u.House)
        .WithOne(h => h.User)
        .HasForeignKey<House>(h => h.UserId)
        .OnDelete(DeleteBehavior.Cascade); // Deleting a User also deletes their House

      // House-County: A House can have one HouseSeatCounty, and a County can have many Houses
      modelBuilder.Entity<House>()
        .HasOne(h => h.HouseSeatCounty)
        .WithMany(c => c.Houses) // Assuming County has a `Houses` collection
        .HasForeignKey(h => h.HouseSeatCountyId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes for counties

      // County-Stat: A County can have one AssociatedStat, and a Stat can have many Counties
      modelBuilder.Entity<County>()
          .HasOne(c => c.AssociatedStat)
          .WithMany(s => s.Counties) // Assuming Stat has a `Counties` collection
          .HasForeignKey(c => c.AssociatedStatId)
          .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes for stats
    }

  }
}
