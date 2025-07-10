using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MakerSchedule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFileUrlToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // AspNetRoles
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
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
            }
            else
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
            }

            // AspNetUsers
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
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
                        UserType = table.Column<int>(type: "int", nullable: false),
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
            }
            else
            {
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
            }

            // AspNetRoleClaims
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateTable(
                    name: "AspNetRoleClaims",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
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
            }
            else
            {
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
            }

            // AspNetUserClaims
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateTable(
                    name: "AspNetUserClaims",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
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
            }
            else
            {
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
            }

            // AspNetUserLogins
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
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
            }
            else
            {
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
            }

            // AspNetUserRoles
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
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
            }
            else
            {
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
            }

            // AspNetUserTokens
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
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
            }
            else
            {
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
            }

            // Customers
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateTable(
                    name: "Customers",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        CustomerNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        PreferredContactMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
            }
            else
            {
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
            }

            // Employees
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateTable(
                    name: "Employees",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        EmployeeNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
            }
            else
            {
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
            }

            // Events
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateTable(
                    name: "Events",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        EventType = table.Column<int>(type: "int", nullable: false),
                        Duration = table.Column<int>(type: "int", nullable: false),
                        FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
            }
            else
            {
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
                        FileUrl = table.Column<string>(type: "TEXT", nullable: true),
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
            }

            // Occurrences
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateTable(
                    name: "Occurrences",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        EventId = table.Column<int>(type: "INTEGER", nullable: false),
                        ScheduleStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                        Duration = table.Column<int>(type: "int", nullable: true),
                        Attendees = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Leaders = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
            }
            else
            {
                migrationBuilder.CreateTable(
                    name: "Occurrences",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        EventId = table.Column<int>(type: "INTEGER", nullable: false),
                        ScheduleStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                        Duration = table.Column<int>(type: "INTEGER", nullable: true),
                        Attendees = table.Column<string>(type: "TEXT", nullable: false),
                        Leaders = table.Column<string>(type: "TEXT", nullable: false)
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
            }

            // InsertData for Events
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.InsertData(
                    table: "Events",
                    columns: new[] { "Id", "CustomerId", "Description", "Duration", "EmployeeId", "EventName", "EventType", "FileUrl" },
                    values: new object[,]
                    {
                        { 1, null, "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.", 7200000, null, "Advanced Pottery", 2, null },
                        { 2, null, "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.", 10800000, null, "Woodworking Workshop", 1, null },
                        { 3, null, "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.", 5400000, null, "Sewing Basics", 3, null },
                        { 4, null, "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.", 9000000, null, "Pottery for Beginners", 2, null },
                        { 5, null, "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.", 14400000, null, "Advanced Woodworking", 1, null }
                    });
            }
            else
            {
                migrationBuilder.InsertData(
                    table: "Events",
                    columns: new[] { "Id", "CustomerId", "Description", "Duration", "EmployeeId", "EventName", "EventType", "FileUrl" },
                    values: new object[,]
                    {
                        { 1, null, "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.", 7200000, null, "Advanced Pottery", 2, null },
                        { 2, null, "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.", 10800000, null, "Woodworking Workshop", 1, null },
                        { 3, null, "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.", 5400000, null, "Sewing Basics", 3, null },
                        { 4, null, "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.", 9000000, null, "Pottery for Beginners", 2, null },
                        { 5, null, "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.", 14400000, null, "Advanced Woodworking", 1, null }
                    });
            }

            // InsertData for Occurrences
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.InsertData(
                    table: "Occurrences",
                    columns: new[] { "Id", "Attendees", "Duration", "EventId", "Leaders", "ScheduleStart" },
                    values: new object[,]
                    {
                        { 1, "[]", 90, 1, "[]", new DateTime(2025, 7, 21, 8, 15, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 2, "[]", 105, 1, "[]", new DateTime(2025, 7, 24, 11, 33, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 3, "[]", 120, 1, "[]", new DateTime(2025, 8, 24, 9, 25, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 4, "[]", 90, 1, "[]", new DateTime(2025, 7, 30, 11, 25, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 5, "[]", 60, 1, "[]", new DateTime(2025, 8, 6, 14, 23, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 6, "[]", 120, 1, "[]", new DateTime(2025, 8, 24, 6, 5, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 7, "[]", 30, 2, "[]", new DateTime(2025, 8, 13, 8, 54, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 8, "[]", 90, 2, "[]", new DateTime(2025, 9, 11, 0, 50, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 9, "[]", 45, 2, "[]", new DateTime(2025, 7, 12, 22, 19, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 10, "[]", 90, 2, "[]", new DateTime(2025, 9, 28, 21, 52, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 11, "[]", 60, 2, "[]", new DateTime(2025, 7, 25, 6, 14, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 12, "[]", 90, 3, "[]", new DateTime(2025, 9, 20, 5, 20, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 13, "[]", 75, 3, "[]", new DateTime(2025, 9, 19, 23, 12, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 14, "[]", 30, 3, "[]", new DateTime(2025, 7, 22, 5, 52, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 15, "[]", 30, 3, "[]", new DateTime(2025, 9, 17, 20, 17, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 16, "[]", 60, 3, "[]", new DateTime(2025, 7, 31, 23, 47, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 17, "[]", 90, 4, "[]", new DateTime(2025, 7, 21, 6, 19, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 18, "[]", 75, 4, "[]", new DateTime(2025, 8, 24, 6, 28, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 19, "[]", 45, 4, "[]", new DateTime(2025, 9, 10, 20, 19, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 20, "[]", 60, 4, "[]", new DateTime(2025, 7, 10, 14, 0, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 21, "[]", 30, 4, "[]", new DateTime(2025, 7, 15, 22, 29, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 22, "[]", 120, 4, "[]", new DateTime(2025, 8, 19, 5, 32, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 23, "[]", 30, 5, "[]", new DateTime(2025, 8, 24, 17, 14, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 24, "[]", 30, 5, "[]", new DateTime(2025, 9, 21, 5, 33, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 25, "[]", 30, 5, "[]", new DateTime(2025, 7, 14, 20, 56, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 26, "[]", 75, 5, "[]", new DateTime(2025, 9, 30, 3, 32, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 27, "[]", 30, 5, "[]", new DateTime(2025, 8, 11, 20, 2, 7, 975, DateTimeKind.Utc).AddTicks(1945) }
                    });
            }
            else
            {
                migrationBuilder.InsertData(
                    table: "Occurrences",
                    columns: new[] { "Id", "Attendees", "Duration", "EventId", "Leaders", "ScheduleStart" },
                    values: new object[,]
                    {
                        { 1, "[]", 90, 1, "[]", new DateTime(2025, 7, 21, 8, 15, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 2, "[]", 105, 1, "[]", new DateTime(2025, 7, 24, 11, 33, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 3, "[]", 120, 1, "[]", new DateTime(2025, 8, 24, 9, 25, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 4, "[]", 90, 1, "[]", new DateTime(2025, 7, 30, 11, 25, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 5, "[]", 60, 1, "[]", new DateTime(2025, 8, 6, 14, 23, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 6, "[]", 120, 1, "[]", new DateTime(2025, 8, 24, 6, 5, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 7, "[]", 30, 2, "[]", new DateTime(2025, 8, 13, 8, 54, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 8, "[]", 90, 2, "[]", new DateTime(2025, 9, 11, 0, 50, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 9, "[]", 45, 2, "[]", new DateTime(2025, 7, 12, 22, 19, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 10, "[]", 90, 2, "[]", new DateTime(2025, 9, 28, 21, 52, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 11, "[]", 60, 2, "[]", new DateTime(2025, 7, 25, 6, 14, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 12, "[]", 90, 3, "[]", new DateTime(2025, 9, 20, 5, 20, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 13, "[]", 75, 3, "[]", new DateTime(2025, 9, 19, 23, 12, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 14, "[]", 30, 3, "[]", new DateTime(2025, 7, 22, 5, 52, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 15, "[]", 30, 3, "[]", new DateTime(2025, 9, 17, 20, 17, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 16, "[]", 60, 3, "[]", new DateTime(2025, 7, 31, 23, 47, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 17, "[]", 90, 4, "[]", new DateTime(2025, 7, 21, 6, 19, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 18, "[]", 75, 4, "[]", new DateTime(2025, 8, 24, 6, 28, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 19, "[]", 45, 4, "[]", new DateTime(2025, 9, 10, 20, 19, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 20, "[]", 60, 4, "[]", new DateTime(2025, 7, 10, 14, 0, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 21, "[]", 30, 4, "[]", new DateTime(2025, 7, 15, 22, 29, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 22, "[]", 120, 4, "[]", new DateTime(2025, 8, 19, 5, 32, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 23, "[]", 30, 5, "[]", new DateTime(2025, 8, 24, 17, 14, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 24, "[]", 30, 5, "[]", new DateTime(2025, 9, 21, 5, 33, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 25, "[]", 30, 5, "[]", new DateTime(2025, 7, 14, 20, 56, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 26, "[]", 75, 5, "[]", new DateTime(2025, 9, 30, 3, 32, 7, 975, DateTimeKind.Utc).AddTicks(1945) },
                        { 27, "[]", 30, 5, "[]", new DateTime(2025, 8, 11, 20, 2, 7, 975, DateTimeKind.Utc).AddTicks(1945) }
                    });
            }

            // CreateIndex for AspNetRoleClaims
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AspNetRoleClaims_RoleId",
                    table: "AspNetRoleClaims",
                    column: "RoleId");
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AspNetRoleClaims_RoleId",
                    table: "AspNetRoleClaims",
                    column: "RoleId");
            }

            // CreateIndex for AspNetRoles
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "RoleNameIndex",
                    table: "AspNetRoles",
                    column: "NormalizedName",
                    unique: true);
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "RoleNameIndex",
                    table: "AspNetRoles",
                    column: "NormalizedName",
                    unique: true);
            }

            // CreateIndex for AspNetUserClaims
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AspNetUserClaims_UserId",
                    table: "AspNetUserClaims",
                    column: "UserId");
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AspNetUserClaims_UserId",
                    table: "AspNetUserClaims",
                    column: "UserId");
            }

            // CreateIndex for AspNetUserLogins
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AspNetUserLogins_UserId",
                    table: "AspNetUserLogins",
                    column: "UserId");
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AspNetUserLogins_UserId",
                    table: "AspNetUserLogins",
                    column: "UserId");
            }

            // CreateIndex for AspNetUserRoles
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AspNetUserRoles_RoleId",
                    table: "AspNetUserRoles",
                    column: "RoleId");
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "IX_AspNetUserRoles_RoleId",
                    table: "AspNetUserRoles",
                    column: "RoleId");
            }

            // CreateIndex for AspNetUsers
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "EmailIndex",
                    table: "AspNetUsers",
                    column: "NormalizedEmail");
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "EmailIndex",
                    table: "AspNetUsers",
                    column: "NormalizedEmail");
            }

            // CreateIndex for AspNetUsers
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "UserNameIndex",
                    table: "AspNetUsers",
                    column: "NormalizedUserName",
                    unique: true);
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "UserNameIndex",
                    table: "AspNetUsers",
                    column: "NormalizedUserName",
                    unique: true);
            }

            // CreateIndex for Customers
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Customers_UserId",
                    table: "Customers",
                    column: "UserId",
                    unique: true);
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Customers_UserId",
                    table: "Customers",
                    column: "UserId",
                    unique: true);
            }

            // CreateIndex for Employees
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Employees_EmployeeNumber",
                    table: "Employees",
                    column: "EmployeeNumber",
                    unique: true);
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Employees_EmployeeNumber",
                    table: "Employees",
                    column: "EmployeeNumber",
                    unique: true);
            }

            // CreateIndex for Employees
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Employees_UserId",
                    table: "Employees",
                    column: "UserId",
                    unique: true);
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Employees_UserId",
                    table: "Employees",
                    column: "UserId",
                    unique: true);
            }

            // CreateIndex for Events
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Events_CustomerId",
                    table: "Events",
                    column: "CustomerId");
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Events_CustomerId",
                    table: "Events",
                    column: "CustomerId");
            }

            // CreateIndex for Events
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Events_EmployeeId",
                    table: "Events",
                    column: "EmployeeId");
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Events_EmployeeId",
                    table: "Events",
                    column: "EmployeeId");
            }

            // CreateIndex for Occurrences
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Occurrences_EventId",
                    table: "Occurrences",
                    column: "EventId");
            }
            else
            {
                migrationBuilder.CreateIndex(
                    name: "IX_Occurrences_EventId",
                    table: "Occurrences",
                    column: "EventId");
            }
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
                name: "Occurrences");

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
