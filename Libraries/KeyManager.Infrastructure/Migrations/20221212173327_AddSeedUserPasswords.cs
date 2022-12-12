using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KeyManager.Infrastructure.Migrations
{
    public partial class AddSeedUserPasswords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "OfficeManager");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Director");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Name",
                value: "OfficeUser");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Name", "Password", "Surname", "Username" },
                values: new object[] { "OfficeManager", "AQAAAAEAACcQAAAAEFUGForCcdWHYJyckgcjZ0pFQhrgt4Eqe+6PGIX5ikKvEpA59nqexR8t9vGf9rkqzA==", "OfficeManager", "OfficeManager" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Name", "Password", "Surname", "Username" },
                values: new object[] { "Director", "AQAAAAEAACcQAAAAEBvydgTOzCeMJb7wl7/t5ocKay40ZlGb1S7aMs2y8TH9nu20KZY/HCnmEN8UlOHbBw==", "Director", "Director" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Name", "Password", "Surname", "Username" },
                values: new object[] { "OfficeUser", "AQAAAAEAACcQAAAAEIy8r8Fw3fH8XbRcZQ4Twu9FAm8smsLBIb1rhUFxZ00XEyfRvxTZtSTV7HGESbz/VA==", "OfficeUser", "OfficeUser" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "OfficeUser");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "OfficeManager");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Name",
                value: "Director");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Name", "Password", "Surname", "Username" },
                values: new object[] { "OfficeUser", "AQAAAAEAACcQAAAAEDDH7PXitR5QZs+JkRt9qiTcxsYwhH4gvxwt9pwcEJSeP9+Zbm7RVZvAZOt1FGkZ4A==", "OfficeUser", "OfficeUser" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Name", "Password", "Surname", "Username" },
                values: new object[] { "OfficeManager", "AQAAAAEAACcQAAAAECbcBfVqoFRZHe8z9g9jfMPNpMVME0cbcKxpZK/bFo7sZQoJyXu71iEtCWfpBKkfig==", "OfficeManager", "OfficeManager" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Name", "Password", "Surname", "Username" },
                values: new object[] { "Director", "AQAAAAEAACcQAAAAEAUpNaQIWfZPk8HJcu6NuZ6YFSkGlZPHaHQPqRFMQOZpLAMgYjXHfaARPa31lXj81w==", "Director", "Director" });
        }
    }
}
