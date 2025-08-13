
using MakerSchedule.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MakerSchedule.Domain.Aggregates.Event;
using MakerSchedule.Domain.Aggregates.EventType;
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
            Id = Guid.NewGuid(),
            UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            PreferredContactMethod = "Email",
            Email = new Email("admin@ms.com"),
            PhoneNumber = new PhoneNumber("1234567890"),
            FirstName = "Admin",
            LastName = "User",
            Address = "123 Admin St, City, Country",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        }
    };

    // Removed SeedCustomers

    // Seed EventTypes (use static GUIDs matching migration/SQL)
    private static readonly Guid woodworkingTypeId = Guid.Parse("e25981d8-cd4d-412f-b261-eede0559c5f6");
    private static readonly Guid sewingTypeId = Guid.Parse("dd74cd38-bf1d-4694-8b28-2cccbdf44fe9");
    private static readonly Guid potteryTypeId = Guid.Parse("4861f65b-fff2-42b1-a5a1-e55a45bfc2ef");

    public static List<EventType> SeedEventTypes => new List<EventType>
    {
        new EventType { Id = woodworkingTypeId, Name = new EventTypeName("Woodworking") },
        new EventType { Id = potteryTypeId, Name = new EventTypeName("Pottery") },
        new EventType { Id = sewingTypeId, Name = new EventTypeName("Sewing") }
    };

    // Generate GUIDs for each event and store them for reuse - match migration GUIDs
    private static readonly Guid event1Id = Guid.Parse("c2f8d0f9-b2af-4d3a-91da-13c112c6212c"); // Advanced Pottery
    private static readonly Guid event2Id = Guid.Parse("3709300b-3c35-4350-9f3c-277759214bbb"); // Woodworking Workshop
    private static readonly Guid event3Id = Guid.Parse("5df8ac2c-b1af-4f9d-861d-a3e1e42b23ec"); // Sewing Basics

    public static List<Event> SeedEvents => new List<Event>
    {
        new Event
        {
            Id = event1Id,
            EventName = new EventName("Advanced Pottery"),
            Description = "Advanced pottery techniques for experienced artists. Wheel throwing and glazing. In this workshop, participants will explore complex forms and surface decoration methods, including carving, slip trailing, and underglaze painting. The instructor will demonstrate advanced wheel techniques, such as making large vessels and assembling multi-part pieces. You will also learn about glaze chemistry, firing schedules, and troubleshooting common issues. Bring your creative ideas and prepare to push your skills to the next level. All materials and firing fees are included. Prior pottery experience is required for this class.",
            Duration = new Duration(120 ),
            EventTypeId = potteryTypeId
        },
        new Event
        {
            Id = event2Id,
            EventName = new EventName("Woodworking Workshop"),
            Description = "Learn to build a simple wooden shelf. All materials provided. This hands-on workshop covers the basics of woodworking, including measuring, cutting, sanding, and assembling wood pieces. You will use both hand and power tools under the guidance of an experienced instructor. Safety procedures and tool maintenance will be emphasized throughout the session. By the end of the class, you will have constructed your own sturdy shelf to take home. The workshop also includes tips on finishing techniques, such as staining and sealing, to enhance the appearance and durability of your project. Suitable for all skill levels.",
            Duration = new Duration(180),
            EventTypeId = woodworkingTypeId
        },
        new Event
        {
            Id = event3Id,
            EventName = new EventName("Sewing Basics"),
            Description = "Introduction to sewing for beginners. Learn to use a sewing machine and create simple projects. This class covers the fundamentals of sewing, including threading a machine, selecting fabrics, reading patterns, and basic stitches. You will practice on scrap fabric before creating a simple project to take home. The instructor will provide guidance on choosing the right materials and tools for your projects. Perfect for those who want to start sewing their own clothes or home decor items. All equipment and materials are provided.",
            Duration = new Duration(90 ), 
            EventTypeId = sewingTypeId
        }
    };

    public static List<Occurrence> SeedOccurrences
    {
        get
        {
            var occurrences = new List<Occurrence>();
            var random = new Random(42); // deterministic for repeatability
            var now = DateTime.UtcNow;
            var durationOptions = Enumerable.Range(2, 8).Select(i => i * 15).ToArray(); // 30, 45, ..., 120

            // Use the static event IDs for occurrences
            var eventIds = new[] { event1Id, event2Id, event3Id };
            foreach (var eventId in eventIds)
            {
                int count = random.Next(3, 8); // 3 to 7 occurrences
                for (int i = 0; i < count; i++)
                {
                    // Alternate between past and future occurrences
                    int daysOffset;
                    if (i % 2 == 0)
                    {
                        // Past: 1 to 45 days ago
                        daysOffset = -random.Next(1, 46);
                    }
                    else
                    {
                        // Future: 1 to 45 days ahead
                        daysOffset = random.Next(1, 46);
                    }
                    // Business hours: 9 AM to 6 PM (9:00 to 18:00) CST
                    var businessHourStart = 9; // 9 AM
                    var businessHourEnd = 18; // 6 PM
                    var businessHours = businessHourEnd - businessHourStart; // 9 hours
                    // Random hour within business hours
                    var randomHour = businessHourStart + random.Next(businessHours);
                    // Random minute (0, 15, 30, or 45 for cleaner times)
                    var randomMinute = random.Next(4) * 15;
                    // Define CST zone before use
                    var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                    // Generate the local CST time with Kind=Unspecified
                    var localCstDate = DateTime.SpecifyKind(
                        now.Date.AddDays(daysOffset)
                            .AddHours(randomHour)
                            .AddMinutes(randomMinute),
                        DateTimeKind.Unspecified
                    );
                    // Convert CST to UTC
                    var start = TimeZoneInfo.ConvertTimeToUtc(localCstDate, cstZone);
                    
                    var duration = durationOptions[random.Next(durationOptions.Length) ] * 60 ;
                    
                    // Create occurrence directly with ScheduleStart.ForSeeding to bypass future date validation
                    var occurrence = new Occurrence
                    {
                        Id = Guid.NewGuid(),
                        EventId = eventId,
                        ScheduleStart = ScheduleStart.ForSeeding(start),
                        Status = (start.AddMinutes(duration) < DateTime.UtcNow)
                            ? OccurrenceStatus.Complete
                            : OccurrenceStatus.Pending
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
        ("customer10@ms.com", "Mona", "White", Roles.Customer),
        ("customer11@ms.com", "Nina", "Martin", Roles.Customer),
        ("customer12@ms.com", "Oscar", "Lee", Roles.Customer),
        ("customer13@ms.com", "Paula", "Clark", Roles.Customer),
        ("customer14@ms.com", "Quinn", "Lewis", Roles.Customer),
        ("customer15@ms.com", "Rita", "Walker", Roles.Customer),
        ("customer16@ms.com", "Sam", "Hall", Roles.Customer),
        ("customer17@ms.com", "Tina", "Allen", Roles.Customer),
        ("customer18@ms.com", "Uma", "Young", Roles.Customer),
        ("customer19@ms.com", "Vince", "King", Roles.Customer),
        ("customer20@ms.com", "Wendy", "Wright", Roles.Customer),
        ("customer21@ms.com", "Xander", "Scott", Roles.Customer),
        ("customer22@ms.com", "Yara", "Green", Roles.Customer),
        ("customer23@ms.com", "Zane", "Baker", Roles.Customer),
        ("customer24@ms.com", "Amy", "Adams", Roles.Customer),
        ("customer25@ms.com", "Brian", "Nelson", Roles.Customer),
        ("customer26@ms.com", "Cathy", "Carter", Roles.Customer),
        ("customer27@ms.com", "Derek", "Mitchell", Roles.Customer),
        ("customer28@ms.com", "Elena", "Perez", Roles.Customer),
        ("customer29@ms.com", "Felix", "Roberts", Roles.Customer),
        ("customer30@ms.com", "Gina", "Turner", Roles.Customer),
        ("customer31@ms.com", "Harvey", "Phillips", Roles.Customer),
        ("customer32@ms.com", "Isabel", "Campbell", Roles.Customer),
        ("customer33@ms.com", "Jon", "Parker", Roles.Customer),
        ("customer34@ms.com", "Kara", "Evans", Roles.Customer),
        ("customer35@ms.com", "Liam", "Edwards", Roles.Customer),
        ("customer36@ms.com", "Maya", "Collins", Roles.Customer),
        ("customer37@ms.com", "Noah", "Stewart", Roles.Customer),
        ("customer38@ms.com", "Olga", "Sanchez", Roles.Customer),
        ("customer39@ms.com", "Peter", "Morris", Roles.Customer),
        ("customer40@ms.com", "Queenie", "Rogers", Roles.Customer),
        ("customer41@ms.com", "Ray", "Reed", Roles.Customer),
        ("customer42@ms.com", "Sara", "Cook", Roles.Customer),
        ("customer43@ms.com", "Tom", "Morgan", Roles.Customer),
        ("customer44@ms.com", "Ursula", "Bell", Roles.Customer),
        ("customer45@ms.com", "Vera", "Murphy", Roles.Customer),
        ("customer46@ms.com", "Will", "Bailey", Roles.Customer),
        ("customer47@ms.com", "Xenia", "Rivera", Roles.Customer),
        ("customer48@ms.com", "Yusuf", "Cooper", Roles.Customer),
        ("customer49@ms.com", "Zelda", "Richardson", Roles.Customer),
        ("customer50@ms.com", "Ava", "Ward", Roles.Customer)
    };
}

public class DatabaseSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public DatabaseSeeder(
        UserManager<User> userManager,
        ApplicationDbContext context,
        RoleManager<IdentityRole<Guid>> roleManager,
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
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
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
                // Ensure we have a valid email
                if (string.IsNullOrEmpty(user.Email))
                {
                    _logger.LogWarning("User {UserId} has no email, skipping DomainUser creation", user.Id);
                    continue;
                }
                
                // Ensure we have a valid phone number
                var phoneNumber = !string.IsNullOrEmpty(user.PhoneNumber) 
                    ? user.PhoneNumber 
                    : "0000000000"; // Default phone number if none provided
                
                var domainUser = new DomainUser
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    PreferredContactMethod = "Email",
                    Email = new Email(user.Email),
                    PhoneNumber = new PhoneNumber(phoneNumber),
                    FirstName = firstName,
                    LastName = lastName,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
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

        // Ensure every occurrence has at least one leader
        var random = new Random(42);
        var domainLeaders = new List<DomainUser>();
        // Get all domain leaders
        foreach (var leader in leaders)
        {
            var domainLeader = await _context.DomainUsers.FirstOrDefaultAsync(d => d.UserId == leader.Id);
            if (domainLeader != null)
            {
                domainLeaders.Add(domainLeader);
            }
        }

        // Assign at least one unique leader to each occurrence
        var shuffledLeaders = domainLeaders.OrderBy(_ => random.Next()).ToList();
        for (int i = 0; i < occurrences.Count; i++)
        {
            var leader = shuffledLeaders[i % shuffledLeaders.Count];
            if (!await _context.OccurrenceLeaders.AnyAsync(x => x.OccurrenceId == occurrences[i].Id && x.UserId == leader.Id))
            {
                _context.OccurrenceLeaders.Add(new OccurrenceLeader
                {
                    Id = Guid.NewGuid(),
                    OccurrenceId = occurrences[i].Id,
                    UserId = leader.Id
                });
            }
        }

        // Optionally, assign each leader to 1-2 more random occurrences for variety, but never duplicate
        foreach (var domainLeader in domainLeaders)
        {
            var leaderOccurrences = occurrences.OrderBy(_ => random.Next()).Take(2).ToList();
            foreach (var occ in leaderOccurrences)
            {
                if (!await _context.OccurrenceLeaders.AnyAsync(x => x.OccurrenceId == occ.Id && x.UserId == domainLeader.Id))
                {
                    _context.OccurrenceLeaders.Add(new OccurrenceLeader
                    {
                        Id = Guid.NewGuid(),
                        OccurrenceId = occ.Id,
                        UserId = domainLeader.Id
                    });
                }
            }
        }

        // Assign customers to occurrences: ensure each occurrence has at least 10 unique customers, no duplicates
        var domainCustomers = new List<DomainUser>();
        foreach (var customer in customers)
        {
            var domainCustomer = await _context.DomainUsers.FirstOrDefaultAsync(d => d.UserId == customer.Id);
            if (domainCustomer != null)
            {
                domainCustomers.Add(domainCustomer);
            }
        }

        // Shuffle customers for random assignment
        var shuffledCustomers = domainCustomers.OrderBy(_ => random.Next()).ToList();

        // Track which customers are assigned to which occurrences
        var occurrenceToCustomers = new Dictionary<Guid, HashSet<Guid>>();
        foreach (var occ in occurrences)
        {
            occurrenceToCustomers[occ.Id] = new HashSet<Guid>(
                (await _context.OccurrenceAttendees.Where(x => x.OccurrenceId == occ.Id).Select(x => x.UserId).ToListAsync())
            );
        }

        // First, ensure each occurrence has at least 10 unique customers
        int customerIndex = 0;
        foreach (var occ in occurrences)
        {
            var assigned = occurrenceToCustomers[occ.Id];
            while (assigned.Count < 10 && customerIndex < shuffledCustomers.Count)
            {
                var customer = shuffledCustomers[customerIndex];
                if (!assigned.Contains(customer.Id))
                {
                    _context.OccurrenceAttendees.Add(new OccurrenceAttendee
                    {
                        Id = Guid.NewGuid(),
                        OccurrenceId = occ.Id,
                        UserId = customer.Id
                    });
                    assigned.Add(customer.Id);
                }
                customerIndex = (customerIndex + 1) % shuffledCustomers.Count;
            }
        }

        // Then, assign each customer to up to 2 more random occurrences (no duplicates)
        foreach (var customer in shuffledCustomers)
        {
            // Get occurrences this customer is already assigned to
            var assignedOccs = new HashSet<Guid>(
                await _context.OccurrenceAttendees.Where(x => x.UserId == customer.Id).Select(x => x.OccurrenceId).ToListAsync()
            );
            // Pick up to 2 more occurrences not already assigned
            var availableOccs = occurrences.Where(o => !assignedOccs.Contains(o.Id)).OrderBy(_ => random.Next()).Take(2);
            foreach (var occ in availableOccs)
            {
                _context.OccurrenceAttendees.Add(new OccurrenceAttendee
                {
                    Id = Guid.NewGuid(),
                    OccurrenceId = occ.Id,
                    UserId = customer.Id
                });
            }
        }

        await _context.SaveChangesAsync();
    }

    // Add a method to seed EventTypes manually
    private async Task SeedEventTypesAsync()
    {
        // Define the static GUIDs from SeedData
        Guid woodworkingTypeId = Guid.Parse("e25981d8-cd4d-412f-b261-eede0559c5f6");
        Guid potteryTypeId = Guid.Parse("4861f65b-fff2-42b1-a5a1-e55a45bfc2ef");
        Guid sewingTypeId = Guid.Parse("dd74cd38-bf1d-4694-8b28-2cccbdf44fe9");

        // Check if event types already exist
        if (!await _context.EventTypes.AnyAsync())
        {
            _logger.LogInformation("Seeding EventTypes...");
            
            // Create event types with proper names
            var woodworkingType = new EventType { 
                Id = woodworkingTypeId, 
                Name = new EventTypeName("Woodworking") 
            };
            
            var potteryType = new EventType { 
                Id = potteryTypeId, 
                Name = new EventTypeName("Pottery") 
            };
            
            var sewingType = new EventType { 
                Id = sewingTypeId, 
                Name = new EventTypeName("Sewing") 
            };
            
            // Add them to the context
            _context.EventTypes.Add(woodworkingType);
            _context.EventTypes.Add(potteryType);
            _context.EventTypes.Add(sewingType);
            
            // Save changes to the database
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Successfully seeded EventTypes.");
        }
        else
        {
            _logger.LogInformation("EventTypes already exist, skipping seeding.");
        }
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
            
            // Seed EventTypes first - this is critical as Events depend on them
            await SeedEventTypesAsync();
            
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
                await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }

    private async Task SeedAdminUserAsync()
    {
        try
        {
            const string adminEmail = "admin@ms.com";
            const string adminPassword = "Admin123!"; // Default password - should be changed after first login
            const string adminPhoneNumber = "1234567890"; 
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
                    Id = Guid.Parse(adminUserId),
                    UserName = adminEmail,
                    Email = adminEmail,
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
            _logger.LogInformation("adminUser.Id: {AdminUserId}, adminEmail: {AdminEmail}", adminUser?.Id, adminEmail);
            if (string.IsNullOrWhiteSpace(adminEmail))
            {
                _logger.LogError("Cannot create DomainUser: adminEmail is null or empty!");
                return;
            }
            if (adminUser == null)
            {
                _logger.LogError("Cannot create DomainUser: adminUser is null!");
                return;
            }
            var adminDomainUser = await _context.DomainUsers.FirstOrDefaultAsync(e => e.UserId == adminUser.Id);
            if (adminDomainUser == null)
            {
                _logger.LogInformation("Creating admin domain user record...");
                try
                {
                    _logger.LogInformation("Preparing to create DomainUser with values: UserId={UserId}, Email={Email}, PhoneNumber={PhoneNumber}, FirstName={FirstName}, LastName={LastName}, Address={Address}",
                        adminUser.Id,
                        adminEmail,
                        adminPhoneNumber,
                        adminUser.DomainUser?.FirstName ?? "Admin",
                        adminUser.DomainUser?.LastName ?? "User",
                        adminUser.DomainUser?.Address ?? string.Empty);
                    Email? domainEmailObj = null;
                    string firstName = adminUser.DomainUser?.FirstName ?? "Admin";
                    string lastName = adminUser.DomainUser?.LastName ?? "User";
                    string address = adminUser.DomainUser?.Address ?? string.Empty;
                    string phoneNumber = adminPhoneNumber;
                    _logger.LogInformation("DomainUser constructor args: UserId={UserId}, Email={Email}, PhoneNumber={PhoneNumber}, FirstName={FirstName}, LastName={LastName}, Address={Address}",
                        adminUser.Id, adminEmail, phoneNumber, firstName, lastName, address);
                    if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                    {
                        _logger.LogError("Cannot create DomainUser: One or more required values are null or empty. Email={Email}, PhoneNumber={PhoneNumber}, FirstName={FirstName}, LastName={LastName}", adminEmail, phoneNumber, firstName, lastName);
                        return;
                    }
                    try
                    {
                        domainEmailObj = new Email(adminEmail);
                        _logger.LogInformation("Successfully created Email value object: {EmailValue}", domainEmailObj.Value);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to create Email value object with value: '{Email}'", adminEmail);
                        throw;
                    }
                    if (domainEmailObj == null || string.IsNullOrWhiteSpace(domainEmailObj.Value))
                    {
                        _logger.LogError("Cannot create DomainUser: domainEmailObj is null or its Value is empty. domainEmailObj={DomainEmailObj}, type={Type}", domainEmailObj, domainEmailObj?.GetType());
                        return;
                    }
                    _logger.LogInformation("About to construct DomainUser: UserId={UserId}, Email={Email}, PhoneNumber={PhoneNumber}, FirstName={FirstName}, LastName={LastName}, Address={Address}, EmailObjType={EmailObjType}",
                        adminUser.Id, domainEmailObj.Value, phoneNumber, firstName, lastName, address, domainEmailObj.GetType());
                    var domainUser = new DomainUser
                    {
                        UserId = adminUser.Id,
                        PreferredContactMethod = "Email",
                        Email = domainEmailObj,
                        PhoneNumber = new PhoneNumber(phoneNumber),
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };
                    _logger.LogInformation("DomainUser constructed: UserId={UserId}, Email={Email}, PhoneNumber={PhoneNumber}, FirstName={FirstName}, LastName={LastName}, Address={Address}, EmailObjType={EmailObjType}, EmailObjValue={EmailObjValue}",
                        domainUser.UserId, domainUser.Email?.Value, domainUser.PhoneNumber?.Value, domainUser.FirstName, domainUser.LastName, domainUser.Address, domainUser.Email?.GetType(), domainUser.Email?.Value);

                    _context.DomainUsers.Add(domainUser);
                    _logger.LogInformation("DomainUser added to context. Email property: {Email}", domainUser.Email?.Value);

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
