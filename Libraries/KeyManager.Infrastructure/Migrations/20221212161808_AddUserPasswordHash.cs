using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeyManager.Infrastructure.Migrations
{
    public partial class AddUserPasswordHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Password",
                value: "AQAAAAEAACcQAAAAEDDH7PXitR5QZs+JkRt9qiTcxsYwhH4gvxwt9pwcEJSeP9+Zbm7RVZvAZOt1FGkZ4A==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Password",
                value: "AQAAAAEAACcQAAAAECbcBfVqoFRZHe8z9g9jfMPNpMVME0cbcKxpZK/bFo7sZQoJyXu71iEtCWfpBKkfig==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Password",
                value: "AQAAAAEAACcQAAAAEAUpNaQIWfZPk8HJcu6NuZ6YFSkGlZPHaHQPqRFMQOZpLAMgYjXHfaARPa31lXj81w==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
