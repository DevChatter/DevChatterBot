using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Modules.WastefulGame.Migrations
{
    public partial class AddMoneyColumnToSurvivor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Money",
                table: "Survivors",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Money",
                table: "Survivors");
        }
    }
}
