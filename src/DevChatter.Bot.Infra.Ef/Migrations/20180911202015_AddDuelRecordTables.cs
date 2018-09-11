using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AddDuelRecordTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DuelsPlayed",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DuelType = table.Column<string>(nullable: true),
                    DateDueled = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuelsPlayed", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DuelPlayerRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DuelId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserDisplayName = table.Column<string>(nullable: true),
                    WinLossTie = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuelPlayerRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuelPlayerRecord_DuelsPlayed_DuelId",
                        column: x => x.DuelId,
                        principalTable: "DuelsPlayed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuelPlayerRecord_DuelId",
                table: "DuelPlayerRecord",
                column: "DuelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuelPlayerRecord");

            migrationBuilder.DropTable(
                name: "DuelsPlayed");
        }
    }
}
