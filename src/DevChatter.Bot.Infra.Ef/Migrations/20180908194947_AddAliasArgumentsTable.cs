using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AddAliasArgumentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AliasArgumentEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CommandWordEntityId = table.Column<Guid>(nullable: true),
                    Index = table.Column<int>(nullable: false),
                    Argument = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AliasArgumentEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AliasArgumentEntity_CommandWords_CommandWordEntityId",
                        column: x => x.CommandWordEntityId,
                        principalTable: "CommandWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AliasArgumentEntity_CommandWordEntityId",
                table: "AliasArgumentEntity",
                column: "CommandWordEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AliasArgumentEntity");
        }
    }
}
