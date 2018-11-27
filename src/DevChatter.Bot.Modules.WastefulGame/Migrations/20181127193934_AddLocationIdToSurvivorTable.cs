using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Modules.WastefulGame.Migrations
{
    public partial class AddLocationIdToSurvivorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Survivors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Survivors_LocationId",
                table: "Survivors",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Survivors_Locations_LocationId",
                table: "Survivors",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Survivors_Locations_LocationId",
                table: "Survivors");

            migrationBuilder.DropIndex(
                name: "IX_Survivors_LocationId",
                table: "Survivors");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Survivors");
        }
    }
}
