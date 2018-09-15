using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AddBlastTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlastTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlastTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlastTypes_Name",
                table: "BlastTypes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlastTypes");
        }
    }
}
