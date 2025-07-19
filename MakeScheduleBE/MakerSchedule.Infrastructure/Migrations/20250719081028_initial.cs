using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MakerSchedule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "DomainUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreferredContactMethod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DomainUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Occurrences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occurrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Occurrences_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OccurrenceAttendees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccurrenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Attended = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OccurrenceAttendees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OccurrenceAttendees_DomainUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "DomainUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OccurrenceAttendees_Occurrences_OccurrenceId",
                        column: x => x.OccurrenceId,
                        principalTable: "Occurrences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OccurrenceLeaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccurrenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OccurrenceLeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OccurrenceLeaders_DomainUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "DomainUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OccurrenceLeaders_Occurrences_OccurrenceId",
                        column: x => x.OccurrenceId,
                        principalTable: "Occurrences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "Duration", "EventName", "EventType", "FileUrl" },
                values: new object[,]
                {
                    { new Guid("05ef5f52-ed35-48af-aee0-6efef900f7ef"), "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.", 14400000, "Advanced Woodworking", 1, null },
                    { new Guid("615345e3-ce61-4da1-81b2-2243592191f4"), "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.", 7200000, "Advanced Pottery", 2, null },
                    { new Guid("88f265de-8e86-4773-baed-99c889cce9f1"), "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.", 5400000, "Sewing Basics", 3, null },
                    { new Guid("a2d69932-4c0c-449b-a980-26b96477c8c8"), "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.", 10800000, "Woodworking Workshop", 1, null },
                    { new Guid("ea9714cc-0041-4a94-b921-6ecdae429e4d"), "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.", 9000000, "Pottery for Beginners", 2, null }
                });

            migrationBuilder.InsertData(
                table: "Occurrences",
                columns: new[] { "Id", "Duration", "EventId", "ScheduleStart" },
                values: new object[,]
                {
                    { new Guid("007c911d-bd56-449c-bfb2-01c9bdb5c205"), 3600000, new Guid("615345e3-ce61-4da1-81b2-2243592191f4"), new DateTime(2025, 8, 16, 17, 18, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("01a03596-0639-4c42-bd57-139e02e41eb0"), 4500000, new Guid("ea9714cc-0041-4a94-b921-6ecdae429e4d"), new DateTime(2025, 9, 3, 9, 23, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("07d110f4-bdaa-4f32-93fa-f70dc6db14dd"), 1800000, new Guid("a2d69932-4c0c-449b-a980-26b96477c8c8"), new DateTime(2025, 8, 23, 11, 49, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("0edae221-7b7e-4deb-ae5c-3541976e170f"), 2700000, new Guid("ea9714cc-0041-4a94-b921-6ecdae429e4d"), new DateTime(2025, 9, 20, 23, 14, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("128807eb-1d8f-4308-b359-0e4d7a18d0f1"), 1800000, new Guid("88f265de-8e86-4773-baed-99c889cce9f1"), new DateTime(2025, 9, 27, 23, 12, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("1d3ad88c-4e0c-44f7-a20c-07810c282a74"), 3600000, new Guid("88f265de-8e86-4773-baed-99c889cce9f1"), new DateTime(2025, 8, 11, 2, 42, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("2601b6e3-d331-4383-850f-ac2dc141930f"), 1800000, new Guid("05ef5f52-ed35-48af-aee0-6efef900f7ef"), new DateTime(2025, 10, 1, 8, 28, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("4cbe4226-e0df-4606-9626-b481abdb2682"), 7200000, new Guid("615345e3-ce61-4da1-81b2-2243592191f4"), new DateTime(2025, 9, 3, 9, 0, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("50c0e036-8c56-4e8a-a46b-44d96183d0a0"), 3600000, new Guid("ea9714cc-0041-4a94-b921-6ecdae429e4d"), new DateTime(2025, 7, 20, 16, 55, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("6043c615-1ad6-466c-bf38-82b7a9bb4a89"), 2700000, new Guid("a2d69932-4c0c-449b-a980-26b96477c8c8"), new DateTime(2025, 7, 23, 1, 14, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("66838d86-ef53-4063-89dc-5848b2b2c721"), 1800000, new Guid("88f265de-8e86-4773-baed-99c889cce9f1"), new DateTime(2025, 8, 1, 8, 47, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("67a16db7-361a-40d0-bb3c-d3a1d12b99c4"), 6300000, new Guid("615345e3-ce61-4da1-81b2-2243592191f4"), new DateTime(2025, 8, 3, 14, 28, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("7350ef0c-76e4-47d4-a657-f45762c03d26"), 5400000, new Guid("615345e3-ce61-4da1-81b2-2243592191f4"), new DateTime(2025, 8, 9, 14, 20, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("81d1ee40-38d0-40b1-b16f-ca50ef1df167"), 5400000, new Guid("a2d69932-4c0c-449b-a980-26b96477c8c8"), new DateTime(2025, 10, 9, 0, 47, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("9247bfbd-06d3-46aa-b8e6-495d25b771dc"), 1800000, new Guid("05ef5f52-ed35-48af-aee0-6efef900f7ef"), new DateTime(2025, 9, 3, 20, 9, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("990b47cd-4adc-4e36-8402-d24a430964fa"), 5400000, new Guid("ea9714cc-0041-4a94-b921-6ecdae429e4d"), new DateTime(2025, 7, 31, 9, 14, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("a32140e1-73bd-4013-947e-9b052a26dcef"), 1800000, new Guid("05ef5f52-ed35-48af-aee0-6efef900f7ef"), new DateTime(2025, 8, 21, 22, 57, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("ad5ba3b8-207a-4cd4-a74c-0f16ba26ad03"), 5400000, new Guid("615345e3-ce61-4da1-81b2-2243592191f4"), new DateTime(2025, 7, 31, 11, 10, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("b20d4d3c-d4b7-454b-be66-a93614f057dd"), 3600000, new Guid("a2d69932-4c0c-449b-a980-26b96477c8c8"), new DateTime(2025, 8, 4, 9, 9, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("c26ea429-2fb0-4e3f-9ba0-99144e73616c"), 1800000, new Guid("ea9714cc-0041-4a94-b921-6ecdae429e4d"), new DateTime(2025, 7, 26, 1, 24, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("c689ae02-9548-4c92-b51b-fcf23ff1c917"), 7200000, new Guid("ea9714cc-0041-4a94-b921-6ecdae429e4d"), new DateTime(2025, 8, 29, 8, 27, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("c844164a-002c-40fd-92cb-4aa2d02cbd6d"), 7200000, new Guid("615345e3-ce61-4da1-81b2-2243592191f4"), new DateTime(2025, 9, 3, 12, 20, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("ce4737cf-35cd-4fea-89de-194f78998117"), 4500000, new Guid("88f265de-8e86-4773-baed-99c889cce9f1"), new DateTime(2025, 9, 30, 2, 7, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("d667d1ab-c56e-4087-9488-124f79864c5d"), 5400000, new Guid("88f265de-8e86-4773-baed-99c889cce9f1"), new DateTime(2025, 9, 30, 8, 15, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("de47c484-a620-48de-a5a9-416f54e57962"), 5400000, new Guid("a2d69932-4c0c-449b-a980-26b96477c8c8"), new DateTime(2025, 9, 21, 3, 45, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("f183ca09-ac39-4b23-9842-6822b269b486"), 4500000, new Guid("05ef5f52-ed35-48af-aee0-6efef900f7ef"), new DateTime(2025, 10, 10, 6, 27, 28, 257, DateTimeKind.Utc).AddTicks(3912) },
                    { new Guid("f607acfe-ff00-4dfd-b254-9a8b3a8d68fd"), 1800000, new Guid("05ef5f52-ed35-48af-aee0-6efef900f7ef"), new DateTime(2025, 7, 24, 23, 51, 28, 257, DateTimeKind.Utc).AddTicks(3912) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DomainUsers_UserId",
                table: "DomainUsers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OccurrenceAttendees_OccurrenceId",
                table: "OccurrenceAttendees",
                column: "OccurrenceId");

            migrationBuilder.CreateIndex(
                name: "IX_OccurrenceAttendees_UserId",
                table: "OccurrenceAttendees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OccurrenceLeaders_OccurrenceId",
                table: "OccurrenceLeaders",
                column: "OccurrenceId");

            migrationBuilder.CreateIndex(
                name: "IX_OccurrenceLeaders_UserId",
                table: "OccurrenceLeaders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Occurrences_EventId",
                table: "Occurrences",
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
                name: "OccurrenceAttendees");

            migrationBuilder.DropTable(
                name: "OccurrenceLeaders");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DomainUsers");

            migrationBuilder.DropTable(
                name: "Occurrences");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
