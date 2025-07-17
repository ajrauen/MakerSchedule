
using MakerSchedule.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.Aggregates.DomainUser;
using MakerSchedule.Domain.ValueObjects;
using MakerSchedule.Domain.Constants;

namespace MakerSchedule.Infrastructure.Data;

public class SeedData
{
    // Remove SeedUsers since we'll create users through UserManager
    // public static List<User> SeedUsers => new List<User> { ... };

    public static List<DomainUser> SeedEmployees => new List<DomainUser>
    {
        new DomainUser
        {
            Id = Guid.NewGuid().ToString(),
            UserId = "11111111-1111-1111-1111-111111111111",
            // Add other required DomainUser properties if needed
        }
    };

    // Removed SeedCustomers

    // Generate GUIDs for each event and store them for reuse
    private static readonly string event1Id = Guid.NewGuid().ToString();
    private static readonly string event2Id = Guid.NewGuid().ToString();
    private static readonly string event3Id = Guid.NewGuid().ToString();
    private static readonly string event4Id = Guid.NewGuid().ToString();
    private static readonly string event5Id = Guid.NewGuid().ToString();

    public static List<Event> SeedEvents => new List<Event>
    {
        new Event
        {
            Id = event1Id,
            EventName = new EventName("Advanced Pottery"),
            Description = "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.",
            Duration = new Duration(120 * 60 * 1000), // 2 hours in milliseconds
            EventType = EventTypeEnum.Pottery
        },
        new Event
        {
            Id = event2Id,
            EventName = new EventName("Woodworking Workshop"),
            Description = "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.",
            Duration = new Duration(180 * 60 * 1000), // 3 hours in milliseconds
            EventType = EventTypeEnum.Woodworking
        },
        new Event
        {
            Id = event3Id,
            EventName = new EventName("Sewing Basics"),
            Description = "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.",
            Duration = new Duration(90 * 60 * 1000), // 1.5 hours in milliseconds
            EventType = EventTypeEnum.Sewing
        },
        new Event
        {
            Id = event4Id,
            EventName = new EventName("Pottery for Beginners"),
            Description = "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.",
            Duration = new Duration(150 * 60 * 1000), // 2.5 hours in milliseconds
            EventType = EventTypeEnum.Pottery
        },
        new Event
        {
            Id = event5Id,
            EventName = new EventName("Advanced Woodworking"),
            Description = "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.",
            Duration = new Duration(240 * 60 * 1000), // 4 hours in milliseconds
            EventType = EventTypeEnum.Woodworking
        }
    };

    public static List<Occurrence> SeedOccurrences
    {
        get
        {
            var occurrences = new List<Occurrence>();
            var random = new Random(42); // deterministic for repeatability
            int occurrenceId = 1;
            var now = DateTime.UtcNow;
            var durationOptions = Enumerable.Range(2, 8).Select(i => i * 15).ToArray(); // 30, 45, ..., 120

            // Use the static event IDs for occurrences
            var eventIds = new[] { event1Id, event2Id, event3Id, event4Id, event5Id };
            foreach (var eventId in eventIds)
            {
                int count = random.Next(3, 8); // 3 to 7 occurrences
                for (int i = 0; i < count; i++)
                {
                    var daysOffset = random.Next(0, 90);
                    var minutesOffset = random.Next(0, 24 * 60);
                    var start = now.AddDays(daysOffset).AddMinutes(minutesOffset);
                    var duration = durationOptions[random.Next(durationOptions.Length)];
                    var info = new OccurrenceInfo(start, duration);
                    var occurrence = new Occurrence(eventId, info)
                    {
                        Id = Guid.NewGuid().ToString()
                    };
                    occurrences.Add(occurrence);
                }
            }
            return occurrences;
        }
    }
}

public static class SeedUserData
{
    public static List<(string Email, string FirstName, string LastName, string Role)> Leaders = new()
    {
        ("leader1@ms.com", "Alice", "Smith", Roles.Leader),
        ("leader2@ms.com", "Bob", "Johnson", Roles.Leader),
        ("leader3@ms.com", "Carol", "Williams", Roles.Leader)
    };

    public static List<(string Email, string FirstName, string LastName, string Role)> Customers = new()
    {
        ("customer1@ms.com", "David", "Brown", Roles.Customer),
        ("customer2@ms.com", "Eve", "Davis", Roles.Customer),
        ("customer3@ms.com", "Frank", "Miller", Roles.Customer),
        ("customer4@ms.com", "Grace", "Wilson", Roles.Customer),
        ("customer5@ms.com", "Hank", "Moore", Roles.Customer),
        ("customer6@ms.com", "Ivy", "Taylor", Roles.Customer),
        ("customer7@ms.com", "Jack", "Anderson", Roles.Customer),
        ("customer8@ms.com", "Kathy", "Thomas", Roles.Customer),
        ("customer9@ms.com", "Leo", "Jackson", Roles.Customer),
        ("customer10@ms.com", "Mona", "White", Roles.Customer)
    };
}

public class DatabaseSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DatabaseSeeder(
        UserManager<User> userManager,
        ApplicationDbContext context,
        RoleManager<IdentityRole> roleManager,
        ILogger<DatabaseSeeder> logger)

    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
        _roleManager = roleManager;
    }

    private async Task SeedLeadersAndCustomersAsync()
    {
        var defaultPassword = "Password123!";
        var allUsers = SeedUserData.Leaders.Concat(SeedUserData.Customers);
        foreach (var (email, firstName, lastName, role) in allUsers)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            User user;
            if (existingUser == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                var result = await _userManager.CreateAsync(user, defaultPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    // If user creation failed, skip DomainUser creation for this user
                    continue;
                }
            }
            else
            {
                user = existingUser;
                if (!await _userManager.IsInRoleAsync(existingUser, role))
                {
                    await _userManager.AddToRoleAsync(existingUser, role);
                }
            }

            // Ensure associated DomainUser exists
            var domainUserExists = await _context.DomainUsers.AnyAsync(d => d.UserId == user.Id);
            if (!domainUserExists)
            {
                var domainUser = new DomainUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    PreferredContactMethod = "Email"
                    // Add other properties as needed
                };
                _context.DomainUsers.Add(domainUser);
                await _context.SaveChangesAsync();
            }
        }
    }

    private async Task SeedEventLeadersAndAttendeesAsync()
    {
        // Get all leaders and customers
        var leaders = await _userManager.GetUsersInRoleAsync(Roles.Leader);
        var customers = await _userManager.GetUsersInRoleAsync(Roles.Customer);
        var events = await _context.Events.ToListAsync();
        var occurrences = await _context.Occurrences.ToListAsync();

        // Assign each leader to lead 1-2 random occurrences
        var random = new Random(42);
        foreach (var leader in leaders)
        {
            var domainLeader = await _context.DomainUsers.FirstOrDefaultAsync(d => d.UserId == leader.Id);
            if (domainLeader == null) continue;
            var leaderOccurrences = occurrences.OrderBy(_ => random.Next()).Take(2).ToList();
            foreach (var occ in leaderOccurrences)
            {
                if (!await _context.OccurrenceLeaders.AnyAsync(x => x.OccurrenceId == occ.Id && x.UserId == domainLeader.Id))
                {
                    _context.OccurrenceLeaders.Add(new OccurrenceLeader
                    {
                        Id = Guid.NewGuid().ToString(),
                        OccurrenceId = occ.Id,
                        UserId = domainLeader.Id
                    });
                }
            }
        }

        // Assign each customer as attendee to 1-2 random occurrences
        foreach (var customer in customers)
        {
            var domainCustomer = await _context.DomainUsers.FirstOrDefaultAsync(d => d.UserId == customer.Id);
            if (domainCustomer == null) continue;
            var customerOccurrences = occurrences.OrderBy(_ => random.Next()).Take(2).ToList();
            foreach (var occ in customerOccurrences)
            {
                if (!await _context.OccurrenceAttendees.AnyAsync(x => x.OccurrenceId == occ.Id && x.UserId == domainCustomer.Id))
                {
                    _context.OccurrenceAttendees.Add(new OccurrenceAttendee
                    {
                        Id = Guid.NewGuid().ToString(),
                        OccurrenceId = occ.Id,
                        UserId = domainCustomer.Id
                    });
                }
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Starting database seeding process...");
            _logger.LogInformation("Current environment: {Environment}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

            // Check if we can access the database
            var canConnect = await _context.Database.CanConnectAsync();
            _logger.LogInformation("Database connection test: {CanConnect}", canConnect);

            // Check current data in tables
            var userCount = await _context.Users.CountAsync();
            var domainUserCount = await _context.DomainUsers.CountAsync();
            var eventCount = await _context.Events.CountAsync();
            _logger.LogInformation("Current table counts - Users: {UserCount}, DomainUsers: {DomainUserCount}, Events: {EventCount}",
                userCount, domainUserCount, eventCount);

            await SeedUserRolesAsync();
            await SeedAdminUserAsync();
            await SeedLeadersAndCustomersAsync();
            await SeedEventLeadersAndAttendeesAsync();
            _logger.LogInformation("Database seeding process completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during database seeding");
            throw; // Re-throw to see the actual error
        }
    }

    private async Task SeedUserRolesAsync()
    {
        string[] roles = new[] { Roles.Admin, Roles.Customer, Roles.Leader };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private async Task SeedAdminUserAsync()
    {
        try
        {
            const string adminEmail = "admin@ms.com";
            const string adminPassword = "Admin123!"; // Default password - should be changed after first login
            var adminUserId = "11111111-1111-1111-1111-111111111111";

            _logger.LogInformation("Starting admin user seeding process...");

            var existingUser = await _userManager.FindByEmailAsync(adminEmail);
            User adminUser;
            if (existingUser != null)
            {
                _logger.LogInformation("Admin user already exists: {Email}", adminEmail);
                adminUser = existingUser;
            }
            else
            {
                _logger.LogInformation("Creating new admin user: {Email}", adminEmail);
                adminUser = new User
                {
                    Id = adminUserId, // Fixed ID to match seed data
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    Address = "123 Admin St",
                    PhoneNumber = "123-456-7890",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    // UserType = UserType.Employee, // Remove or comment out if not present
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                _logger.LogInformation("About to create user with UserManager...");
                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                  


                    _logger.LogInformation("Admin user created successfully: {Email}", adminEmail);
                    _logger.LogWarning("Default admin password is: {Password}. Please change it after first login.", adminPassword);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to create admin user: {Errors}", errors);
                    _logger.LogWarning("Skipping admin user creation due to errors");
                    return; // Don't throw, just return
                }
            }

            if (!await _userManager.IsInRoleAsync(adminUser, Roles.Admin))
            {
                await _userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }

            // Ensure associated DomainUser exists
            _logger.LogInformation("Checking for admin domain user record...");
            var adminDomainUser = await _context.DomainUsers.FirstOrDefaultAsync(e => e.UserId == adminUser.Id);
            if (adminDomainUser == null)
            {
                _logger.LogInformation("Creating admin domain user record...");
                try
                {
                    var domainUser = new DomainUser
                    {
                        UserId = adminUser.Id,
                        PreferredContactMethod = "Email"
                    };
                    _logger.LogInformation("DomainUser object created with UserId: {UserId}", domainUser.UserId);

                    _context.DomainUsers.Add(domainUser);
                    _logger.LogInformation("DomainUser added to context");

                    var saveResult = await _context.SaveChangesAsync();
                    _logger.LogInformation("Admin domain user record created for user: {Email}. SaveChanges result: {SaveResult}", adminEmail, saveResult);

                    // Verify the record was actually saved
                    var verifyUser = await _context.DomainUsers.FirstOrDefaultAsync(e => e.UserId == adminUser.Id);
                    if (verifyUser != null)
                    {
                        _logger.LogInformation("Verified: DomainUser record exists with ID: {DomainUserId}", verifyUser.Id);
                    }
                    else
                    {
                        _logger.LogError("Failed to verify DomainUser record was saved!");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception occurred while creating DomainUser record");
                    throw; // Re-throw to see the actual error
                }
            }
            else
            {
                _logger.LogInformation("Admin domain user record already exists for user: {Email} with ID: {DomainUserId}", adminEmail, adminDomainUser.Id);
            }

            _logger.LogInformation("Admin user seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during admin user seeding");
            throw; // Re-throw to see the actual error
        }
    }
}
