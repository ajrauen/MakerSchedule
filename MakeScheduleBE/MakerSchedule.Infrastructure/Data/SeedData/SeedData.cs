
using MakerSchedule.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MakerSchedule.Domain.Aggregates.Customer;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Aggregates.Employee;
using MakerSchedule.Domain.Aggregates.User;

namespace MakerSchedule.Infrastructure.Data;

public class SeedData
{
    // Remove SeedUsers since we'll create users through UserManager
    // public static List<User> SeedUsers => new List<User> { ... };

    public static List<Employee> SeedEmployees => new List<Employee>
    {
        new Employee
        {
            Id = 1,
            UserId = "11111111-1111-1111-1111-111111111111",
            EmployeeNumber = "EMP001",
            Department = "Administration",
            Position = "Administrator",
            HireDate = new DateTime(2020, 1, 15)
        }
    };

    public static List<Customer> SeedCustomers => new List<Customer>
    {
        // No customers in seed data for now
    };

    public static List<Event> SeedEvents => new List<Event>
    {
        new Event
        {
            Id = 1,
            EventName = "Advanced Pottery",
            Description = "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.",
            Duration = 120 * 60 * 1000, // 2 hours in milliseconds
            EventType = EventTypeEnum.Pottery
        },
        new Event
        {
            Id = 2,
            EventName = "Woodworking Workshop",
            Description = "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.",
            Duration = 180 * 60 * 1000, // 3 hours in milliseconds
            EventType = EventTypeEnum.Woodworking
        },
        new Event
        {
            Id = 3,
            EventName = "Sewing Basics",
            Description = "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.",
            Duration = 90 * 60 * 1000, // 1.5 hours in milliseconds
            EventType = EventTypeEnum.Sewing
        },
        new Event
        {
            Id = 4,
            EventName = "Pottery for Beginners",
            Description = "Introduction to pottery and clay work. Learn basic hand-building techniques. This beginner-friendly class introduces you to the world of ceramics through hand-building methods like pinch pots, coil building, and slab construction. You will learn about different types of clay, basic glazing techniques, and the firing process. The instructor will guide you through creating several small pieces that will be fired and glazed. No prior experience is necessary. All materials and firing fees are included.",
            Duration = 150 * 60 * 1000, // 2.5 hours in milliseconds
            EventType = EventTypeEnum.Pottery
        },
        new Event
        {
            Id = 5,
            EventName = "Advanced Woodworking",
            Description = "Advanced woodworking techniques for experienced craftsmen. Learn joinery and finishing methods. This advanced workshop focuses on traditional woodworking joinery techniques such as dovetails, mortise and tenon, and finger joints. You will also learn advanced finishing techniques including French polishing, oil finishes, and lacquer application. The class includes safety training for power tools and hand tools. Participants should have basic woodworking experience. Bring your own safety equipment or use ours.",
            Duration = 240 * 60 * 1000, // 4 hours in milliseconds
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

            foreach (var ev in SeedEvents)
            {
                int count = random.Next(3, 8); // 3 to 7 occurrences
                for (int i = 0; i < count; i++)
                {
                    var daysOffset = random.Next(0, 90);
                    var minutesOffset = random.Next(0, 24 * 60);
                    var start = now.AddDays(daysOffset).AddMinutes(minutesOffset);
                    var duration = durationOptions[random.Next(durationOptions.Length)];
                    var info = new OccurrenceInfo(start, duration, new List<int>(), new List<int>());
                    var occurrence = new Occurrence(ev.Id, info)
                    {
                        Id = occurrenceId++
                    };
                    occurrences.Add(occurrence);
                }
            }
            return occurrences;
        }
    }
}

public class DatabaseSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        UserManager<User> userManager,
        ApplicationDbContext context,
        ILogger<DatabaseSeeder> logger)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Starting database seeding process...");
            await SeedAdminUserAsync();
            _logger.LogInformation("Database seeding process completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during database seeding");
            // Don't re-throw - let the application continue
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
                    UserType = UserType.Employee,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

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

            // Ensure associated Employee exists
            _logger.LogInformation("Checking for admin employee record...");
            var adminEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == adminUser.Id);
            if (adminEmployee == null)
            {
                _logger.LogInformation("Creating admin employee record...");
                var employee = new Employee
                {
                    UserId = adminUser.Id,
                    EmployeeNumber = "EMP001",
                    Department = "Administration",
                    Position = "Administrator",
                    HireDate = new DateTime(2020, 1, 15)
                };
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Admin employee record created for user: {Email}", adminEmail);
            }
            else
            {
                _logger.LogInformation("Admin employee record already exists for user: {Email}", adminEmail);
            }

            _logger.LogInformation("Admin user seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during admin user seeding");
            // Don't re-throw - let the application continue
        }
    }
}
