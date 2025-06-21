using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWeb.Migrations
{
    /// <inheritdoc />
    public partial class addIsCorrectFormula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsCorrect",
                table: "ExamSubmissionDetails",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComputedColumnSql: "CASE WHEN ChosenAnswer = Question.CorrectAnswer THEN 1 ELSE 0 END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsCorrect",
                table: "ExamSubmissionDetails",
                type: "bit",
                nullable: false,
                computedColumnSql: "CASE WHEN ChosenAnswer = Question.CorrectAnswer THEN 1 ELSE 0 END",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
