using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlockApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class RestructureBlocklist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockNumbers");

            migrationBuilder.CreateTable(
                name: "BlockEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntryType = table.Column<int>(type: "INTEGER", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    BankName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    AccountNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    AccountHolderName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    AddedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockEntries_Users_AddedByUserId",
                        column: x => x.AddedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserBlockEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    BlockEntryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Reasons = table.Column<int>(type: "INTEGER", nullable: false),
                    OtherReason = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlockEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBlockEntries_BlockEntries_BlockEntryId",
                        column: x => x.BlockEntryId,
                        principalTable: "BlockEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBlockEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockEntries_AddedByUserId",
                table: "BlockEntries",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockEntries_BankName_AccountNumber",
                table: "BlockEntries",
                columns: new[] { "BankName", "AccountNumber" },
                unique: true,
                filter: "\"AccountNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BlockEntries_PhoneNumber",
                table: "BlockEntries",
                column: "PhoneNumber",
                unique: true,
                filter: "\"PhoneNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlockEntries_BlockEntryId",
                table: "UserBlockEntries",
                column: "BlockEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBlockEntries_UserId_BlockEntryId",
                table: "UserBlockEntries",
                columns: new[] { "UserId", "BlockEntryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBlockEntries");

            migrationBuilder.DropTable(
                name: "BlockEntries");

            migrationBuilder.CreateTable(
                name: "BlockNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockNumbers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockNumbers_UserId_PhoneNumber",
                table: "BlockNumbers",
                columns: new[] { "UserId", "PhoneNumber" },
                unique: true);
        }
    }
}
