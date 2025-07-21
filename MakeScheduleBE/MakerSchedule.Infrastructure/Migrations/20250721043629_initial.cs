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
                    Duration = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
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
                    { new Guid("4b6daacb-2ab6-4c46-b262-d156a264337e"), "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.", 5400000, "Sewing Basics", 3, null },
                    { new Guid("57c7944c-ff89-4a07-8e8f-2e443d115a2d"), "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.", 9000000, "Pottery for Beginners", 2, null },
                    { new Guid("8cdf6a28-8d6a-4dbe-a2f3-ca1879f62744"), "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.", 14400000, "Advanced Woodworking", 1, null },
                    { new Guid("9f9b9aff-79bb-4837-a5ca-499673f9112c"), "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.", 7200000, "Advanced Pottery", 2, null },
                    { new Guid("c8bcb79a-07bd-40e0-b80e-baa62e8426cb"), "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.", 10800000, "Woodworking Workshop", 1, null }
                });

            migrationBuilder.InsertData(
                table: "Occurrences",
                columns: new[] { "Id", "Duration", "EventId", "ScheduleStart", "Status" },
                values: new object[,]
                {
                    { new Guid("0eff7048-47ff-47b8-86c0-906fbfd2e8db"), 7200000, new Guid("c8bcb79a-07bd-40e0-b80e-baa62e8426cb"), new DateTime(2025, 8, 3, 14, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("23ca075b-de83-4470-887d-c5cb3e3a629b"), 4500000, new Guid("9f9b9aff-79bb-4837-a5ca-499673f9112c"), new DateTime(2025, 7, 24, 21, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("38f53bf9-f182-4134-b516-782f48cbd230"), 5400000, new Guid("9f9b9aff-79bb-4837-a5ca-499673f9112c"), new DateTime(2025, 8, 18, 17, 15, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("3a6779ad-65c2-4e7b-9f6e-87bbdbb5c41c"), 3600000, new Guid("4b6daacb-2ab6-4c46-b262-d156a264337e"), new DateTime(2025, 7, 31, 16, 45, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("3dc96884-de19-4279-8225-85245b20c3d4"), 1800000, new Guid("c8bcb79a-07bd-40e0-b80e-baa62e8426cb"), new DateTime(2025, 9, 21, 18, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("5faa4599-b174-4449-87f0-69f1b9338ce7"), 4500000, new Guid("57c7944c-ff89-4a07-8e8f-2e443d115a2d"), new DateTime(2025, 8, 29, 21, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("71bb75f2-775d-4fac-8e4c-6a3fae77d850"), 6300000, new Guid("8cdf6a28-8d6a-4dbe-a2f3-ca1879f62744"), new DateTime(2025, 10, 8, 22, 15, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("74bdecf6-7eec-4e77-ac42-22179ac61452"), 8100000, new Guid("c8bcb79a-07bd-40e0-b80e-baa62e8426cb"), new DateTime(2025, 7, 24, 20, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("74da77ea-b31a-4341-a79a-15b528367a54"), 4500000, new Guid("4b6daacb-2ab6-4c46-b262-d156a264337e"), new DateTime(2025, 8, 31, 14, 45, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("8f89c275-c273-43d9-bf17-8a8889efe826"), 1800000, new Guid("57c7944c-ff89-4a07-8e8f-2e443d115a2d"), new DateTime(2025, 7, 21, 14, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("99e9235e-db7c-4fc1-afd9-72a6c9caf7f6"), 5400000, new Guid("9f9b9aff-79bb-4837-a5ca-499673f9112c"), new DateTime(2025, 9, 27, 16, 15, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("9c44346f-c0fd-4648-a669-15ef2e8b717d"), 1800000, new Guid("4b6daacb-2ab6-4c46-b262-d156a264337e"), new DateTime(2025, 8, 14, 14, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("9e000ed5-2b41-438c-b447-c400e20f2470"), 4500000, new Guid("c8bcb79a-07bd-40e0-b80e-baa62e8426cb"), new DateTime(2025, 9, 7, 21, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("a03f1d93-6de2-41fa-b561-5802557dfe1c"), 7200000, new Guid("9f9b9aff-79bb-4837-a5ca-499673f9112c"), new DateTime(2025, 8, 3, 14, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("aa69edf9-9ea2-4334-a291-6a8dc14a694a"), 4500000, new Guid("57c7944c-ff89-4a07-8e8f-2e443d115a2d"), new DateTime(2025, 10, 11, 22, 15, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("b5328991-9967-429b-8395-b1a494a5235e"), 2700000, new Guid("9f9b9aff-79bb-4837-a5ca-499673f9112c"), new DateTime(2025, 8, 13, 20, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("bebdaaa5-d30b-4cc9-aad0-0e5a4d0f19e7"), 1800000, new Guid("c8bcb79a-07bd-40e0-b80e-baa62e8426cb"), new DateTime(2025, 8, 17, 19, 45, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("c0832e11-cfb7-4494-8cb0-723a3ba592d9"), 3600000, new Guid("8cdf6a28-8d6a-4dbe-a2f3-ca1879f62744"), new DateTime(2025, 8, 4, 14, 15, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("cba64abd-0be7-4b58-8099-905063115137"), 1800000, new Guid("57c7944c-ff89-4a07-8e8f-2e443d115a2d"), new DateTime(2025, 9, 3, 14, 45, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("cc1be26d-aebc-4ce1-b7e5-e416a1ee6ce3"), 3600000, new Guid("4b6daacb-2ab6-4c46-b262-d156a264337e"), new DateTime(2025, 9, 15, 15, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("d0ef8382-161a-405c-80ab-53e0ffd7f600"), 6300000, new Guid("4b6daacb-2ab6-4c46-b262-d156a264337e"), new DateTime(2025, 9, 5, 14, 15, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("e6c9bf2b-46a1-461d-9d22-f55f6d909674"), 2700000, new Guid("8cdf6a28-8d6a-4dbe-a2f3-ca1879f62744"), new DateTime(2025, 9, 28, 15, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("e769d393-c7d4-4e91-b31a-238a31692aa8"), 2700000, new Guid("57c7944c-ff89-4a07-8e8f-2e443d115a2d"), new DateTime(2025, 9, 14, 14, 15, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("ef63d334-3ae9-4b0a-8645-19181748c248"), 5400000, new Guid("4b6daacb-2ab6-4c46-b262-d156a264337e"), new DateTime(2025, 9, 17, 15, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("ef78a562-5897-4422-9b95-e5d031d7b135"), 1800000, new Guid("8cdf6a28-8d6a-4dbe-a2f3-ca1879f62744"), new DateTime(2025, 10, 10, 19, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("f5d0506e-9cca-4a5e-b579-46aa40695fb0"), 2700000, new Guid("9f9b9aff-79bb-4837-a5ca-499673f9112c"), new DateTime(2025, 8, 2, 15, 30, 0, 0, DateTimeKind.Utc), 1 }
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
