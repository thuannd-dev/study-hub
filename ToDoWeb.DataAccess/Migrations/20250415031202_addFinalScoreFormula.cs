using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWeb.Migrations
{
    /// <inheritdoc />
    public partial class addFinalScoreFormula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "FinalScore",
                table: "ExamSubmissions",
                type: "float(5)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldComputedColumnSql: "10 * (SELECT COUNT(*) FROM ExamSubmissionDetails WHERE ExamSubmissionId = Id AND IsCorrect = 1) / (SELECT COUNT(*) FROM ExamSubmissionDetails WHERE ExamSubmissionId = Id)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "FinalScore",
                table: "ExamSubmissions",
                type: "float",
                nullable: false,
                computedColumnSql: "10 * (SELECT COUNT(*) FROM ExamSubmissionDetails WHERE ExamSubmissionId = Id AND IsCorrect = 1) / (SELECT COUNT(*) FROM ExamSubmissionDetails WHERE ExamSubmissionId = Id)",
                stored: true,
                oldClrType: typeof(double),
                oldType: "float(5)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
