using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class RenameColumnToSettingsTypeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommandNameFull",
                table: "CommandSettings",
                newName: "SettingsTypeName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SettingsTypeName",
                table: "CommandSettings",
                newName: "CommandNameFull");
        }
    }
}
