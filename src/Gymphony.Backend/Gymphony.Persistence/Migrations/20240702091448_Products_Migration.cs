using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymphony.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Products_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MembershipPlans");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DurationUnit = table.Column<int>(type: "integer", nullable: false),
                    DurationCount = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ActivationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StripeProductId = table.Column<string>(type: "text", nullable: true),
                    StripePriceId = table.Column<string>(type: "text", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Capacity = table.Column<int>(type: "integer", maxLength: 1000, nullable: true),
                    SessionDurationInMinutes = table.Column<int>(type: "integer", maxLength: 1000, nullable: true),
                    EnrollmentsCountPerWeek = table.Column<int>(type: "integer", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedByUserId",
                table: "Products",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ModifiedByUserId",
                table: "Products",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.CreateTable(
                name: "MembershipPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ActivationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    DurationCount = table.Column<byte>(type: "smallint", nullable: false),
                    DurationUnit = table.Column<int>(type: "integer", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StripeDetails_PriceId = table.Column<string>(type: "text", nullable: true),
                    StripeDetails_ProductId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipPlans_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MembershipPlans_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPlans_CreatedByUserId",
                table: "MembershipPlans",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPlans_ModifiedByUserId",
                table: "MembershipPlans",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPlans_Name",
                table: "MembershipPlans",
                column: "Name",
                unique: true);
        }
    }
}
