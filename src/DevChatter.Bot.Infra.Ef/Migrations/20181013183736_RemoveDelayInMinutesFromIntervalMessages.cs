using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class RemoveDelayInMinutesFromIntervalMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelayInMinutes",
                table: "IntervalMessages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DelayInMinutes",
                table: "IntervalMessages",
                nullable: false,
                defaultValue: 0);
        }
    }
}
