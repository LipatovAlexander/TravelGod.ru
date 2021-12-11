using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelGod.ru.Migrations
{
    public partial class Avatars_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatar_Files_FileId",
                table: "Avatar");

            migrationBuilder.DropForeignKey(
                name: "FK_Avatar_Users_UserId",
                table: "Avatar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Avatar",
                table: "Avatar");

            migrationBuilder.RenameTable(
                name: "Avatar",
                newName: "Avatars");

            migrationBuilder.RenameIndex(
                name: "IX_Avatar_UserId",
                table: "Avatars",
                newName: "IX_Avatars_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Avatar_FileId",
                table: "Avatars",
                newName: "IX_Avatars_FileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Avatars",
                table: "Avatars",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Files_FileId",
                table: "Avatars",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Users_UserId",
                table: "Avatars",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_Files_FileId",
                table: "Avatars");

            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_Users_UserId",
                table: "Avatars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Avatars",
                table: "Avatars");

            migrationBuilder.RenameTable(
                name: "Avatars",
                newName: "Avatar");

            migrationBuilder.RenameIndex(
                name: "IX_Avatars_UserId",
                table: "Avatar",
                newName: "IX_Avatar_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Avatars_FileId",
                table: "Avatar",
                newName: "IX_Avatar_FileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Avatar",
                table: "Avatar",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Avatar_Files_FileId",
                table: "Avatar",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avatar_Users_UserId",
                table: "Avatar",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
