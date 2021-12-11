using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelGod.ru.Migrations
{
    public partial class AuditableEntity1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_InitiatorId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_UserId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_InitiatorId",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "InitiatorId",
                table: "Trips",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_InitiatorId",
                table: "Trips",
                newName: "IX_Trips_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Ratings",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Ratings",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                newName: "IX_Ratings_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Messages",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Messages",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                newName: "IX_Messages_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Comments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                newName: "IX_Comments_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "InitiatorId",
                table: "Chats",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_InitiatorId",
                table: "Chats",
                newName: "IX_Chats_CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_CreatedBy",
                table: "Chats",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CreatedBy",
                table: "Comments",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_CreatedBy",
                table: "Messages",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_CreatedBy",
                table: "Ratings",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_CreatedBy",
                table: "Trips",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_CreatedBy",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CreatedBy",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_CreatedBy",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_CreatedBy",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_CreatedBy",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Trips",
                newName: "InitiatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_CreatedBy",
                table: "Trips",
                newName: "IX_Trips_InitiatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Ratings",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Ratings",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_CreatedBy",
                table: "Ratings",
                newName: "IX_Ratings_UserId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Messages",
                newName: "DateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_CreatedBy",
                table: "Messages",
                newName: "IX_Messages_UserId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Comments",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CreatedBy",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Chats",
                newName: "InitiatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_CreatedBy",
                table: "Chats",
                newName: "IX_Chats_InitiatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_InitiatorId",
                table: "Chats",
                column: "InitiatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_UserId",
                table: "Ratings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_InitiatorId",
                table: "Trips",
                column: "InitiatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
