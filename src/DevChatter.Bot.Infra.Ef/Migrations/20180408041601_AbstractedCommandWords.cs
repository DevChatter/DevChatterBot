using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AbstractedCommandWords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommandWords",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CommandWord = table.Column<string>(nullable: false),
                    FullTypeName = table.Column<string>(nullable: false),
                    IsPrimary = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandWords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommandWords_CommandWord",
                table: "CommandWords",
                column: "CommandWord");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandWords");
        }
    }
}
