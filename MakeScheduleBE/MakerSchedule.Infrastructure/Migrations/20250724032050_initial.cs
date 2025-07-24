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
                    Status = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                    { new Guid("8aac4c12-3ad3-4da5-be6c-e2d94e16922e"), "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.", 14400000, "Advanced Woodworking", 1, null },
                    { new Guid("9704124c-81be-40d9-a870-7ac2f40eb7ca"), "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.", 9000000, "Pottery for Beginners", 2, null },
                    { new Guid("b6bce105-e723-4da1-bb11-07b04e1b6372"), "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.", 7200000, "Advanced Pottery", 2, null },
                    { new Guid("d860c49c-2fbc-44a2-9cbc-871b44eb5a90"), "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.", 10800000, "Woodworking Workshop", 1, null },
                    { new Guid("fdf36425-d2ad-40dd-8ef2-0051cc103594"), "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.", 5400000, "Sewing Basics", 3, null }
                });

            migrationBuilder.InsertData(
                table: "Occurrences",
                columns: new[] { "Id", "Duration", "EventId", "ScheduleStart", "Status", "isDeleted" },
                values: new object[,]
                {
                    { new Guid("0273883f-3ad2-4c99-bfc1-9cdbfc31cf61"), 6300000, new Guid("fdf36425-d2ad-40dd-8ef2-0051cc103594"), new DateTime(2025, 6, 30, 14, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("22529b56-42ec-458d-9272-9852e0cdbc86"), 4500000, new Guid("9704124c-81be-40d9-a870-7ac2f40eb7ca"), new DateTime(2025, 7, 4, 21, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("26ab78c2-0369-4fd3-aa0c-ff56968c0fee"), 8100000, new Guid("d860c49c-2fbc-44a2-9cbc-871b44eb5a90"), new DateTime(2025, 7, 22, 20, 0, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("3058faa6-d2c9-4d45-b871-d31836092f27"), 1800000, new Guid("9704124c-81be-40d9-a870-7ac2f40eb7ca"), new DateTime(2025, 7, 1, 14, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("386dddb8-7503-417a-b3cf-d5a5cad3402f"), 3600000, new Guid("fdf36425-d2ad-40dd-8ef2-0051cc103594"), new DateTime(2025, 8, 22, 15, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("42820b8c-d41e-49cf-805b-452793cffb71"), 7200000, new Guid("d860c49c-2fbc-44a2-9cbc-871b44eb5a90"), new DateTime(2025, 7, 17, 14, 0, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("477603ea-7882-4895-98cf-abeec53ec3c4"), 2700000, new Guid("9704124c-81be-40d9-a870-7ac2f40eb7ca"), new DateTime(2025, 8, 21, 14, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("60b47b18-cf5e-494b-a647-bb3c90194693"), 4500000, new Guid("9704124c-81be-40d9-a870-7ac2f40eb7ca"), new DateTime(2025, 6, 12, 22, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("66eb32cb-59c3-4650-b8ae-96b14b81a44b"), 3600000, new Guid("8aac4c12-3ad3-4da5-be6c-e2d94e16922e"), new DateTime(2025, 7, 16, 14, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("74c332f3-8498-4b9b-8e52-bc3d21491add"), 1800000, new Guid("d860c49c-2fbc-44a2-9cbc-871b44eb5a90"), new DateTime(2025, 7, 10, 19, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("7a20ba3b-d311-4c7c-ad7b-a9261e2148b4"), 5400000, new Guid("b6bce105-e723-4da1-bb11-07b04e1b6372"), new DateTime(2025, 6, 19, 16, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("7b9125b9-9de4-4965-87be-2d67027f56f9"), 6300000, new Guid("8aac4c12-3ad3-4da5-be6c-e2d94e16922e"), new DateTime(2025, 9, 2, 22, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("81a8876c-b6ae-454b-be50-846223db7729"), 2700000, new Guid("8aac4c12-3ad3-4da5-be6c-e2d94e16922e"), new DateTime(2025, 8, 28, 15, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("95a10070-0b43-4cd8-8665-495adf4fde2a"), 1800000, new Guid("9704124c-81be-40d9-a870-7ac2f40eb7ca"), new DateTime(2025, 7, 25, 14, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("9dd97fa0-7c7d-4718-8121-e7d085c4740f"), 1800000, new Guid("8aac4c12-3ad3-4da5-be6c-e2d94e16922e"), new DateTime(2025, 6, 13, 19, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("9e8477cd-7461-48c1-9e2f-a5ec01b08c06"), 4500000, new Guid("fdf36425-d2ad-40dd-8ef2-0051cc103594"), new DateTime(2025, 8, 14, 14, 45, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("a2ec72ba-f1cd-459e-99a0-092171c86425"), 7200000, new Guid("b6bce105-e723-4da1-bb11-07b04e1b6372"), new DateTime(2025, 7, 31, 14, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("b76947e6-8079-4a72-ac6f-ea43333559ed"), 4500000, new Guid("b6bce105-e723-4da1-bb11-07b04e1b6372"), new DateTime(2025, 7, 22, 21, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("bc251576-2a4d-42b3-a345-d634b409e483"), 1800000, new Guid("fdf36425-d2ad-40dd-8ef2-0051cc103594"), new DateTime(2025, 7, 11, 14, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("c35a644a-28ae-4aaf-82e1-257d9a23ba3a"), 5400000, new Guid("b6bce105-e723-4da1-bb11-07b04e1b6372"), new DateTime(2025, 8, 8, 17, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("d746ca04-4dd0-40cf-97e6-72d63d84336b"), 4500000, new Guid("d860c49c-2fbc-44a2-9cbc-871b44eb5a90"), new DateTime(2025, 8, 18, 21, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("dfef3076-b78e-4de1-aac4-53930bacdf53"), 2700000, new Guid("b6bce105-e723-4da1-bb11-07b04e1b6372"), new DateTime(2025, 7, 17, 15, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("eb37451a-97a8-4beb-8296-83d409e43be8"), 1800000, new Guid("d860c49c-2fbc-44a2-9cbc-871b44eb5a90"), new DateTime(2025, 8, 25, 18, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("f9fb20bb-7313-4bc8-b944-1da64a19a479"), 3600000, new Guid("fdf36425-d2ad-40dd-8ef2-0051cc103594"), new DateTime(2025, 7, 18, 16, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("faab32ac-4c63-4370-8d88-c01e8a993801"), 2700000, new Guid("b6bce105-e723-4da1-bb11-07b04e1b6372"), new DateTime(2025, 8, 5, 20, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("fe4bac14-ec42-4b9b-853f-c41170e1f589"), 5400000, new Guid("fdf36425-d2ad-40dd-8ef2-0051cc103594"), new DateTime(2025, 8, 23, 15, 0, 0, 0, DateTimeKind.Utc), 1, false }
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
