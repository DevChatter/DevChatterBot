using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataItemStatus = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: true),
                    Tokens = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntervalMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataItemStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntervalMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuoteEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AddedBy = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    DataItemStatus = table.Column<int>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    QuoteId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleResponseMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CommandText = table.Column<string>(nullable: true),
                    DataItemStatus = table.Column<int>(nullable: false),
                    RoleRequired = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleResponseMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatUsers");

            migrationBuilder.DropTable(
                name: "IntervalMessages");

            migrationBuilder.DropTable(
                name: "QuoteEntities");

            migrationBuilder.DropTable(
                name: "SimpleResponseMessages");
        }
    }
}
