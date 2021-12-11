using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace TravelGod.ru.Migrations
{
    public partial class AuditableEntity6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_CreatedById",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_CreatedById",
                table: "Files");

            migrationBuilder.CreateTable(
                name: "Avatar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avatar_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Avatar_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CreatedById",
                table: "Files",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_CreatedById",
                table: "Files",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict,
                onUpdate: ReferentialAction.Restrict);

            migrationBuilder.CreateIndex(
                name: "IX_Avatar_FileId",
                table: "Avatar",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Avatar_UserId",
                table: "Avatar",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avatar");

            migrationBuilder.DropIndex(
                name: "IX_Files_CreatedById",
                table: "Files");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CreatedById",
                table: "Files",
                column: "CreatedById",
                unique: true);
        }
    }
}