using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlockApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPointsPackagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointsPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Points = table.Column<int>(type: "INTEGER", nullable: false),
                    PriceTHB = table.Column<decimal>(type: "TEXT", nullable: false),
                    BonusPoints = table.Column<int>(type: "INTEGER", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsPackages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PointsPackages",
                columns: new[] { "Id", "BonusPoints", "CreatedAt", "DisplayOrder", "IsActive", "Points", "PriceTHB", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, 100, 100m, null },
                    { 2, 50, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, true, 500, 500m, null },
                    { 3, 150, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, true, 1000, 1000m, null },
                    { 4, 1000, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, true, 5000, 5000m, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointsPackages");
        }
    }
}
