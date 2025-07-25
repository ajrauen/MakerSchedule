using Xunit;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using Microsoft.EntityFrameworkCore;
using MakerSchedule.Infrastructure.Data;
using System;
using Moq;
using MakerSchedule.Domain.Aggregates.User;
using MakerSchedule.Domain.Aggregates.DomainUser;
using MakerSchedule.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

public class DomainUserServiceTests : IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer;
    public ApplicationDbContext? DbContext { get; private set; }

    public DomainUserServiceTests()
    {
        _sqlContainer = new MsSqlBuilder()
            .WithPassword("Your_password123")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(_sqlContainer.GetConnectionString())
            .Options;
        DbContext = new ApplicationDbContext(options);
        await DbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _sqlContainer.DisposeAsync();
    }

    [Fact]
    public async Task GetAvailableOccurrenceLeadersAsync_ReturnsExpectedLeaders_WithRealDb()
    {
        // Arrange: Seed test leader user and domain user
        var testLeader = new User { Id = "leader1", UserName = "leader1@ms.com", FirstName = "Test", LastName = "Leader" };
        var testDomainUser = new DomainUser { Id = "domain1", UserId = "leader1", User = testLeader };
        DbContext!.Users.Add(testLeader);
        DbContext.DomainUsers.Add(testDomainUser);
        await DbContext.SaveChangesAsync();

        // Seed occurrence
        var testEvent = new MakerSchedule.Domain.Aggregates.Event.Event
        {
            Id = "event1",
            EventName = new MakerSchedule.Domain.ValueObjects.EventName("Test Event"),
            Description = "Test event description",
            EventType = MakerSchedule.Domain.Enums.EventTypeEnum.Woodworking
        };
        DbContext!.Events.Add(testEvent);
        await DbContext.SaveChangesAsync();

        var now = DateTime.UtcNow.AddHours(5);
        var testOccurrence = new MakerSchedule.Domain.Aggregates.Event.Occurrence
        {
            Id = "occ1",
            EventId = "event1",
            ScheduleStart = new MakerSchedule.Domain.ValueObjects.ScheduleStart(now),
            Duration = 60
        };
        DbContext.Occurrences.Add(testOccurrence);
        await DbContext.SaveChangesAsync();

        // Add a second leader and domain user
        var testLeader2 = new User { Id = "leader2", UserName = "leader2@ms.com", FirstName = "Busy", LastName = "Leader" };
        var testDomainUser2 = new DomainUser { Id = "domain2", UserId = "leader2", User = testLeader2 };
        DbContext!.Users.Add(testLeader2);
        DbContext.DomainUsers.Add(testDomainUser2);
        await DbContext.SaveChangesAsync();

        // Add a second occurrence that overlaps with the first
        var overlappingOccurrence = new MakerSchedule.Domain.Aggregates.Event.Occurrence
        {
            Id = "occ2",
            EventId = "event1",
            ScheduleStart = new MakerSchedule.Domain.ValueObjects.ScheduleStart(now), // same as occ1
            Duration = 60
        };
        DbContext.Occurrences.Add(overlappingOccurrence);
        await DbContext.SaveChangesAsync();

        // Assign leader2 as a leader for the overlapping occurrence
        var busyLeader = new MakerSchedule.Domain.Aggregates.Event.OccurrenceLeader
        {
            Id = "ol1",
            OccurrenceId = "occ2",
            UserId = "domain2",
            AssignedAt = DateTime.UtcNow
        };
        DbContext.OccurrenceLeaders.Add(busyLeader);
        await DbContext.SaveChangesAsync();

        // Setup UserManager mock to return both leaders
        var store = new Mock<IUserStore<User>>();
        var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        mockUserManager.Setup(m => m.GetUsersInRoleAsync(Roles.Leader))
            .ReturnsAsync(new List<User> {
                new User { Id = "leader1", UserName = "leader1@ms.com" },
                new User { Id = "leader2", UserName = "leader2@ms.com" }
            });

        var mockLogger = new Mock<ILogger<MakerSchedule.Application.Services.DomainUserService>>();
        var mockMapper = new Mock<IMapper>();

        var service = new MakerSchedule.Application.Services.DomainUserService(
            DbContext,
            mockLogger.Object,
            mockUserManager.Object,
            mockMapper.Object);

        long startTimeMs = new DateTimeOffset(now).ToUnixTimeMilliseconds();
        long durationMs = 60 * 60 * 1000; // 1 hour in ms
        var result = await service.GetAvailableOccurrenceLeadersAsync((long)now.Ticks, 60);

        // Assert
        Assert.NotNull(result);
        var ids = result.Select(x => x.Id).ToList();
        Assert.Single(ids);
        Assert.Contains("domain1", ids); // Only the available leader
        Assert.DoesNotContain("domain2", ids); // The busy leader should not be returned
    }

    [Fact]
    public async Task GetAllDomainUsersByRoleAsync_ReturnsAllLeaders()
    {
        // Arrange: Seed two leaders and domain users
        var leader1 = new User { Id = "leader1", UserName = "leader1@ms.com", FirstName = "Test", LastName = "Leader" };
        var domainUser1 = new DomainUser { Id = "domain1", UserId = "leader1", User = leader1 };
        var leader2 = new User { Id = "leader2", UserName = "leader2@ms.com", FirstName = "Busy", LastName = "Leader" };
        var domainUser2 = new DomainUser { Id = "domain2", UserId = "leader2", User = leader2 };
        // Add some non-leader users
        var user3 = new User { Id = "user3", UserName = "user3@ms.com", FirstName = "Non", LastName = "Leader" };
        var domainUser3 = new DomainUser { Id = "domain3", UserId = "user3", User = user3 };
        var user4 = new User { Id = "user4", UserName = "user4@ms.com", FirstName = "Another", LastName = "User" };
        var domainUser4 = new DomainUser { Id = "domain4", UserId = "user4", User = user4 };
        DbContext!.Users.AddRange(leader1, leader2, user3, user4);
        DbContext.DomainUsers.AddRange(domainUser1, domainUser2, domainUser3, domainUser4);
        await DbContext.SaveChangesAsync();

        // Setup UserManager mock to return only the leaders for the role
        var store = new Mock<IUserStore<User>>();
        var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        mockUserManager.Setup(m => m.GetUsersInRoleAsync(Roles.Leader))
            .ReturnsAsync(new List<User> {
                new User { Id = "leader1", UserName = "leader1@ms.com" },
                new User { Id = "leader2", UserName = "leader2@ms.com" }
            });

        var mockLogger = new Mock<ILogger<MakerSchedule.Application.Services.DomainUserService>>();
        var mockMapper = new Mock<IMapper>();

        var service = new MakerSchedule.Application.Services.DomainUserService(
            DbContext,
            mockLogger.Object,
            mockUserManager.Object,
            mockMapper.Object);

        // Act
        var result = await service.GetAllDomainUsersByRoleAsync(Roles.Leader);

        // Assert
        Assert.NotNull(result);
        var ids = result.Select(x => x.Id).ToList();
        Assert.Contains("domain1", ids);
        Assert.Contains("domain2", ids);
        Assert.Equal(2, ids.Count);
        Assert.DoesNotContain("domain3", ids);
        Assert.DoesNotContain("domain4", ids);
    }

    [Fact]
    public async Task GetAllDomainUsersWithDetailsAsync_ReturnsAllDomainUsers()
    {
        // Arrange
        var user1 = new User { Id = "user1", UserName = "user1@ms.com", FirstName = "A", LastName = "One" };
        var user2 = new User { Id = "user2", UserName = "user2@ms.com", FirstName = "B", LastName = "Two" };
        var domainUser1 = new DomainUser { Id = "domain1", UserId = "user1", User = user1 };
        var domainUser2 = new DomainUser { Id = "domain2", UserId = "user2", User = user2 };
        DbContext!.Users.AddRange(user1, user2);
        DbContext.DomainUsers.AddRange(domainUser1, domainUser2);
        await DbContext.SaveChangesAsync();

        var store = new Mock<IUserStore<User>>();
        var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        var mockLogger = new Mock<ILogger<MakerSchedule.Application.Services.DomainUserService>>();
        var mockMapper = new Mock<IMapper>();
        var service = new MakerSchedule.Application.Services.DomainUserService(
            DbContext,
            mockLogger.Object,
            mockUserManager.Object,
            mockMapper.Object);

        // Act
        var result = await service.GetAllDomainUsersWithDetailsAsync();

        // Assert
        Assert.NotNull(result);
        var ids = result.Select(x => x.Id).ToList();
        Assert.Contains("domain1", ids);
        Assert.Contains("domain2", ids);
        Assert.Equal(2, ids.Count);
    }

    [Fact]
    public async Task GetAllDomainUsersAsync_ReturnsAllDomainUserDTOs()
    {
        // Arrange
        var user1 = new User { Id = "user1", UserName = "user1@ms.com", FirstName = "A", LastName = "One" };
        var user2 = new User { Id = "user2", UserName = "user2@ms.com", FirstName = "B", LastName = "Two" };
        var domainUser1 = new DomainUser { Id = "domain1", UserId = "user1", User = user1 };
        var domainUser2 = new DomainUser { Id = "domain2", UserId = "user2", User = user2 };
        DbContext!.Users.AddRange(user1, user2);
        DbContext.DomainUsers.AddRange(domainUser1, domainUser2);
        await DbContext.SaveChangesAsync();

        var store = new Mock<IUserStore<User>>();
        var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        var mockLogger = new Mock<ILogger<MakerSchedule.Application.Services.DomainUserService>>();
        var mockMapper = new Mock<IMapper>();
        var service = new MakerSchedule.Application.Services.DomainUserService(
            DbContext,
            mockLogger.Object,
            mockUserManager.Object,
            mockMapper.Object);

        // Act
        var result = await service.GetAllDomainUsersAsync();

        // Assert
        Assert.NotNull(result);
        var ids = result.Select(x => x.Id).ToList();
        Assert.Contains("domain1", ids);
        Assert.Contains("domain2", ids);
        Assert.Equal(2, ids.Count);
    }

    [Fact]
    public async Task GetDomainUserByIdAsync_ReturnsCorrectDomainUserDTO()
    {
        // Arrange
        var user = new User { Id = "user1", UserName = "user1@ms.com", FirstName = "A", LastName = "One", Email = "user1@ms.com", PhoneNumber = "123", Address = "Addr", IsActive = true };
        var domainUser = new DomainUser { Id = "domain1", UserId = "user1", User = user };
        DbContext!.Users.Add(user);
        DbContext.DomainUsers.Add(domainUser);
        await DbContext.SaveChangesAsync();

        var store = new Mock<IUserStore<User>>();
        var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        var mockLogger = new Mock<ILogger<MakerSchedule.Application.Services.DomainUserService>>();
        var mockMapper = new Mock<IMapper>();
        var service = new MakerSchedule.Application.Services.DomainUserService(
            DbContext,
            mockLogger.Object,
            mockUserManager.Object,
            mockMapper.Object);

        // Act
        var result = await service.GetDomainUserByIdAsync("domain1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("domain1", result.Id);
        Assert.Equal("user1", result.UserId);
        Assert.Equal("user1@ms.com", result.Email);
        Assert.Equal("A", result.FirstName);
        Assert.Equal("One", result.LastName);
        Assert.Equal("123", result.PhoneNumber);
        Assert.Equal("Addr", result.Address);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task GetDomainUserByIdAsync_ThrowsNotFoundException_WhenNotFound()
    {
        // Arrange
        var store = new Mock<IUserStore<User>>();
        var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        var mockLogger = new Mock<ILogger<MakerSchedule.Application.Services.DomainUserService>>();
        var mockMapper = new Mock<IMapper>();
        var service = new MakerSchedule.Application.Services.DomainUserService(
            DbContext!,
            mockLogger.Object,
            mockUserManager.Object,
            mockMapper.Object);

        // Act & Assert
        await Assert.ThrowsAsync<MakerSchedule.Application.Exceptions.NotFoundException>(async () =>
            await service.GetDomainUserByIdAsync("doesnotexist"));
    }

    [Fact]
    public async Task DeleteDomainUserByIdAsync_DeletesDomainUserAndUser()
    {
        // Arrange
        var user = new User { Id = "user1", UserName = "user1@ms.com", FirstName = "A", LastName = "One" };
        var domainUser = new DomainUser { Id = "domain1", UserId = "user1", User = user };
        DbContext!.Users.Add(user);
        DbContext.DomainUsers.Add(domainUser);
        await DbContext.SaveChangesAsync();

        var store = new Mock<IUserStore<User>>();
        var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        mockUserManager.Setup(m => m.FindByIdAsync("user1")).ReturnsAsync(user);
        mockUserManager.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);
        var mockLogger = new Mock<ILogger<MakerSchedule.Application.Services.DomainUserService>>();
        var mockMapper = new Mock<IMapper>();
        var service = new MakerSchedule.Application.Services.DomainUserService(
            DbContext,
            mockLogger.Object,
            mockUserManager.Object,
            mockMapper.Object);

        // Act
        await service.DeleteDomainUserByIdAsync("domain1");

        // Assert
        var domainUserInDb = await DbContext.DomainUsers.FindAsync("domain1");
        Assert.Null(domainUserInDb);
        // User deletion is handled by UserManager, so we only check domain user removal here
    }

    [Fact]
    public async Task DeleteDomainUserByIdAsync_ThrowsNotFoundException_WhenDomainUserNotFound()
    {
        // Arrange
        var store = new Mock<IUserStore<User>>();
        var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        var mockLogger = new Mock<ILogger<MakerSchedule.Application.Services.DomainUserService>>();
        var mockMapper = new Mock<IMapper>();
        var service = new MakerSchedule.Application.Services.DomainUserService(
            DbContext!,
            mockLogger.Object,
            mockUserManager.Object,
            mockMapper.Object);

        // Act & Assert
        await Assert.ThrowsAsync<MakerSchedule.Application.Exceptions.NotFoundException>(async () =>
            await service.DeleteDomainUserByIdAsync("doesnotexist"));
    }

    [Fact]
    public async Task DeleteDomainUserByIdAsync_ThrowsNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var user = new User { Id = "user1", UserName = "user1@ms.com" };
        var domainUser = new DomainUser { Id = "domain1", UserId = "user1", User = user };
        DbContext!.Users.Add(user);
        DbContext.DomainUsers.Add(domainUser);
        await DbContext.SaveChangesAsync();

        var store = new Mock<IUserStore<User>>();
        var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        mockUserManager.Setup(m => m.FindByIdAsync("user1")).ReturnsAsync((User?)null);
        var mockLogger = new Mock<ILogger<MakerSchedule.Application.Services.DomainUserService>>();
        var mockMapper = new Mock<IMapper>();
        var service = new MakerSchedule.Application.Services.DomainUserService(
            DbContext!,
            mockLogger.Object,
            mockUserManager.Object,
            mockMapper.Object);

        // Act & Assert
        await Assert.ThrowsAsync<MakerSchedule.Application.Exceptions.NotFoundException>(async () =>
            await service.DeleteDomainUserByIdAsync("domain1"));
    }

    [Fact]
    public async Task DeleteDomainUserByIdAsync_ThrowsBaseException_WhenUserDeleteFails()
    {
        // Arrange
        var user = new User { Id = "user1", UserName = "user1@ms.com" };
        var domainUser = new DomainUser { Id = "domain1", UserId = "user1", User = user };
        DbContext!.Users.Add(user);
        DbContext.DomainUsers.Add(domainUser);
        await DbContext.SaveChangesAsync();

        var store = new Mock<IUserStore<User>>();
        var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        mockUserManager.Setup(m => m.FindByIdAsync("user1")).ReturnsAsync(user);
        mockUserManager.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "fail" }));
        var mockLogger = new Mock<ILogger<MakerSchedule.Application.Services.DomainUserService>>();
        var mockMapper = new Mock<IMapper>();
        var service = new MakerSchedule.Application.Services.DomainUserService(
            DbContext!,
            mockLogger.Object,
            mockUserManager.Object,
            mockMapper.Object);

        // Act & Assert
        await Assert.ThrowsAsync<MakerSchedule.Application.Exceptions.BaseException>(async () =>
            await service.DeleteDomainUserByIdAsync("domain1"));
    }


} 