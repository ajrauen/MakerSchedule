using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakerSchedule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialEmployeeSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "IsActive", "Name", "Phone", "UpdatedAt" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "123 Main St", new DateTime(2025, 6, 16, 4, 32, 10, 446, DateTimeKind.Utc).AddTicks(5021), "john.doe@example.com", true, "John Doe", "123-456-7890", new DateTime(2025, 6, 16, 4, 32, 10, 446, DateTimeKind.Utc).AddTicks(5023) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
