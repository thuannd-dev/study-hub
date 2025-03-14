using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWeb.Migrations
{
    /// <inheritdoc />
    public partial class addIDelete2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "CourseStudent");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "CourseStudent");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CourseStudent");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "School",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeleteBy",
                table: "School",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "School",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "School");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "School");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "School");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "Grades",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeleteBy",
                table: "Grades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Grades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "CourseStudent",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeleteBy",
                table: "CourseStudent",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CourseStudent",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
