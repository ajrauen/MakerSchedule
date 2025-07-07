using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakerSchedule.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Events",
            keyColumn: "Id",
            keyValue: 1,
            column: "ScheduleStart",
            value: new DateTime(2025, 7, 7, 5, 17, 28, 547, DateTimeKind.Utc).AddTicks(2581));

        migrationBuilder.UpdateData(
            table: "Events",
            keyColumn: "Id",
            keyValue: 2,
            column: "ScheduleStart",
            value: new DateTime(2025, 7, 8, 5, 17, 28, 547, DateTimeKind.Utc).AddTicks(2593));

        migrationBuilder.UpdateData(
            table: "Events",
            keyColumn: "Id",
            keyValue: 3,
            column: "ScheduleStart",
            value: new DateTime(2025, 7, 10, 5, 17, 28, 547, DateTimeKind.Utc).AddTicks(2594));

        migrationBuilder.UpdateData(
            table: "Events",
            keyColumn: "Id",
            keyValue: 4,
            column: "ScheduleStart",
            value: new DateTime(2025, 7, 12, 5, 17, 28, 547, DateTimeKind.Utc).AddTicks(2596));

        migrationBuilder.UpdateData(
            table: "Events",
            keyColumn: "Id",
            keyValue: 5,
            column: "ScheduleStart",
            value: new DateTime(2025, 7, 15, 5, 17, 28, 547, DateTimeKind.Utc).AddTicks(2597));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "Events",
            keyColumn: "Id",
            keyValue: 1,
            column: "ScheduleStart",
            value: new DateTime(2025, 7, 7, 5, 13, 8, 725, DateTimeKind.Utc).AddTicks(8197));

        migrationBuilder.UpdateData(
            table: "Events",
            keyColumn: "Id",
            keyValue: 2,
            column: "ScheduleStart",
            value: new DateTime(2025, 7, 8, 5, 13, 8, 725, DateTimeKind.Utc).AddTicks(8206));

        migrationBuilder.UpdateData(
            table: "Events",
            keyColumn: "Id",
            keyValue: 3,
            column: "ScheduleStart",
            value: new DateTime(2025, 7, 10, 5, 13, 8, 725, DateTimeKind.Utc).AddTicks(8208));

        migrationBuilder.UpdateData(
            table: "Events",
            keyColumn: "Id",
            keyValue: 4,
            column: "ScheduleStart",
            value: new DateTime(2025, 7, 12, 5, 13, 8, 725, DateTimeKind.Utc).AddTicks(8209));

        migrationBuilder.UpdateData(
            table: "Events",
            keyColumn: "Id",
            keyValue: 5,
            column: "ScheduleStart",
            value: new DateTime(2025, 7, 15, 5, 13, 8, 725, DateTimeKind.Utc).AddTicks(8210));
    }
}
