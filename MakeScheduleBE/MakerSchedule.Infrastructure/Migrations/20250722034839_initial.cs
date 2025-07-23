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
                    { new Guid("12d514a0-5d72-4bb3-847d-f0ed0fa4be41"), "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.", 10800000, "Woodworking Workshop", 1, null },
                    { new Guid("3858b0b3-3c86-4997-918f-f64d9ed28c5c"), "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.", 9000000, "Pottery for Beginners", 2, null },
                    { new Guid("4ff3df7b-a915-4251-b59a-15aba6f5e2c6"), "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.", 14400000, "Advanced Woodworking", 1, null },
                    { new Guid("6358cd0d-8563-42af-9038-3593b31a9a4d"), "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.", 5400000, "Sewing Basics", 3, null },
                    { new Guid("c63111d3-b15e-4ec6-a948-3d787fea51ef"), "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.", 7200000, "Advanced Pottery", 2, null }
                });

            migrationBuilder.InsertData(
                table: "Occurrences",
                columns: new[] { "Id", "Duration", "EventId", "ScheduleStart", "Status", "isDeleted" },
                values: new object[,]
                {
                    { new Guid("029e04d2-fc78-4cac-93a2-cd85cd2eb03a"), 3600000, new Guid("6358cd0d-8563-42af-9038-3593b31a9a4d"), new DateTime(2025, 8, 20, 15, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("11b4dbf4-8281-495a-ab75-0919a22baa73"), 4500000, new Guid("12d514a0-5d72-4bb3-847d-f0ed0fa4be41"), new DateTime(2025, 8, 16, 21, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("227db5d3-e2b6-4734-aeb2-a698ce7e9d38"), 2700000, new Guid("4ff3df7b-a915-4251-b59a-15aba6f5e2c6"), new DateTime(2025, 8, 26, 15, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("2889bf71-2af3-4db6-8fc6-be9db5aced59"), 3600000, new Guid("4ff3df7b-a915-4251-b59a-15aba6f5e2c6"), new DateTime(2025, 7, 14, 14, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("3a49c783-e555-47e7-a5eb-cffe3af35d9b"), 2700000, new Guid("c63111d3-b15e-4ec6-a948-3d787fea51ef"), new DateTime(2025, 7, 15, 15, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("41329243-f3d0-415d-9d6d-e49b81f5743b"), 7200000, new Guid("c63111d3-b15e-4ec6-a948-3d787fea51ef"), new DateTime(2025, 7, 29, 14, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("41559ccf-50ff-4872-85ab-6cd2f9c5f5d4"), 1800000, new Guid("3858b0b3-3c86-4997-918f-f64d9ed28c5c"), new DateTime(2025, 7, 23, 14, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("427fb98d-8273-4e32-a8de-2cbd354f9890"), 5400000, new Guid("c63111d3-b15e-4ec6-a948-3d787fea51ef"), new DateTime(2025, 8, 6, 17, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("5024d2b2-c3eb-4e49-894d-257848c06b2f"), 1800000, new Guid("12d514a0-5d72-4bb3-847d-f0ed0fa4be41"), new DateTime(2025, 8, 23, 18, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("5dc474f9-7212-4a73-98b9-6aa3b9dd033b"), 1800000, new Guid("12d514a0-5d72-4bb3-847d-f0ed0fa4be41"), new DateTime(2025, 7, 8, 19, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("76d5a13f-f5af-41f0-904f-be8aabac65c0"), 6300000, new Guid("4ff3df7b-a915-4251-b59a-15aba6f5e2c6"), new DateTime(2025, 8, 31, 22, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("7b9ca3c1-132d-4b37-8db7-94a8de1edfc2"), 5400000, new Guid("6358cd0d-8563-42af-9038-3593b31a9a4d"), new DateTime(2025, 8, 21, 15, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("7f393041-fec9-400f-b105-2eef5344de8a"), 4500000, new Guid("3858b0b3-3c86-4997-918f-f64d9ed28c5c"), new DateTime(2025, 6, 10, 22, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("8a21094b-4e23-4d0e-8e08-01c808d1ccd2"), 7200000, new Guid("12d514a0-5d72-4bb3-847d-f0ed0fa4be41"), new DateTime(2025, 7, 15, 14, 0, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("9130ecd9-4d5a-46be-a081-989ebfee55b4"), 2700000, new Guid("3858b0b3-3c86-4997-918f-f64d9ed28c5c"), new DateTime(2025, 8, 19, 14, 15, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("92f37068-53db-43c7-8246-8f4514585e8a"), 8100000, new Guid("12d514a0-5d72-4bb3-847d-f0ed0fa4be41"), new DateTime(2025, 7, 20, 20, 0, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("a2b88ced-43b0-4bc5-90db-5674ae148a97"), 5400000, new Guid("c63111d3-b15e-4ec6-a948-3d787fea51ef"), new DateTime(2025, 6, 17, 16, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("a6b74ed4-b6ab-479d-928f-e15cf67bbcba"), 1800000, new Guid("4ff3df7b-a915-4251-b59a-15aba6f5e2c6"), new DateTime(2025, 6, 11, 19, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("ba905b86-cb7c-41e1-b3d9-abfad1fad731"), 4500000, new Guid("c63111d3-b15e-4ec6-a948-3d787fea51ef"), new DateTime(2025, 7, 20, 21, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("bc8774c2-e20d-4e67-91a0-2dd256365039"), 2700000, new Guid("c63111d3-b15e-4ec6-a948-3d787fea51ef"), new DateTime(2025, 8, 3, 20, 30, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("c323cae3-b9ef-4951-9511-fb2b20314602"), 3600000, new Guid("6358cd0d-8563-42af-9038-3593b31a9a4d"), new DateTime(2025, 7, 16, 16, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("d45f6fc9-2a82-4a85-beeb-42883be70b70"), 6300000, new Guid("6358cd0d-8563-42af-9038-3593b31a9a4d"), new DateTime(2025, 6, 28, 14, 15, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("da35f9eb-bcde-4a8c-8673-21135fe619ab"), 4500000, new Guid("3858b0b3-3c86-4997-918f-f64d9ed28c5c"), new DateTime(2025, 7, 2, 21, 30, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("ef6631f7-b473-47d5-84e3-de06c3c1b17d"), 4500000, new Guid("6358cd0d-8563-42af-9038-3593b31a9a4d"), new DateTime(2025, 8, 12, 14, 45, 0, 0, DateTimeKind.Utc), 1, false },
                    { new Guid("f0bd3fb6-8ea8-4cb2-a50d-8e1d82ab312e"), 1800000, new Guid("3858b0b3-3c86-4997-918f-f64d9ed28c5c"), new DateTime(2025, 6, 29, 14, 45, 0, 0, DateTimeKind.Utc), 2, false },
                    { new Guid("fa0640c4-650e-447c-bf5c-82bfb3d1d6d6"), 1800000, new Guid("6358cd0d-8563-42af-9038-3593b31a9a4d"), new DateTime(2025, 7, 9, 14, 30, 0, 0, DateTimeKind.Utc), 2, false }
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
