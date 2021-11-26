using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelGod.ru.Migrations
{
    public partial class Chat_Fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateChat",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Messages",
                newName: "DateTime");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Comments",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsGroupChat",
                table: "Chats",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGroupChat",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Messages",
                newName: "Date");

            migrationBuilder.AddColumn<bool>(
                name: "CreateChat",
                table: "Trips",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Comments",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Chats",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
