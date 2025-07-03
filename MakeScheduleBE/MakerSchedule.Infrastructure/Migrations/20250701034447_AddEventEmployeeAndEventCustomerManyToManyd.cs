using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakerSchedule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventEmployeeAndEventCustomerManyToManyd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 3, 44, 47, 228, DateTimeKind.Utc).AddTicks(4249), new DateTime(2025, 7, 1, 3, 44, 47, 228, DateTimeKind.Utc).AddTicks(4250) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 3, 44, 47, 228, DateTimeKind.Utc).AddTicks(4261), new DateTime(2025, 7, 1, 3, 44, 47, 228, DateTimeKind.Utc).AddTicks(4261) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 1, 3, 44, 47, 228, DateTimeKind.Utc).AddTicks(4266), new DateTime(2025, 7, 1, 3, 44, 47, 228, DateTimeKind.Utc).AddTicks(4266) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 27, 0, 5, 26, 638, DateTimeKind.Utc).AddTicks(4979), new DateTime(2025, 6, 27, 0, 5, 26, 638, DateTimeKind.Utc).AddTicks(4980) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 27, 0, 5, 26, 638, DateTimeKind.Utc).AddTicks(4985), new DateTime(2025, 6, 27, 0, 5, 26, 638, DateTimeKind.Utc).AddTicks(4985) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 27, 0, 5, 26, 638, DateTimeKind.Utc).AddTicks(5011), new DateTime(2025, 6, 27, 0, 5, 26, 638, DateTimeKind.Utc).AddTicks(5011) });
        }
    }
}
