using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Modules.WastefulGame.Migrations
{
    public partial class AddUsesColumnToShopItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Uses",
                table: "ShopItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uses",
                table: "ShopItems");
        }
    }
}
