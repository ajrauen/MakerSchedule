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
                columns: new[] { "Id", "Description", "Duration", "EventName", "EventType", "FileUrl" },
                values: new object[,]
                {
                    { new Guid("13f8db47-ef70-4745-b4c2-f44406731600"), "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.", 7200000, "Advanced Pottery", 2, null },
                    { new Guid("261de8f4-12a5-4020-8d7d-87605433e4ec"), "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.", 10800000, "Woodworking Workshop", 1, null },
                    { new Guid("3c6b7ef0-0ee2-4bf7-9833-dfe3ea581fde"), "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.", 9000000, "Pottery for Beginners", 2, null },
                    { new Guid("414e4b0c-c69c-4c33-b4dc-ed851f7ca152"), "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.", 14400000, "Advanced Woodworking", 1, null },
                    { new Guid("c0de217f-e23f-4d3b-ade8-bc939ce5c32d"), "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.", 5400000, "Sewing Basics", 3, null }
                });

            migrationBuilder.InsertData(
                table: "Occurrences",
                columns: new[] { "Id", "Duration", "EventId", "ScheduleStart", "Status", "isDeleted" },
                values: new object[,]
                {
                    { new Guid("18289e74-b887-43d8-9f0d-14b9cc41b7d0"), 3600000, new Guid("c0de217f-e23f-4d3b-ade8-bc939ce5c32d"), new DateTime(2025, 7, 18, 16, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("1aea4c6b-7f52-4385-a9f5-37049b747c2b"), 1800000, new Guid("c0de217f-e23f-4d3b-ade8-bc939ce5c32d"), new DateTime(2025, 7, 11, 14, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("2026d755-886e-414f-8aac-dc7af1f0469d"), 7200000, new Guid("261de8f4-12a5-4020-8d7d-87605433e4ec"), new DateTime(2025, 7, 17, 14, 0, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("2245a080-1de3-4761-a8f0-86a051828a27"), 1800000, new Guid("414e4b0c-c69c-4c33-b4dc-ed851f7ca152"), new DateTime(2025, 6, 13, 19, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("445136bc-f7ac-47df-baa3-e4535ff15dbe"), 1800000, new Guid("3c6b7ef0-0ee2-4bf7-9833-dfe3ea581fde"), new DateTime(2025, 7, 25, 14, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("4834c06c-72f8-4a0d-941d-897fb692f512"), 4500000, new Guid("13f8db47-ef70-4745-b4c2-f44406731600"), new DateTime(2025, 7, 22, 21, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("4a5abfe4-6bcf-453d-822d-70a17ef8979c"), 1800000, new Guid("261de8f4-12a5-4020-8d7d-87605433e4ec"), new DateTime(2025, 7, 10, 19, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("688d0d68-09ae-4041-9abd-5da4c7aebaa6"), 8100000, new Guid("261de8f4-12a5-4020-8d7d-87605433e4ec"), new DateTime(2025, 7, 22, 20, 0, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("6b4dae87-84a9-4e38-9372-7aa14c25b37b"), 4500000, new Guid("3c6b7ef0-0ee2-4bf7-9833-dfe3ea581fde"), new DateTime(2025, 7, 4, 21, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("6c250541-bfc8-431d-808d-164d9162a2d6"), 4500000, new Guid("261de8f4-12a5-4020-8d7d-87605433e4ec"), new DateTime(2025, 8, 18, 21, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("78b44314-8997-42e3-a2d1-097f42e597b0"), 1800000, new Guid("261de8f4-12a5-4020-8d7d-87605433e4ec"), new DateTime(2025, 8, 25, 18, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("8167e1d8-745f-449b-bda1-3455a9743ec0"), 6300000, new Guid("414e4b0c-c69c-4c33-b4dc-ed851f7ca152"), new DateTime(2025, 9, 2, 22, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("907d9e0c-6cd7-4887-bf5b-1952d3d8b7e6"), 5400000, new Guid("c0de217f-e23f-4d3b-ade8-bc939ce5c32d"), new DateTime(2025, 8, 23, 15, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("91b94ba7-ec3c-4ad8-90d1-df30b3fd63c4"), 7200000, new Guid("13f8db47-ef70-4745-b4c2-f44406731600"), new DateTime(2025, 7, 31, 14, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("96b392f3-2193-496c-ba76-25e64ae37785"), 4500000, new Guid("c0de217f-e23f-4d3b-ade8-bc939ce5c32d"), new DateTime(2025, 8, 14, 14, 45, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("9b04a499-a273-4e90-a016-7f85dabb747e"), 2700000, new Guid("13f8db47-ef70-4745-b4c2-f44406731600"), new DateTime(2025, 7, 17, 15, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("aa93ddc8-f002-480a-b59d-6ac0b65aa5e9"), 5400000, new Guid("13f8db47-ef70-4745-b4c2-f44406731600"), new DateTime(2025, 8, 8, 17, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("aa9473f1-1c53-40e6-bb29-a1ae02cd8496"), 2700000, new Guid("414e4b0c-c69c-4c33-b4dc-ed851f7ca152"), new DateTime(2025, 8, 28, 15, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("b6244803-500d-4b91-a1ee-f0e6d043cc8d"), 3600000, new Guid("414e4b0c-c69c-4c33-b4dc-ed851f7ca152"), new DateTime(2025, 7, 16, 14, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("be1e1f38-c656-43e0-a541-d1b1c68f61df"), 3600000, new Guid("c0de217f-e23f-4d3b-ade8-bc939ce5c32d"), new DateTime(2025, 8, 22, 15, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("c87fd5e2-1bf5-44e7-9aba-cf16c7cacf83"), 2700000, new Guid("13f8db47-ef70-4745-b4c2-f44406731600"), new DateTime(2025, 8, 5, 20, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("e19b166d-59dc-4b91-8b13-913d575a19fb"), 1800000, new Guid("3c6b7ef0-0ee2-4bf7-9833-dfe3ea581fde"), new DateTime(2025, 7, 1, 14, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("e51e1fca-0c5d-439b-a510-95b4fb9ebfe2"), 2700000, new Guid("3c6b7ef0-0ee2-4bf7-9833-dfe3ea581fde"), new DateTime(2025, 8, 21, 14, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("f12bf7c7-d925-4182-96e7-1aca29f3c6a1"), 5400000, new Guid("13f8db47-ef70-4745-b4c2-f44406731600"), new DateTime(2025, 6, 19, 16, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("f1ba91f6-adea-4ba1-9f35-82bddda1edfa"), 4500000, new Guid("3c6b7ef0-0ee2-4bf7-9833-dfe3ea581fde"), new DateTime(2025, 6, 12, 22, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("ff1cd40e-f00e-4451-a7e4-390666f7730d"), 6300000, new Guid("c0de217f-e23f-4d3b-ade8-bc939ce5c32d"), new DateTime(2025, 6, 30, 14, 15, 0, 0, DateTimeKind.Utc), 2, false }
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
