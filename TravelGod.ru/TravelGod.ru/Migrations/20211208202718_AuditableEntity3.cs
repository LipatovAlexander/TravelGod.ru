using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelGod.ru.Migrations
{
    public partial class AuditableEntity3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_CreatedById",
                table: "Trips");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Trips",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Trips",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Trips",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Trips",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Ratings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Ratings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Messages",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Files",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Files",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Comments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Chats",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Chats",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Chats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ModifiedById",
                table: "Trips",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ModifiedById",
                table: "Ratings",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ModifiedById",
                table: "Messages",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ModifiedById",
                table: "Files",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ModifiedById",
                table: "Comments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ModifiedById",
                table: "Chats",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_ModifiedById",
                table: "Chats",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_ModifiedById",
                table: "Comments",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_ModifiedById",
                table: "Files",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ModifiedById",
                table: "Messages",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_ModifiedById",
                table: "Ratings",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_CreatedById",
                table: "Trips",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_ModifiedById",
                table: "Trips",
                column: "ModifiedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_ModifiedById",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_ModifiedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_ModifiedById",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ModifiedById",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_ModifiedById",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_CreatedById",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_ModifiedById",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_ModifiedById",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_ModifiedById",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ModifiedById",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Files_ModifiedById",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ModifiedById",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Chats_ModifiedById",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Chats");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_CreatedById",
                table: "Trips",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
