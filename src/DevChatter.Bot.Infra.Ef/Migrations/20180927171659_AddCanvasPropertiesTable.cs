using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AddCanvasPropertiesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CanvasProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CanvasId = table.Column<string>(nullable: true),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    TopY = table.Column<int>(nullable: false),
                    LeftX = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanvasProperties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CanvasProperties_CanvasId",
                table: "CanvasProperties",
                column: "CanvasId",
                unique: true,
                filter: "[CanvasId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CanvasProperties");
        }
    }
}
