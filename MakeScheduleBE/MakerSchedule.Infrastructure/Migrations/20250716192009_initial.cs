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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OccurrenceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OccurrenceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    { "224de1c5-f778-4118-b64f-dbea23c9a0d4", "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.", 7200000, "Advanced Pottery", 2, null },
                    { "56ec6462-3307-4567-b97b-4a0a7aed5c28", "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.", 5400000, "Sewing Basics", 3, null },
                    { "9f5a5299-4cba-4eed-a2c5-0f9f6e4ae43a", "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.", 14400000, "Advanced Woodworking", 1, null },
                    { "d2272a69-1840-46da-815f-3a92262126e5", "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.", 10800000, "Woodworking Workshop", 1, null },
                    { "eb0a36c9-eef2-4e3d-9b51-b05b0aa4fdc1", "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.", 9000000, "Pottery for Beginners", 2, null }
                });

            migrationBuilder.InsertData(
                table: "Occurrences",
                columns: new[] { "Id", "Duration", "EventId", "ScheduleStart" },
                values: new object[,]
                {
                    { "0078edbb-ebbb-43f7-88bc-7a33ab8b820c", 60, "56ec6462-3307-4567-b97b-4a0a7aed5c28", new DateTime(2025, 8, 8, 13, 52, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "061acfe8-b179-4335-b6c6-327f63eba764", 90, "224de1c5-f778-4118-b64f-dbea23c9a0d4", new DateTime(2025, 7, 28, 22, 20, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "1c7cadd9-2024-46b8-8a18-dc79d761a474", 30, "9f5a5299-4cba-4eed-a2c5-0f9f6e4ae43a", new DateTime(2025, 8, 19, 10, 7, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "20530845-399b-486b-8671-fcc2d77e4861", 120, "224de1c5-f778-4118-b64f-dbea23c9a0d4", new DateTime(2025, 8, 31, 20, 10, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "30dc2bdf-a4db-4982-91cf-763e7a5c46f2", 105, "224de1c5-f778-4118-b64f-dbea23c9a0d4", new DateTime(2025, 8, 1, 1, 38, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "37f3e849-0366-4b3a-9154-c70eacc6d2dd", 45, "eb0a36c9-eef2-4e3d-9b51-b05b0aa4fdc1", new DateTime(2025, 9, 18, 10, 24, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "600aae46-934d-4363-a437-0aa68b18f28e", 90, "224de1c5-f778-4118-b64f-dbea23c9a0d4", new DateTime(2025, 8, 7, 1, 30, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "6edb31bf-dd31-4da8-8d70-e0b36dc84893", 75, "56ec6462-3307-4567-b97b-4a0a7aed5c28", new DateTime(2025, 9, 27, 13, 17, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "709f8e01-2e25-4792-a9a5-96fa6cdcd79c", 60, "d2272a69-1840-46da-815f-3a92262126e5", new DateTime(2025, 8, 1, 20, 19, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "78572e4e-cd51-40ef-9ef3-62758522118a", 60, "eb0a36c9-eef2-4e3d-9b51-b05b0aa4fdc1", new DateTime(2025, 7, 18, 4, 5, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "802d78f8-6204-4be5-870f-f347706b9bcc", 30, "9f5a5299-4cba-4eed-a2c5-0f9f6e4ae43a", new DateTime(2025, 9, 28, 19, 38, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "858fb191-1949-4af9-9bda-31674ab0c459", 30, "9f5a5299-4cba-4eed-a2c5-0f9f6e4ae43a", new DateTime(2025, 9, 1, 7, 19, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "89f09741-819b-43b6-8446-a0d2051c7a0f", 90, "d2272a69-1840-46da-815f-3a92262126e5", new DateTime(2025, 9, 18, 14, 55, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "91ff7406-c301-4172-8328-b5389c19d550", 30, "d2272a69-1840-46da-815f-3a92262126e5", new DateTime(2025, 8, 20, 22, 59, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "9a937e3b-843c-4e43-bc91-da10085cfe57", 75, "9f5a5299-4cba-4eed-a2c5-0f9f6e4ae43a", new DateTime(2025, 10, 7, 17, 37, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "af1f5243-faa2-4561-a033-08c5f1e7250e", 60, "224de1c5-f778-4118-b64f-dbea23c9a0d4", new DateTime(2025, 8, 14, 4, 28, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "b79874bb-9997-456b-bc68-02f29e1f040e", 90, "56ec6462-3307-4567-b97b-4a0a7aed5c28", new DateTime(2025, 9, 27, 19, 25, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "cc0ba8f8-8c0a-45b2-82ea-4932fcb9e8f1", 30, "56ec6462-3307-4567-b97b-4a0a7aed5c28", new DateTime(2025, 9, 25, 10, 22, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "cca72055-03e8-4ce6-81e7-efe5a14e1e99", 45, "d2272a69-1840-46da-815f-3a92262126e5", new DateTime(2025, 7, 20, 12, 24, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "cca7940d-19c0-4ec0-b2da-c4bb1a1f2f57", 120, "224de1c5-f778-4118-b64f-dbea23c9a0d4", new DateTime(2025, 8, 31, 23, 30, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "cfa58e0c-8af7-495a-bc4c-aa645b64fe79", 90, "eb0a36c9-eef2-4e3d-9b51-b05b0aa4fdc1", new DateTime(2025, 7, 28, 20, 24, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "d7182758-e516-4375-9407-7103a80667c9", 120, "eb0a36c9-eef2-4e3d-9b51-b05b0aa4fdc1", new DateTime(2025, 8, 26, 19, 37, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "d843e224-3f58-48f5-a5ec-a26e1b2b25ca", 75, "eb0a36c9-eef2-4e3d-9b51-b05b0aa4fdc1", new DateTime(2025, 8, 31, 20, 33, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "f4782594-153a-4319-b33d-f3594f1d3779", 30, "56ec6462-3307-4567-b97b-4a0a7aed5c28", new DateTime(2025, 7, 29, 19, 57, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "f5e61020-8b26-4888-bf6e-fffe218f3486", 30, "eb0a36c9-eef2-4e3d-9b51-b05b0aa4fdc1", new DateTime(2025, 7, 23, 12, 34, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "f89d5870-abe9-4fd8-918b-44efe8af8ee4", 90, "d2272a69-1840-46da-815f-3a92262126e5", new DateTime(2025, 10, 6, 11, 57, 9, 551, DateTimeKind.Utc).AddTicks(4941) },
                    { "fe7946b4-2537-48fb-9196-4a4af2665fa2", 30, "9f5a5299-4cba-4eed-a2c5-0f9f6e4ae43a", new DateTime(2025, 7, 22, 11, 1, 9, 551, DateTimeKind.Utc).AddTicks(4941) }
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
