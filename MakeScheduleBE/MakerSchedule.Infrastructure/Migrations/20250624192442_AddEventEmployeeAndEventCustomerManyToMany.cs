using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MakerSchedule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventEmployeeAndEventCustomerManyToMany : Migration
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

            migrationBuilder.CreateTable(
                name: "CustomerEvent",
                columns: table => new
                {
                    AttendeesId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventsAttendedId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerEvent", x => new { x.AttendeesId, x.EventsAttendedId });
                    table.ForeignKey(
                        name: "FK_CustomerEvent_Customers_AttendeesId",
                        column: x => x.AttendeesId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerEvent_Events_EventsAttendedId",
                        column: x => x.EventsAttendedId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEvent",
                columns: table => new
                {
                    EventsLedId = table.Column<int>(type: "INTEGER", nullable: false),
                    LeadersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvent", x => new { x.EventsLedId, x.LeadersId });
                    table.ForeignKey(
                        name: "FK_EmployeeEvent_Employees_LeadersId",
                        column: x => x.LeadersId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEvent_Events_EventsLedId",
                        column: x => x.EventsLedId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CustomerEvent_EventsAttendedId",
                table: "CustomerEvent",
                column: "EventsAttendedId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvent_LeadersId",
                table: "EmployeeEvent",
                column: "LeadersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerEvent");

            migrationBuilder.DropTable(
                name: "EmployeeEvent");

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
                values: new object[] { new DateTime(2025, 6, 24, 19, 6, 31, 428, DateTimeKind.Utc).AddTicks(5374), new DateTime(2025, 6, 24, 19, 6, 31, 428, DateTimeKind.Utc).AddTicks(5375) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 19, 6, 31, 428, DateTimeKind.Utc).AddTicks(5385), new DateTime(2025, 6, 24, 19, 6, 31, 428, DateTimeKind.Utc).AddTicks(5385) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 6, 24, 19, 6, 31, 428, DateTimeKind.Utc).AddTicks(5390), new DateTime(2025, 6, 24, 19, 6, 31, 428, DateTimeKind.Utc).AddTicks(5390) });

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
