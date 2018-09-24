using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AddAliasTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AliasArgumentEntity_CommandWords_CommandWordEntityId",
                table: "AliasArgumentEntity");

            migrationBuilder.RenameColumn(
                name: "CommandWordEntityId",
                table: "AliasArgumentEntity",
                newName: "AliasId");

            migrationBuilder.RenameIndex(
                name: "IX_AliasArgumentEntity_CommandWordEntityId",
                table: "AliasArgumentEntity",
                newName: "IX_AliasArgumentEntity_AliasId");

            migrationBuilder.CreateTable(
                name: "AliasEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CommandId = table.Column<Guid>(nullable: true),
                    Word = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AliasEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AliasEntity_CommandWords_CommandId",
                        column: x => x.CommandId,
                        principalTable: "CommandWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AliasEntity_CommandId",
                table: "AliasEntity",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_AliasEntity_Word",
                table: "AliasEntity",
                column: "Word",
                unique: true,
                filter: "[Word] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AliasEntity");

            migrationBuilder.RenameColumn(
                name: "AliasId",
                table: "AliasArgumentEntity",
                newName: "CommandWordEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_AliasArgumentEntity_AliasId",
                table: "AliasArgumentEntity",
                newName: "IX_AliasArgumentEntity_CommandWordEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AliasArgumentEntity_CommandWords_CommandWordEntityId",
                table: "AliasArgumentEntity",
                column: "CommandWordEntityId",
                principalTable: "CommandWords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
