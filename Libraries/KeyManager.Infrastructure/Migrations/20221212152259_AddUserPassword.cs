using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeyManager.Infrastructure.Migrations
{
    public partial class AddUserPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Password",
                value: "AGv7UyvVBJRuYrSq/IT6S7qqCeCCKHrxhykSwZrrIPW4HTCw5HmjZ6xNnI6k3RQrCA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Password",
                value: "AArmasRqrHkUjd1xSaIDqn74z1M4b0tlbnacYN/FzK/jpB/2cfj9KangrflvroK+rQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Password",
                value: "AEYpnvDw1VY6Z9bqnCr3hAgVwFsx5d9JoNYAT9bu+BCP5pF93DTbwa3pg4up9t5fng==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");
        }
    }
}
