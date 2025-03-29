using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWeb.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabaseRestrictDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudent_Course_CourseId",
                table: "CourseStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudent_Students_StudentId",
                table: "CourseStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Exams_ExamId",
                table: "ExamQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Questions_QuestionId",
                table: "ExamQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Course_CourseId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubmissionDetails_ExamSubmissions_ExamSubmissionId",
                table: "ExamSubmissionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubmissionDetails_Questions_QuestionId",
                table: "ExamSubmissionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubmissions_CourseStudent_CourseStudentId",
                table: "ExamSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_CourseStudent_CourseStudentId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_School_SId",
                table: "Students");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudent_Course_CourseId",
                table: "CourseStudent",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudent_Students_StudentId",
                table: "CourseStudent",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_Exams_ExamId",
                table: "ExamQuestions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_Questions_QuestionId",
                table: "ExamQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Course_CourseId",
                table: "Exams",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubmissionDetails_ExamSubmissions_ExamSubmissionId",
                table: "ExamSubmissionDetails",
                column: "ExamSubmissionId",
                principalTable: "ExamSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubmissionDetails_Questions_QuestionId",
                table: "ExamSubmissionDetails",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubmissions_CourseStudent_CourseStudentId",
                table: "ExamSubmissions",
                column: "CourseStudentId",
                principalTable: "CourseStudent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_CourseStudent_CourseStudentId",
                table: "Grades",
                column: "CourseStudentId",
                principalTable: "CourseStudent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_School_SId",
                table: "Students",
                column: "SId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudent_Course_CourseId",
                table: "CourseStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudent_Students_StudentId",
                table: "CourseStudent");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Exams_ExamId",
                table: "ExamQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Questions_QuestionId",
                table: "ExamQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Course_CourseId",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubmissionDetails_ExamSubmissions_ExamSubmissionId",
                table: "ExamSubmissionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubmissionDetails_Questions_QuestionId",
                table: "ExamSubmissionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubmissions_CourseStudent_CourseStudentId",
                table: "ExamSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_CourseStudent_CourseStudentId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_School_SId",
                table: "Students");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudent_Course_CourseId",
                table: "CourseStudent",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudent_Students_StudentId",
                table: "CourseStudent",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_Exams_ExamId",
                table: "ExamQuestions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_Questions_QuestionId",
                table: "ExamQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Course_CourseId",
                table: "Exams",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubmissionDetails_ExamSubmissions_ExamSubmissionId",
                table: "ExamSubmissionDetails",
                column: "ExamSubmissionId",
                principalTable: "ExamSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubmissionDetails_Questions_QuestionId",
                table: "ExamSubmissionDetails",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubmissions_CourseStudent_CourseStudentId",
                table: "ExamSubmissions",
                column: "CourseStudentId",
                principalTable: "CourseStudent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_CourseStudent_CourseStudentId",
                table: "Grades",
                column: "CourseStudentId",
                principalTable: "CourseStudent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_School_SId",
                table: "Students",
                column: "SId",
                principalTable: "School",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
