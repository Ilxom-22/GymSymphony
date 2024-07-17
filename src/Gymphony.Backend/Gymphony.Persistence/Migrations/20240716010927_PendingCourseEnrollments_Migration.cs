using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymphony.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PendingCourseEnrollments_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PendingScheduleEnrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    StripeSessionId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingScheduleEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingScheduleEnrollments_CourseSchedules_CourseScheduleId",
                        column: x => x.CourseScheduleId,
                        principalTable: "CourseSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PendingScheduleEnrollments_Products_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PendingScheduleEnrollments_Users_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PendingScheduleEnrollments_CourseId",
                table: "PendingScheduleEnrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingScheduleEnrollments_CourseScheduleId",
                table: "PendingScheduleEnrollments",
                column: "CourseScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingScheduleEnrollments_MemberId",
                table: "PendingScheduleEnrollments",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingScheduleEnrollments");
        }
    }
}
