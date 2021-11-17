using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelGod.ru.Migrations
{
    public partial class default_avatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Files",
                columns: new[] { "Id", "Name", "Path" },
                values: new object[] { 1, "default-avatar.png", "CustomFiles/Avatars/default-avatar.png" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Files",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
