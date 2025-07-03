using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakerSchedule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Duration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Events");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 26, 4, 19, 26, 104, DateTimeKind.Utc).AddTicks(8040), new DateTime(2025, 6, 26, 4, 19, 26, 104, DateTimeKind.Utc).AddTicks(8041) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 26, 4, 19, 26, 104, DateTimeKind.Utc).AddTicks(8046), new DateTime(2025, 6, 26, 4, 19, 26, 104, DateTimeKind.Utc).AddTicks(8046) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 26, 4, 19, 26, 104, DateTimeKind.Utc).AddTicks(8051), new DateTime(2025, 6, 26, 4, 19, 26, 104, DateTimeKind.Utc).AddTicks(8052) });
        }
    }
}
