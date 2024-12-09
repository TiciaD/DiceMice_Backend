using System;
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
          name: "SkillLevels");

      migrationBuilder.DropTable(
          name: "Skills");

      migrationBuilder.DropTable(
          name: "Classes");

      migrationBuilder.DropTable(
          name: "Houses");

      migrationBuilder.DropTable(
          name: "Counties");

      migrationBuilder.DropTable(
          name: "Users");

      migrationBuilder.DropTable(
          name: "Stats");

      migrationBuilder.DropTable(
          name: "Roles");
    }
  }
}
