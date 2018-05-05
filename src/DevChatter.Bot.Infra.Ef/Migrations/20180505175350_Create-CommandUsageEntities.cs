using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class CreateCommandUsageEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommandUsages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CommandWord = table.Column<string>(nullable: true),
                    DateTimeUsed = table.Column<DateTimeOffset>(nullable: false),
                    FullTypeName = table.Column<string>(nullable: true),
                    UserDisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandUsages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandUsages");
        }
    }
}
