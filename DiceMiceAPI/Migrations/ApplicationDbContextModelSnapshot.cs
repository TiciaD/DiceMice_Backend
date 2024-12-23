﻿// <auto-generated />
using System;
using DiceMiceAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiceMiceAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DiceMiceAPI.Models.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AvailableSkillRanks")
                        .HasColumnType("integer");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ClassId")
                        .HasColumnType("integer");

                    b.Property<int>("CountyId")
                        .HasColumnType("integer");

                    b.Property<int>("ExperiencePoints")
                        .HasColumnType("integer");

                    b.Property<int>("HouseId")
                        .HasColumnType("integer");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Trait")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("CountyId");

                    b.HasIndex("HouseId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.CharacterSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CharacterId")
                        .HasColumnType("integer");

                    b.Property<int>("SkillId")
                        .HasColumnType("integer");

                    b.Property<int>("SkillLevelId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("SkillId");

                    b.HasIndex("SkillLevelId");

                    b.ToTable("CharacterSkills");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.CharacterStat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CharacterId")
                        .HasColumnType("integer");

                    b.Property<int>("StatId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("StatId");

                    b.ToTable("CharacterStats");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.ClassSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClassId")
                        .HasColumnType("integer");

                    b.Property<int>("SkillId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("SkillId");

                    b.ToTable("ClassSkills");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.ClassSkillCap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClassId")
                        .HasColumnType("integer");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<int>("SkillId")
                        .HasColumnType("integer");

                    b.Property<int>("SkillLevelId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("SkillId");

                    b.HasIndex("SkillLevelId");

                    b.ToTable("ClassSkillCaps");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.County", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AssociatedStatId")
                        .HasColumnType("integer");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AssociatedStatId");

                    b.ToTable("Counties");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.House", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("GoldAmount")
                        .HasColumnType("integer");

                    b.Property<string>("HeadOfHouse")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("HouseSeatCountyId")
                        .HasColumnType("integer");

                    b.Property<string>("Motto")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("HouseSeatCountyId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Permissions")
                        .HasColumnType("integer");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AssociatedStatId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AssociatedStatId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.SkillLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("SkillLevels");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.Stat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsRollBased")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Stats");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DiscordId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("RefreshTokenExpiry")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.Character", b =>
                {
                    b.HasOne("DiceMiceAPI.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DiceMiceAPI.Models.County", "OriginCounty")
                        .WithMany()
                        .HasForeignKey("CountyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DiceMiceAPI.Models.House", "House")
                        .WithMany("Characters")
                        .HasForeignKey("HouseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("House");

                    b.Navigation("OriginCounty");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.CharacterSkill", b =>
                {
                    b.HasOne("DiceMiceAPI.Models.Character", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiceMiceAPI.Models.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DiceMiceAPI.Models.SkillLevel", "CurrentSkillLevel")
                        .WithMany()
                        .HasForeignKey("SkillLevelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("CurrentSkillLevel");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.CharacterStat", b =>
                {
                    b.HasOne("DiceMiceAPI.Models.Character", "Character")
                        .WithMany()
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiceMiceAPI.Models.Stat", "Stat")
                        .WithMany()
                        .HasForeignKey("StatId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("Stat");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.ClassSkill", b =>
                {
                    b.HasOne("DiceMiceAPI.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiceMiceAPI.Models.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.ClassSkillCap", b =>
                {
                    b.HasOne("DiceMiceAPI.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiceMiceAPI.Models.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DiceMiceAPI.Models.SkillLevel", "SkillLevel")
                        .WithMany()
                        .HasForeignKey("SkillLevelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Skill");

                    b.Navigation("SkillLevel");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.County", b =>
                {
                    b.HasOne("DiceMiceAPI.Models.Stat", "AssociatedStat")
                        .WithMany()
                        .HasForeignKey("AssociatedStatId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AssociatedStat");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.House", b =>
                {
                    b.HasOne("DiceMiceAPI.Models.County", "HouseSeatCounty")
                        .WithMany("Houses")
                        .HasForeignKey("HouseSeatCountyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DiceMiceAPI.Models.User", "User")
                        .WithOne("House")
                        .HasForeignKey("DiceMiceAPI.Models.House", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("HouseSeatCounty");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.Skill", b =>
                {
                    b.HasOne("DiceMiceAPI.Models.Stat", "AssociatedStat")
                        .WithMany()
                        .HasForeignKey("AssociatedStatId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AssociatedStat");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.User", b =>
                {
                    b.HasOne("DiceMiceAPI.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.County", b =>
                {
                    b.Navigation("Houses");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.House", b =>
                {
                    b.Navigation("Characters");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("DiceMiceAPI.Models.User", b =>
                {
                    b.Navigation("House");
                });
#pragma warning restore 612, 618
        }
    }
}
