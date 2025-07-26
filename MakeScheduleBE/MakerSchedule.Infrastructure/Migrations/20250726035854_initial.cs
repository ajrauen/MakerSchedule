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
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    PreferredContactMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                columns: new[] { "Id", "Description", "Duration", "EventName", "EventType", "ThumbnailUrl" },
                values: new object[,]
                {
                    { new Guid("4de62144-c9e4-4cbd-b7a0-638c61445fbf"), "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.", 10800000, "Woodworking Workshop", 1, null },
                    { new Guid("654166ec-e9ad-48fd-bc35-ab1cab944fb1"), "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.", 5400000, "Sewing Basics", 3, null },
                    { new Guid("6b7f9971-c0ae-47d7-b3c8-fc006fd483b8"), "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.", 9000000, "Pottery for Beginners", 2, null },
                    { new Guid("c823d101-fe7a-4d6e-91c5-ef9aa43525f6"), "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.", 7200000, "Advanced Pottery", 2, null },
                    { new Guid("daf1b716-3265-43f3-9421-77603d1d7979"), "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.", 14400000, "Advanced Woodworking", 1, null }
                });

            migrationBuilder.InsertData(
                table: "Occurrences",
                columns: new[] { "Id", "Duration", "EventId", "ScheduleStart", "Status", "isDeleted" },
                values: new object[,]
                {
                    { new Guid("0cfdccfe-3cd2-43b7-b9db-0c2163b723c6"), 7200000, new Guid("c823d101-fe7a-4d6e-91c5-ef9aa43525f6"), new DateTime(2025, 8, 2, 14, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("0f44907b-61f2-45b4-a9e8-3774d40d5386"), 5400000, new Guid("654166ec-e9ad-48fd-bc35-ab1cab944fb1"), new DateTime(2025, 8, 25, 15, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("1eb7980c-73cc-4910-9c2f-afc389560463"), 3600000, new Guid("daf1b716-3265-43f3-9421-77603d1d7979"), new DateTime(2025, 7, 18, 14, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("21c37e4b-9275-434d-b354-c0ada97b4378"), 4500000, new Guid("4de62144-c9e4-4cbd-b7a0-638c61445fbf"), new DateTime(2025, 8, 20, 21, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("24253fd6-f29f-4fe3-9e76-a70f8a6ec4b3"), 1800000, new Guid("daf1b716-3265-43f3-9421-77603d1d7979"), new DateTime(2025, 6, 15, 19, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("25284893-03e9-4010-83c1-15019800c0fb"), 4500000, new Guid("654166ec-e9ad-48fd-bc35-ab1cab944fb1"), new DateTime(2025, 8, 16, 14, 45, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("38080e9e-f99c-41ea-aa90-a64865298170"), 6300000, new Guid("daf1b716-3265-43f3-9421-77603d1d7979"), new DateTime(2025, 9, 4, 22, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("3c08b46a-41b4-4deb-8551-d1eefe2a2c6c"), 1800000, new Guid("654166ec-e9ad-48fd-bc35-ab1cab944fb1"), new DateTime(2025, 7, 13, 14, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("44737c64-3e22-4c5f-9e3a-17835f6432e3"), 4500000, new Guid("c823d101-fe7a-4d6e-91c5-ef9aa43525f6"), new DateTime(2025, 7, 24, 21, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("44d896f4-73bd-4486-865a-d3ade01c098f"), 7200000, new Guid("4de62144-c9e4-4cbd-b7a0-638c61445fbf"), new DateTime(2025, 7, 19, 14, 0, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("45e69610-caef-4321-a7fc-523477e575dd"), 2700000, new Guid("c823d101-fe7a-4d6e-91c5-ef9aa43525f6"), new DateTime(2025, 8, 7, 20, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("48282f58-6e98-4b65-9e50-e031b01d2be1"), 6300000, new Guid("654166ec-e9ad-48fd-bc35-ab1cab944fb1"), new DateTime(2025, 7, 2, 14, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("63a014bc-f301-4754-988c-bfb49deacbc1"), 1800000, new Guid("4de62144-c9e4-4cbd-b7a0-638c61445fbf"), new DateTime(2025, 8, 27, 18, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("6a0f0d2f-c991-426f-ad2f-49a44f844839"), 3600000, new Guid("654166ec-e9ad-48fd-bc35-ab1cab944fb1"), new DateTime(2025, 8, 24, 15, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("6db35c17-911e-4e38-92da-02b3eafc8112"), 5400000, new Guid("c823d101-fe7a-4d6e-91c5-ef9aa43525f6"), new DateTime(2025, 8, 10, 17, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("6eff5de1-9953-4fbb-adb3-68fcffebc412"), 4500000, new Guid("6b7f9971-c0ae-47d7-b3c8-fc006fd483b8"), new DateTime(2025, 7, 6, 21, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("766e25fb-2c2a-4a20-82c7-18ddb6d4b361"), 3600000, new Guid("654166ec-e9ad-48fd-bc35-ab1cab944fb1"), new DateTime(2025, 7, 20, 16, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("7dce3256-8ca3-4061-8af2-5486d29af6b6"), 2700000, new Guid("c823d101-fe7a-4d6e-91c5-ef9aa43525f6"), new DateTime(2025, 7, 19, 15, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("80c92df5-9aad-436a-968c-eab3b49edb71"), 4500000, new Guid("6b7f9971-c0ae-47d7-b3c8-fc006fd483b8"), new DateTime(2025, 6, 14, 22, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("8fa18c27-155b-49db-acfb-7ac0ec2edb40"), 1800000, new Guid("4de62144-c9e4-4cbd-b7a0-638c61445fbf"), new DateTime(2025, 7, 12, 19, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("ad478183-38ac-4e35-8037-27b20b8fe935"), 2700000, new Guid("daf1b716-3265-43f3-9421-77603d1d7979"), new DateTime(2025, 8, 30, 15, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("c42e1d15-7afc-42e3-8de1-1c6219360d91"), 8100000, new Guid("4de62144-c9e4-4cbd-b7a0-638c61445fbf"), new DateTime(2025, 7, 24, 20, 0, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("ca23750e-ff33-46a3-bd74-caa90f12edb3"), 5400000, new Guid("c823d101-fe7a-4d6e-91c5-ef9aa43525f6"), new DateTime(2025, 6, 21, 16, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("d7d40d46-805f-4933-a6a0-78bcea932ab7"), 1800000, new Guid("6b7f9971-c0ae-47d7-b3c8-fc006fd483b8"), new DateTime(2025, 7, 3, 14, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("db80f09e-dc3e-4a32-9131-f9c984e140ea"), 2700000, new Guid("6b7f9971-c0ae-47d7-b3c8-fc006fd483b8"), new DateTime(2025, 8, 23, 14, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("dbb8cf99-f6f6-4463-8b65-c41c346a90b8"), 1800000, new Guid("6b7f9971-c0ae-47d7-b3c8-fc006fd483b8"), new DateTime(2025, 7, 27, 14, 30, 0, 0, DateTimeKind.Utc), 1, false }
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
