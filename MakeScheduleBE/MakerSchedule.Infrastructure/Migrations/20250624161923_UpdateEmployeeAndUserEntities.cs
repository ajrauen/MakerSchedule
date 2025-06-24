using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakerSchedule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeAndUserEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Events_EventId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Events_EventId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EventId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Customers_EventId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "scheduleStart",
                table: "Events",
                newName: "ScheduleStart");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Events",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Events",
                newName: "Leaders");

            migrationBuilder.AddColumn<string>(
                name: "Attendees",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventName",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 16, 19, 22, 677, DateTimeKind.Utc).AddTicks(7671), new DateTime(2025, 6, 24, 16, 19, 22, 677, DateTimeKind.Utc).AddTicks(7672) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 16, 19, 22, 677, DateTimeKind.Utc).AddTicks(7677), new DateTime(2025, 6, 24, 16, 19, 22, 677, DateTimeKind.Utc).AddTicks(7677) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 16, 19, 22, 677, DateTimeKind.Utc).AddTicks(7683), new DateTime(2025, 6, 24, 16, 19, 22, 677, DateTimeKind.Utc).AddTicks(7684) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attendees",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventName",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "ScheduleStart",
                table: "Events",
                newName: "scheduleStart");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Events",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Leaders",
                table: "Events",
                newName: "name");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Employees",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Customers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 3, 13, 39, 237, DateTimeKind.Utc).AddTicks(1291), new DateTime(2025, 6, 24, 3, 13, 39, 237, DateTimeKind.Utc).AddTicks(1291) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 3, 13, 39, 237, DateTimeKind.Utc).AddTicks(1296), new DateTime(2025, 6, 24, 3, 13, 39, 237, DateTimeKind.Utc).AddTicks(1297) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 3, 13, 39, 237, DateTimeKind.Utc).AddTicks(1303), new DateTime(2025, 6, 24, 3, 13, 39, 237, DateTimeKind.Utc).AddTicks(1303) });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "EventId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "EventId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "EventId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2,
                column: "EventId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EventId",
                table: "Employees",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_EventId",
                table: "Customers",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Events_EventId",
                table: "Customers",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Events_EventId",
                table: "Employees",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
