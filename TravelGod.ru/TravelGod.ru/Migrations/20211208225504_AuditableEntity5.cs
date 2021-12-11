using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelGod.ru.Migrations
{
    public partial class AuditableEntity5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_AvatarId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AvatarId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CreatedById",
                table: "Files",
                column: "CreatedById",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_CreatedById",
                table: "Files",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_CreatedById",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_CreatedById",
                table: "Files");

            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AvatarId",
                table: "Users",
                column: "AvatarId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_AvatarId",
                table: "Users",
                column: "AvatarId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
