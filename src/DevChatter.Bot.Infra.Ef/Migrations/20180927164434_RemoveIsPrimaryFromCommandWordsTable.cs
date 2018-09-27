using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class RemoveIsPrimaryFromCommandWordsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "CommandWords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "CommandWords",
                nullable: false,
                defaultValue: false);
        }
    }
}
