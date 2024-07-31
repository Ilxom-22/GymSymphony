using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymphony.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Admins_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedByUserId",
                table: "Users",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatedByUserId",
                table: "Users",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatedByUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatedByUserId",
                table: "Users");
        }
    }
}
