using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelGod.ru.Migrations
{
    public partial class AuditableEntity4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Files",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Files");
        }
    }
}
