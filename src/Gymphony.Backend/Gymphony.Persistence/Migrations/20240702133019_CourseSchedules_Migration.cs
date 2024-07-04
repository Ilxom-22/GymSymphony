using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymphony.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CourseSchedules_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Day = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseSchedules_Products_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSchedules_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CourseSchedules_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CourseSchedules_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CourseStaff",
                columns: table => new
                {
                    CoursesId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstructorsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStaff", x => new { x.CoursesId, x.InstructorsId });
                    table.ForeignKey(
                        name: "FK_CourseStaff_Products_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStaff_Users_InstructorsId",
                        column: x => x.InstructorsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseScheduleEnrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseScheduleEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseScheduleEnrollments_CourseSchedules_CourseScheduleId",
                        column: x => x.CourseScheduleId,
                        principalTable: "CourseSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseScheduleEnrollments_Subscriptions_CourseSubscriptionId",
                        column: x => x.CourseSubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseScheduleEnrollments_Users_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseScheduleStaff",
                columns: table => new
                {
                    CourseSchedulesId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstructorsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseScheduleStaff", x => new { x.CourseSchedulesId, x.InstructorsId });
                    table.ForeignKey(
                        name: "FK_CourseScheduleStaff_CourseSchedules_CourseSchedulesId",
                        column: x => x.CourseSchedulesId,
                        principalTable: "CourseSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseScheduleStaff_Users_InstructorsId",
                        column: x => x.InstructorsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseScheduleEnrollments_CourseScheduleId",
                table: "CourseScheduleEnrollments",
                column: "CourseScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseScheduleEnrollments_CourseSubscriptionId",
                table: "CourseScheduleEnrollments",
                column: "CourseSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseScheduleEnrollments_MemberId",
                table: "CourseScheduleEnrollments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedules_CourseId",
                table: "CourseSchedules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedules_CreatedByUserId",
                table: "CourseSchedules",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedules_DeletedByUserId",
                table: "CourseSchedules",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSchedules_ModifiedByUserId",
                table: "CourseSchedules",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseScheduleStaff_InstructorsId",
                table: "CourseScheduleStaff",
                column: "InstructorsId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStaff_InstructorsId",
                table: "CourseStaff",
                column: "InstructorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseScheduleEnrollments");

            migrationBuilder.DropTable(
                name: "CourseScheduleStaff");

            migrationBuilder.DropTable(
                name: "CourseStaff");

            migrationBuilder.DropTable(
                name: "CourseSchedules");
        }
    }
}
