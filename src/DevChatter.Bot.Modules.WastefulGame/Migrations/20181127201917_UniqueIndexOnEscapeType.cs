using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Modules.WastefulGame.Migrations
{
    public partial class UniqueIndexOnEscapeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EscapeType",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Locations_EscapeType",
                table: "Locations",
                column: "EscapeType",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_EscapeType",
                table: "Locations");

            migrationBuilder.AlterColumn<string>(
                name: "EscapeType",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
