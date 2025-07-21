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
                    { new Guid("225a9624-236b-4071-851c-c10fc8e3a629"), "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.", 9000000, "Pottery for Beginners", 2, null },
                    { new Guid("2ca1705c-0372-4d67-996e-aff3baed0107"), "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.", 5400000, "Sewing Basics", 3, null },
                    { new Guid("37a28e3f-74a3-4105-a051-38f7fdd8448d"), "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.", 7200000, "Advanced Pottery", 2, null },
                    { new Guid("5ad354a0-2e4c-4fea-add3-70db98525b97"), "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.", 10800000, "Woodworking Workshop", 1, null },
                    { new Guid("a9ad731c-ad36-424c-a21a-4ac491269150"), "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.", 14400000, "Advanced Woodworking", 1, null }
                });

            migrationBuilder.InsertData(
                table: "Occurrences",
                columns: new[] { "Id", "Duration", "EventId", "ScheduleStart", "Status" },
                values: new object[,]
                {
                    { new Guid("1b6612fd-a8ae-46bc-ad5e-419bb7ed3268"), 1800000, new Guid("2ca1705c-0372-4d67-996e-aff3baed0107"), new DateTime(2025, 7, 8, 14, 30, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("1fe05f8b-390b-4a3b-9575-fd4df2584773"), 3600000, new Guid("a9ad731c-ad36-424c-a21a-4ac491269150"), new DateTime(2025, 7, 13, 14, 15, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("3b8bc50f-2778-44e6-9220-116c60ae37d6"), 2700000, new Guid("37a28e3f-74a3-4105-a051-38f7fdd8448d"), new DateTime(2025, 8, 2, 20, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("3c36fa60-75eb-4a2c-8441-caea864d4878"), 6300000, new Guid("2ca1705c-0372-4d67-996e-aff3baed0107"), new DateTime(2025, 6, 27, 14, 15, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("3e7d631b-48f2-4e16-8ed9-470c703673a3"), 4500000, new Guid("225a9624-236b-4071-851c-c10fc8e3a629"), new DateTime(2025, 6, 9, 22, 15, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("500c11b4-5cd5-4ebc-957f-d3e98ee873ca"), 2700000, new Guid("37a28e3f-74a3-4105-a051-38f7fdd8448d"), new DateTime(2025, 7, 14, 15, 30, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("56036d6b-1094-49a8-834e-4e9905ef823d"), 4500000, new Guid("2ca1705c-0372-4d67-996e-aff3baed0107"), new DateTime(2025, 8, 11, 14, 45, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("5bfd5f4c-3349-40e3-886d-b9adc22a4c10"), 5400000, new Guid("2ca1705c-0372-4d67-996e-aff3baed0107"), new DateTime(2025, 8, 20, 15, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("6171e99a-39e8-4f64-bbcd-f543ce2800b3"), 4500000, new Guid("225a9624-236b-4071-851c-c10fc8e3a629"), new DateTime(2025, 7, 1, 21, 30, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("64fa01e0-0958-40e7-b080-8ff95dc1cb2b"), 5400000, new Guid("37a28e3f-74a3-4105-a051-38f7fdd8448d"), new DateTime(2025, 8, 5, 17, 15, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("6d6d74ea-e342-4f1c-8ad2-c207a6197543"), 7200000, new Guid("5ad354a0-2e4c-4fea-add3-70db98525b97"), new DateTime(2025, 7, 14, 14, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("7ac602c6-8b17-402b-990d-bab2af2f6ab7"), 1800000, new Guid("a9ad731c-ad36-424c-a21a-4ac491269150"), new DateTime(2025, 6, 10, 19, 30, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("865f5aad-6bb5-4262-89a7-5f596a7c5b93"), 2700000, new Guid("225a9624-236b-4071-851c-c10fc8e3a629"), new DateTime(2025, 8, 18, 14, 15, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("882ea946-85da-4605-8e45-19e2518b3e92"), 4500000, new Guid("37a28e3f-74a3-4105-a051-38f7fdd8448d"), new DateTime(2025, 7, 19, 21, 30, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("8ce3412a-79ea-4b98-a62c-31e3139b5bf2"), 3600000, new Guid("2ca1705c-0372-4d67-996e-aff3baed0107"), new DateTime(2025, 8, 19, 15, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("8d4976d7-b555-4bc0-ba7b-6ee68c1070ae"), 6300000, new Guid("a9ad731c-ad36-424c-a21a-4ac491269150"), new DateTime(2025, 8, 30, 22, 15, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("91b20611-0186-4220-b83e-47084113fb3e"), 1800000, new Guid("225a9624-236b-4071-851c-c10fc8e3a629"), new DateTime(2025, 6, 28, 14, 45, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("a3619219-1000-453f-a6e0-1e4b1146f9a5"), 7200000, new Guid("37a28e3f-74a3-4105-a051-38f7fdd8448d"), new DateTime(2025, 7, 28, 14, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("a4eca416-a8a4-4094-8c79-070eef2fd4c4"), 1800000, new Guid("225a9624-236b-4071-851c-c10fc8e3a629"), new DateTime(2025, 7, 22, 14, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("b675a134-9a61-4e01-bb9a-2ba309fd8e06"), 3600000, new Guid("2ca1705c-0372-4d67-996e-aff3baed0107"), new DateTime(2025, 7, 15, 16, 45, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("b9588e26-bff4-473e-adca-29ab819f9337"), 1800000, new Guid("5ad354a0-2e4c-4fea-add3-70db98525b97"), new DateTime(2025, 7, 7, 19, 45, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("bac43ac1-d34a-48f3-a920-4e7609bb9de1"), 5400000, new Guid("37a28e3f-74a3-4105-a051-38f7fdd8448d"), new DateTime(2025, 6, 16, 16, 15, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("f176f0ae-1960-4077-b7a8-75282619a218"), 1800000, new Guid("5ad354a0-2e4c-4fea-add3-70db98525b97"), new DateTime(2025, 8, 22, 18, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("f6b53862-8d5e-434c-9d70-9daa16c617fb"), 8100000, new Guid("5ad354a0-2e4c-4fea-add3-70db98525b97"), new DateTime(2025, 7, 19, 20, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { new Guid("f9e5d87f-0034-4289-a2cd-bf9b00034614"), 4500000, new Guid("5ad354a0-2e4c-4fea-add3-70db98525b97"), new DateTime(2025, 8, 15, 21, 30, 0, 0, DateTimeKind.Utc), 1 },
                    { new Guid("fc650e87-0c98-4013-9dae-1c98ffb41693"), 2700000, new Guid("a9ad731c-ad36-424c-a21a-4ac491269150"), new DateTime(2025, 8, 25, 15, 0, 0, 0, DateTimeKind.Utc), 1 }
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
