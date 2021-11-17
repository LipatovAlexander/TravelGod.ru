using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelGod.ru.Migrations
{
    public partial class usertrip_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usertrip_Trips_OwnedTripsId",
                table: "usertrip");

            migrationBuilder.RenameColumn(
                name: "OwnedTripsId",
                table: "usertrip",
                newName: "JoinedTripsId");

            migrationBuilder.AddForeignKey(
                name: "FK_usertrip_Trips_JoinedTripsId",
                table: "usertrip",
                column: "JoinedTripsId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usertrip_Trips_JoinedTripsId",
                table: "usertrip");

            migrationBuilder.RenameColumn(
                name: "JoinedTripsId",
                table: "usertrip",
                newName: "OwnedTripsId");

            migrationBuilder.AddForeignKey(
                name: "FK_usertrip_Trips_OwnedTripsId",
                table: "usertrip",
                column: "OwnedTripsId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
