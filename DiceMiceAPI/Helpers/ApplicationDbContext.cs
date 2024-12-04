using Microsoft.EntityFrameworkCore;
using DiceMiceAPI.Models;

namespace DiceMiceAPI.Helpers
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Character> Characters { get; set; } = null!;
    public DbSet<CharacterSkill> CharacterSkills { get; set; } = null!;
    public DbSet<CharacterStat> CharacterStats { get; set; } = null!;
    public DbSet<Class> Classes { get; set; } = null!;
    public DbSet<ClassSkill> ClassSkills { get; set; } = null!;
    public DbSet<ClassSkillCap> ClassSkillCaps { get; set; } = null!;
    public DbSet<County> Counties { get; set; } = null!;
    public DbSet<House> Houses { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Skill> Skills { get; set; } = null!;
    public DbSet<SkillLevel> SkillLevels { get; set; } = null!;
    public DbSet<Stat> Stats { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      /**** User Configurations ****/
      // User-Role: A User has one Role, and a Role can have many Users
      modelBuilder.Entity<User>()
        .HasOne(u => u.Role)
        .WithMany(r => r.Users) // Assuming Role has a `Users` collection
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes for roles


      /**** County Configurations ****/
      // County-Stat: A County can have one AssociatedStat, and a Stat can have many Counties
      modelBuilder.Entity<County>()
          .HasOne(c => c.AssociatedStat)
          .WithMany()
          .HasForeignKey(c => c.AssociatedStatId)
          .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes for stats


      /**** House Configurations ****/
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


      /**** Character Configurations ****/
      // Character-County: A Character can have one OriginCounty, and a County can have many Houses
      modelBuilder.Entity<Character>()
            .HasOne(c => c.OriginCounty)
            .WithMany()
            .HasForeignKey(c => c.CountyId)
            .OnDelete(DeleteBehavior.Restrict);

      // Character-Class: A Character can have one Class, and a Class can have many Characters
      modelBuilder.Entity<Character>()
          .HasOne(c => c.Class)
          .WithMany()
          .HasForeignKey(c => c.ClassId)
          .OnDelete(DeleteBehavior.Restrict);

      // Character-House: A Character can have one House, and a House can have many Characters
      modelBuilder.Entity<Character>()
          .HasOne(c => c.House)
          .WithMany(c => c.Characters) // Assuming House has a `Characters` collection
          .HasForeignKey(c => c.HouseId)
          .OnDelete(DeleteBehavior.Restrict);


      /**** Skill Configurations ****/
      // Skill-Stat: A Skill can have one AssociatedStat, and a Stat can have many Skills
      modelBuilder.Entity<Skill>()
          .HasOne(s => s.AssociatedStat)
          .WithMany()
          .HasForeignKey(s => s.AssociatedStatId)
          .OnDelete(DeleteBehavior.Restrict);


      /**** CharacterSkill Configurations ****/
      // CharacterSkill-Character: A Character Skill can have one Character, and a Character can have many CharacterSkills
      modelBuilder.Entity<CharacterSkill>()
          .HasOne(cs => cs.Character)
          .WithMany()
          .HasForeignKey(cs => cs.CharacterId)
          .OnDelete(DeleteBehavior.Cascade);

      // CharacterSkill-Skill: A Character Skill can have one Skill, and a Skill can have many CharacterSkills
      modelBuilder.Entity<CharacterSkill>()
          .HasOne(cs => cs.Skill)
          .WithMany()
          .HasForeignKey(cs => cs.SkillId)
          .OnDelete(DeleteBehavior.Restrict);

      // CharacterSkill-SkillLevel: A Character Skill can have one CurrentSkillLevel, and a SkillLevel can have many CharacterSkills
      modelBuilder.Entity<CharacterSkill>()
          .HasOne(cs => cs.CurrentSkillLevel)
          .WithMany()
          .HasForeignKey(cs => cs.SkillLevelId)
          .OnDelete(DeleteBehavior.Restrict);


      /**** CharacterStat Configurations ****/
      // CharacterStat-Character: A Character Stat can have one Character, and a Character can have many CharacterStats
      modelBuilder.Entity<CharacterStat>()
          .HasOne(cs => cs.Character)
          .WithMany()
          .HasForeignKey(cs => cs.CharacterId)
          .OnDelete(DeleteBehavior.Cascade);

      // CharacterStat-Stat: A Character Stat can have one Stat, and a Stat can have many CharacterStats
      modelBuilder.Entity<CharacterStat>()
          .HasOne(cs => cs.Stat)
          .WithMany()
          .HasForeignKey(cs => cs.StatId)
          .OnDelete(DeleteBehavior.Restrict);


      /**** ClassSkill Configurations ****/
      // ClassSkill-Class: A Class Skill can have one Class, and a Class can have many ClassSkills
      modelBuilder.Entity<ClassSkill>()
          .HasOne(cs => cs.Class)
          .WithMany()
          .HasForeignKey(cs => cs.ClassId)
          .OnDelete(DeleteBehavior.Cascade);

      // ClassSkill-Skill: A Class Skill can have one Skill, and a Skill can have many ClassSkills
      modelBuilder.Entity<ClassSkill>()
          .HasOne(cs => cs.Skill)
          .WithMany()
          .HasForeignKey(cs => cs.SkillId)
          .OnDelete(DeleteBehavior.Restrict);


      /**** ClassSkillCap Configurations ****/
      // ClassSkillCap-Class: A Class SkillCap can have one Class, and a Class can have many ClassSkillCaps
      modelBuilder.Entity<ClassSkillCap>()
          .HasOne(csc => csc.Class)
          .WithMany()
          .HasForeignKey(csc => csc.ClassId)
          .OnDelete(DeleteBehavior.Cascade);

      // ClassSkillCap-Skill: A Class SkillCap can have one Skill, and a Skill can have many ClassSkillCaps
      modelBuilder.Entity<ClassSkillCap>()
          .HasOne(csc => csc.Skill)
          .WithMany()
          .HasForeignKey(csc => csc.SkillId)
          .OnDelete(DeleteBehavior.Restrict);

      // ClassSkillCap-SkillLevel: A Class SkillCap can have one SkillLevel, and a SkillLevel can have many ClassSkillCaps
      modelBuilder.Entity<ClassSkillCap>()
          .HasOne(csc => csc.SkillLevel)
          .WithMany()
          .HasForeignKey(csc => csc.SkillLevelId)
          .OnDelete(DeleteBehavior.Restrict);
    }


  }
}
