using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymphony.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Subscriptions_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubscriptionPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionPeriods_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    StripeSubscriptionId = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    LastSubscriptionPeriodId = table.Column<Guid>(type: "uuid", nullable: true),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: true),
                    MembershipPlanId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Products_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Products_MembershipPlanId",
                        column: x => x.MembershipPlanId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_SubscriptionPeriods_LastSubscriptionPeriodId",
                        column: x => x.LastSubscriptionPeriodId,
                        principalTable: "SubscriptionPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPeriods_PaymentId",
                table: "SubscriptionPeriods",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPeriods_SubscriptionId",
                table: "SubscriptionPeriods",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CourseId",
                table: "Subscriptions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_LastSubscriptionPeriodId",
                table: "Subscriptions",
                column: "LastSubscriptionPeriodId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_MemberId",
                table: "Subscriptions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_MembershipPlanId",
                table: "Subscriptions",
                column: "MembershipPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionPeriods_Subscriptions_SubscriptionId",
                table: "SubscriptionPeriods",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionPeriods_Subscriptions_SubscriptionId",
                table: "SubscriptionPeriods");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SubscriptionPeriods");
        }
    }
}
