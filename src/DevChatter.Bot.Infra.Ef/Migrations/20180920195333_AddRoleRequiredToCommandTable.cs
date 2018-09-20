using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AddRoleRequiredToCommandTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequiredRole",
                table: "CommandWords",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredRole",
                table: "CommandWords");
        }
    }
}
