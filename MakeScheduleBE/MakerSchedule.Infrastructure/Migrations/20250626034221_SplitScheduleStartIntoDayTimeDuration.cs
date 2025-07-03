using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakerSchedule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SplitScheduleStartIntoDayTimeDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScheduleStart",
                table: "Events",
                newName: "StartTime");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Day",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 26, 3, 42, 20, 973, DateTimeKind.Utc).AddTicks(7147), new DateTime(2025, 6, 26, 3, 42, 20, 973, DateTimeKind.Utc).AddTicks(7148) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 26, 3, 42, 20, 973, DateTimeKind.Utc).AddTicks(7155), new DateTime(2025, 6, 26, 3, 42, 20, 973, DateTimeKind.Utc).AddTicks(7155) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 26, 3, 42, 20, 973, DateTimeKind.Utc).AddTicks(7160), new DateTime(2025, 6, 26, 3, 42, 20, 973, DateTimeKind.Utc).AddTicks(7160) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Events",
                newName: "ScheduleStart");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 19, 24, 42, 416, DateTimeKind.Utc).AddTicks(507), new DateTime(2025, 6, 24, 19, 24, 42, 416, DateTimeKind.Utc).AddTicks(508) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 19, 24, 42, 416, DateTimeKind.Utc).AddTicks(520), new DateTime(2025, 6, 24, 19, 24, 42, 416, DateTimeKind.Utc).AddTicks(520) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 19, 24, 42, 416, DateTimeKind.Utc).AddTicks(525), new DateTime(2025, 6, 24, 19, 24, 42, 416, DateTimeKind.Utc).AddTicks(525) });
        }
    }
}
