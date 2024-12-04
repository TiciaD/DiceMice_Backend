using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiceMiceAPI.Migrations
{
  /// <inheritdoc />
  public partial class InitialCreate : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Classes",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: false),
            Description = table.Column<string>(type: "text", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Classes", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Skills",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: false),
            Description = table.Column<string>(type: "text", nullable: false),
            AssociatedStatId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Skills", x => x.Id);
            table.ForeignKey(
                      name: "FK_Skills_Stats_AssociatedStatId",
                      column: x => x.AssociatedStatId,
                      principalTable: "Stats",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "ClassSkillCaps",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Level = table.Column<int>(type: "integer", nullable: false),
            ClassId = table.Column<int>(type: "integer", nullable: false),
            SkillId = table.Column<int>(type: "integer", nullable: false),
            SkillLevelId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_ClassSkillCaps", x => x.Id);
            table.ForeignKey(
                      name: "FK_ClassSkillCaps_Classes_ClassId",
                      column: x => x.ClassId,
                      principalTable: "Classes",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_ClassSkillCaps_SkillLevels_SkillLevelId",
                      column: x => x.SkillLevelId,
                      principalTable: "SkillLevels",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_ClassSkillCaps_Skills_SkillId",
                      column: x => x.SkillId,
                      principalTable: "Skills",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "ClassSkills",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            ClassId = table.Column<int>(type: "integer", nullable: false),
            SkillId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_ClassSkills", x => x.Id);
            table.ForeignKey(
                      name: "FK_ClassSkills_Classes_ClassId",
                      column: x => x.ClassId,
                      principalTable: "Classes",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_ClassSkills_Skills_SkillId",
                      column: x => x.SkillId,
                      principalTable: "Skills",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "Characters",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: false),
            Bio = table.Column<string>(type: "text", nullable: false),
            CountyId = table.Column<int>(type: "integer", nullable: false),
            Trait = table.Column<string>(type: "text", nullable: false),
            Level = table.Column<int>(type: "integer", nullable: false),
            ExperiencePoints = table.Column<int>(type: "integer", nullable: false),
            ClassId = table.Column<int>(type: "integer", nullable: false),
            HouseId = table.Column<int>(type: "integer", nullable: false),
            AvailableSkillRanks = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Characters", x => x.Id);
            table.ForeignKey(
                      name: "FK_Characters_Classes_ClassId",
                      column: x => x.ClassId,
                      principalTable: "Classes",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_Characters_Counties_CountyId",
                      column: x => x.CountyId,
                      principalTable: "Counties",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_Characters_Houses_HouseId",
                      column: x => x.HouseId,
                      principalTable: "Houses",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "CharacterSkills",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            CharacterId = table.Column<int>(type: "integer", nullable: false),
            SkillId = table.Column<int>(type: "integer", nullable: false),
            SkillLevelId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_CharacterSkills", x => x.Id);
            table.ForeignKey(
                      name: "FK_CharacterSkills_Characters_CharacterId",
                      column: x => x.CharacterId,
                      principalTable: "Characters",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_CharacterSkills_SkillLevels_SkillLevelId",
                      column: x => x.SkillLevelId,
                      principalTable: "SkillLevels",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_CharacterSkills_Skills_SkillId",
                      column: x => x.SkillId,
                      principalTable: "Skills",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "CharacterStats",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            CharacterId = table.Column<int>(type: "integer", nullable: false),
            StatId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_CharacterStats", x => x.Id);
            table.ForeignKey(
                      name: "FK_CharacterStats_Characters_CharacterId",
                      column: x => x.CharacterId,
                      principalTable: "Characters",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_CharacterStats_Stats_StatId",
                      column: x => x.StatId,
                      principalTable: "Stats",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_CharacterSkills_CharacterId",
          table: "CharacterSkills",
          column: "CharacterId");

      migrationBuilder.CreateIndex(
          name: "IX_CharacterSkills_SkillId",
          table: "CharacterSkills",
          column: "SkillId");

      migrationBuilder.CreateIndex(
          name: "IX_CharacterSkills_SkillLevelId",
          table: "CharacterSkills",
          column: "SkillLevelId");

      migrationBuilder.CreateIndex(
          name: "IX_CharacterStats_CharacterId",
          table: "CharacterStats",
          column: "CharacterId");

      migrationBuilder.CreateIndex(
          name: "IX_CharacterStats_StatId",
          table: "CharacterStats",
          column: "StatId");

      migrationBuilder.CreateIndex(
          name: "IX_Characters_ClassId",
          table: "Characters",
          column: "ClassId");

      migrationBuilder.CreateIndex(
          name: "IX_Characters_CountyId",
          table: "Characters",
          column: "CountyId");

      migrationBuilder.CreateIndex(
          name: "IX_Characters_HouseId",
          table: "Characters",
          column: "HouseId");

      migrationBuilder.CreateIndex(
          name: "IX_ClassSkillCaps_ClassId",
          table: "ClassSkillCaps",
          column: "ClassId");

      migrationBuilder.CreateIndex(
          name: "IX_ClassSkillCaps_SkillId",
          table: "ClassSkillCaps",
          column: "SkillId");

      migrationBuilder.CreateIndex(
          name: "IX_ClassSkillCaps_SkillLevelId",
          table: "ClassSkillCaps",
          column: "SkillLevelId");

      migrationBuilder.CreateIndex(
          name: "IX_ClassSkills_ClassId",
          table: "ClassSkills",
          column: "ClassId");

      migrationBuilder.CreateIndex(
          name: "IX_ClassSkills_SkillId",
          table: "ClassSkills",
          column: "SkillId");

      migrationBuilder.CreateIndex(
          name: "IX_Skills_AssociatedStatId",
          table: "Skills",
          column: "AssociatedStatId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "CharacterSkills");

      migrationBuilder.DropTable(
          name: "CharacterStats");

      migrationBuilder.DropTable(
          name: "ClassSkillCaps");

      migrationBuilder.DropTable(
          name: "ClassSkills");

      migrationBuilder.DropTable(
          name: "Characters");

      migrationBuilder.DropTable(
          name: "Skills");

      migrationBuilder.DropTable(
          name: "Classes");

    }
  }
}
