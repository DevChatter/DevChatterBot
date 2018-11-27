using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Modules.WastefulGame.Migrations
{
    public partial class AddEscapeTypeToLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EscapeType",
                table: "Locations",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EscapeType",
                table: "Locations");
        }
    }
}
