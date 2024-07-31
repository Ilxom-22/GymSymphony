using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymphony.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseScheduleEnrollments_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "EnrollmentDate",
                table: "CourseScheduleEnrollments",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExpiryDate",
                table: "CourseScheduleEnrollments",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "CourseScheduleEnrollments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnrollmentDate",
                table: "CourseScheduleEnrollments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
