using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AddSurvivorAndTeamTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "GameEndRecords");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GameEndRecords");

            migrationBuilder.AddColumn<Guid>(
                name: "SurvivorId",
                table: "GameEndRecords",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Survivors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    TeamId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Survivors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Survivors_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameEndRecords_SurvivorId",
                table: "GameEndRecords",
                column: "SurvivorId");

            migrationBuilder.CreateIndex(
                name: "IX_Survivors_TeamId",
                table: "Survivors",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEndRecords_Survivors_SurvivorId",
                table: "GameEndRecords",
                column: "SurvivorId",
                principalTable: "Survivors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEndRecords_Survivors_SurvivorId",
                table: "GameEndRecords");

            migrationBuilder.DropTable(
                name: "Survivors");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_GameEndRecords_SurvivorId",
                table: "GameEndRecords");

            migrationBuilder.DropColumn(
                name: "SurvivorId",
                table: "GameEndRecords");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "GameEndRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GameEndRecords",
                nullable: true);
        }
    }
}
