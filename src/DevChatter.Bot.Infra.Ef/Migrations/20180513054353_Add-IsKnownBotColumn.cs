using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AddIsKnownBotColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsKnownBot",
                table: "ChatUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsKnownBot",
                table: "ChatUsers");
        }
    }
}
