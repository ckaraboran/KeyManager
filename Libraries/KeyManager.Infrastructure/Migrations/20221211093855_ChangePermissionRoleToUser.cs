using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeyManager.Infrastructure.Migrations
{
    public partial class ChangePermissionRoleToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Roles_RoleId",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Permissions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                newName: "IX_Permissions_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Users_UserId",
                table: "Permissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Users_UserId",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Permissions",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                newName: "IX_Permissions_RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Roles_RoleId",
                table: "Permissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
