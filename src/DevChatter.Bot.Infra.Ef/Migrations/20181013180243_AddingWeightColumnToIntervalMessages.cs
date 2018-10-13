using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AddingWeightColumnToIntervalMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "IntervalMessages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "IntervalMessages");
        }
    }
}
