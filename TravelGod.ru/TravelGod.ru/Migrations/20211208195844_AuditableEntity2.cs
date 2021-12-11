using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelGod.ru.Migrations
{
    public partial class AuditableEntity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_CreatedBy",
                table: "Trips",
                newName: "IX_Trips_CreatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Ratings",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_CreatedBy",
                table: "Ratings",
                newName: "IX_Ratings_CreatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Messages",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_CreatedBy",
                table: "Messages",
                newName: "IX_Messages_CreatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Comments",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CreatedBy",
                table: "Comments",
                newName: "IX_Comments_CreatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Chats",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_CreatedBy",
                table: "Chats",
                newName: "IX_Chats_CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_CreatedById",
                table: "Chats",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CreatedById",
                table: "Comments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_CreatedById",
                table: "Messages",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_CreatedById",
                table: "Ratings",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_CreatedById",
                table: "Trips",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_CreatedById",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CreatedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_CreatedById",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_CreatedById",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_CreatedById",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Trips",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_CreatedById",
                table: "Trips",
                newName: "IX_Trips_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Ratings",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_CreatedById",
                table: "Ratings",
                newName: "IX_Ratings_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Messages",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_CreatedById",
                table: "Messages",
                newName: "IX_Messages_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Comments",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CreatedById",
                table: "Comments",
                newName: "IX_Comments_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Chats",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_CreatedById",
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
    }
}
