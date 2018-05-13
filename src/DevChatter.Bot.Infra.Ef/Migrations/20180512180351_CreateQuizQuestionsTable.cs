using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Infra.Ef.Migrations
{
    public partial class CreateQuizQuestionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CorrectAnswer = table.Column<string>(nullable: true),
                    Hint1 = table.Column<string>(nullable: true),
                    Hint2 = table.Column<string>(nullable: true),
                    MainQuestion = table.Column<string>(nullable: true),
                    WrongAnswer1 = table.Column<string>(nullable: true),
                    WrongAnswer2 = table.Column<string>(nullable: true),
                    WrongAnswer3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizQuestions");
        }
    }
}
