using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DiceMiceAPI.Migrations
{
  /// <inheritdoc />
  public partial class AddCountyStatHouseTables : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {

      migrationBuilder.CreateTable(
          name: "Stats",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: false),
            Description = table.Column<string>(type: "text", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Stats", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Counties",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: false),
            AssociatedStatId = table.Column<int>(type: "integer", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Counties", x => x.Id);
            table.ForeignKey(
                      name: "FK_Counties_Stats_AssociatedStatId",
                      column: x => x.AssociatedStatId,
                      principalTable: "Stats",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "Houses",
          columns: table => new
          {
            Id = table.Column<int>(type: "integer", nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            Name = table.Column<string>(type: "text", nullable: false),
            Bio = table.Column<string>(type: "text", nullable: false),
            Motto = table.Column<string>(type: "text", nullable: false),
            HeadOfHouse = table.Column<string>(type: "text", nullable: false),
            GoldAmount = table.Column<int>(type: "integer", nullable: false),
            UserId = table.Column<int>(type: "integer", nullable: true),
            HouseSeatCountyId = table.Column<int>(type: "integer", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Houses", x => x.Id);
            table.ForeignKey(
                      name: "FK_Houses_Counties_HouseSeatCountyId",
                      column: x => x.HouseSeatCountyId,
                      principalTable: "Counties",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_Houses_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_Counties_AssociatedStatId",
          table: "Counties",
          column: "AssociatedStatId");

      migrationBuilder.CreateIndex(
          name: "IX_Houses_HouseSeatCountyId",
          table: "Houses",
          column: "HouseSeatCountyId");

      migrationBuilder.CreateIndex(
          name: "IX_Houses_UserId",
          table: "Houses",
          column: "UserId",
          unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Houses");

      migrationBuilder.DropTable(
          name: "Counties");

      migrationBuilder.DropTable(
          name: "Stats");
    }
  }
}
