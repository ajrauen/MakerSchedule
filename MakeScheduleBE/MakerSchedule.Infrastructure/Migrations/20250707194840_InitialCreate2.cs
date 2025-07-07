using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MakerSchedule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserType = table.Column<int>(type: "INTEGER", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PreferredContactMethod = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Department = table.Column<string>(type: "TEXT", nullable: false),
                    Position = table.Column<string>(type: "TEXT", nullable: false),
                    HireDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Occurences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduleStart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: true),
                    Attendees = table.Column<string>(type: "TEXT", nullable: false),
                    Leaders = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occurences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Occurences_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CustomerId", "Description", "Duration", "EmployeeId", "EventName", "EventType" },
                values: new object[,]
                {
                    { 1, null, "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.", 7200000, null, "Advanced Pottery", 2 },
                    { 2, null, "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.", 10800000, null, "Woodworking Workshop", 1 },
                    { 3, null, "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.", 5400000, null, "Sewing Basics", 3 },
                    { 4, null, "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.", 9000000, null, "Pottery for Beginners", 2 },
                    { 5, null, "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.", 14400000, null, "Advanced Woodworking", 1 }
                });

            migrationBuilder.InsertData(
                table: "Occurences",
                columns: new[] { "Id", "Attendees", "Duration", "EventId", "Leaders", "ScheduleStart" },
                values: new object[,]
                {
                    { 1, "[]", 90, 1, "[]", new DateTime(2025, 7, 19, 22, 48, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 2, "[]", 105, 1, "[]", new DateTime(2025, 7, 23, 2, 6, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 3, "[]", 120, 1, "[]", new DateTime(2025, 8, 22, 23, 58, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 4, "[]", 90, 1, "[]", new DateTime(2025, 7, 29, 1, 58, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 5, "[]", 60, 1, "[]", new DateTime(2025, 8, 5, 4, 56, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 6, "[]", 120, 1, "[]", new DateTime(2025, 8, 22, 20, 38, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 7, "[]", 30, 2, "[]", new DateTime(2025, 8, 11, 23, 27, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 8, "[]", 90, 2, "[]", new DateTime(2025, 9, 9, 15, 23, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 9, "[]", 45, 2, "[]", new DateTime(2025, 7, 11, 12, 52, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 10, "[]", 90, 2, "[]", new DateTime(2025, 9, 27, 12, 25, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 11, "[]", 60, 2, "[]", new DateTime(2025, 7, 23, 20, 47, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 12, "[]", 90, 3, "[]", new DateTime(2025, 9, 18, 19, 53, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 13, "[]", 75, 3, "[]", new DateTime(2025, 9, 18, 13, 45, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 14, "[]", 30, 3, "[]", new DateTime(2025, 7, 20, 20, 25, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 15, "[]", 30, 3, "[]", new DateTime(2025, 9, 16, 10, 50, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 16, "[]", 60, 3, "[]", new DateTime(2025, 7, 30, 14, 20, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 17, "[]", 90, 4, "[]", new DateTime(2025, 7, 19, 20, 52, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 18, "[]", 75, 4, "[]", new DateTime(2025, 8, 22, 21, 1, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 19, "[]", 45, 4, "[]", new DateTime(2025, 9, 9, 10, 52, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 20, "[]", 60, 4, "[]", new DateTime(2025, 7, 9, 4, 33, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 21, "[]", 30, 4, "[]", new DateTime(2025, 7, 14, 13, 2, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 22, "[]", 120, 4, "[]", new DateTime(2025, 8, 17, 20, 5, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 23, "[]", 30, 5, "[]", new DateTime(2025, 8, 23, 7, 47, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 24, "[]", 30, 5, "[]", new DateTime(2025, 9, 19, 20, 6, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 25, "[]", 30, 5, "[]", new DateTime(2025, 7, 13, 11, 29, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 26, "[]", 75, 5, "[]", new DateTime(2025, 9, 28, 18, 5, 40, 399, DateTimeKind.Utc).AddTicks(4874) },
                    { 27, "[]", 30, 5, "[]", new DateTime(2025, 8, 10, 10, 35, 40, 399, DateTimeKind.Utc).AddTicks(4874) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeNumber",
                table: "Employees",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_CustomerId",
                table: "Events",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EmployeeId",
                table: "Events",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Occurences_EventId",
                table: "Occurences",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Occurences");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
