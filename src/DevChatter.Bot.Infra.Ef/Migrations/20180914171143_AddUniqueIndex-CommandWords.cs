using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AddUniqueIndexCommandWords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommandWords_CommandWord",
                table: "CommandWords");

            migrationBuilder.CreateIndex(
                name: "IX_CommandWords_CommandWord",
                table: "CommandWords",
                column: "CommandWord",
                unique: true,
                filter: "[CommandWord] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommandWords_CommandWord",
                table: "CommandWords");

            migrationBuilder.CreateIndex(
                name: "IX_CommandWords_CommandWord",
                table: "CommandWords",
                column: "CommandWord");
        }
    }
}
