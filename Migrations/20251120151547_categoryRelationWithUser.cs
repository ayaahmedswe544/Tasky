using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasky.Migrations
{
    /// <inheritdoc />
    public partial class categoryRelationWithUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_AppUserId",
                table: "Categories",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_AppUserId",
                table: "Categories",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_AppUserId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_AppUserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Categories");
        }
    }
}
