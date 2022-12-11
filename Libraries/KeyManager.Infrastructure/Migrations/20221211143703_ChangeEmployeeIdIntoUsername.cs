using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeyManager.Infrastructure.Migrations
{
    public partial class ChangeEmployeeIdIntoUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
