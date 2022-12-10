using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeyManager.Infrastructure.Migrations
{
    public partial class ChangeEventTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Doors_DoorId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Incidents");

            migrationBuilder.RenameIndex(
                name: "IX_Events_UserId",
                table: "Incidents",
                newName: "IX_Incidents_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_DoorId",
                table: "Incidents",
                newName: "IX_Incidents_DoorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Incidents",
                table: "Incidents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Doors_DoorId",
                table: "Incidents",
                column: "DoorId",
                principalTable: "Doors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Users_UserId",
                table: "Incidents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Doors_DoorId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Users_UserId",
                table: "Incidents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Incidents",
                table: "Incidents");

            migrationBuilder.RenameTable(
                name: "Incidents",
                newName: "Events");

            migrationBuilder.RenameIndex(
                name: "IX_Incidents_UserId",
                table: "Events",
                newName: "IX_Events_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Incidents_DoorId",
                table: "Events",
                newName: "IX_Events_DoorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Doors_DoorId",
                table: "Events",
                column: "DoorId",
                principalTable: "Doors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
