using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class AddUserIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CommandUsages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ChatUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CommandUsages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChatUsers");
        }
    }
}
