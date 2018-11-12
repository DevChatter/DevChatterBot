using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Modules.WastefulGame.Migrations
{
    public partial class MakeGameEndTypeNonNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EndType",
                table: "GameEndRecords",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EndType",
                table: "GameEndRecords",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
