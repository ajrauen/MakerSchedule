using System;
using System.Collections.Generic;

using MakerSchedule.Domain.Entities;
using MakerSchedule.Domain.Enums;

namespace MakerSchedule.Infrastructure.Data
{
    public static class SeedData
    {
        public static List<User> SeedUsers => new List<User>
        {
            new User
            {
                Id = "11111111-1111-1111-1111-111111111111",
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                CreatedAt = new DateTime(2025, 6, 17, 23, 45, 49, 86, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 6, 17, 23, 45, 49, 86, DateTimeKind.Utc),
                IsActive = true,
                Email = "john.doe@example.com",
                UserName = "john.doe@example.com",
                UserType = UserType.Employee,
                EmailConfirmed = true,
                PhoneNumber = "123-456-7890",
                PhoneNumberConfirmed = true,
                SecurityStamp = "5191f6eb-ce10-4825-8ff9-1edb55809163",
                ConcurrencyStamp = "31372edb-255c-4acc-869a-3a3aa8dc0ab7"
            },
            new User
            {
                Id = "22222222-2222-2222-2222-222222222222",
                FirstName = "Jane",
                LastName = "Smith",
                Address = "456 Oak Ave",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                Email = "jane.smith@example.com",
                UserName = "jane.smith@example.com",
                UserType = UserType.Employee,
                EmailConfirmed = true,
                PhoneNumber = "555-123-4567",
                PhoneNumberConfirmed = true,
                SecurityStamp = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
                ConcurrencyStamp = "b2c3d4e5-f678-90ab-cdef-1234567890ab"
            },
            new User
            {
                Id = "33333333-3333-3333-3333-333333333333",
                FirstName = "Alice",
                LastName = "Johnson",
                Address = "789 Pine Rd",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                Email = "alice.johnson@example.com",
                UserName = "alice.johnson@example.com",
                UserType = UserType.Customer,
                EmailConfirmed = true,
                PhoneNumber = "444-555-6666",
                PhoneNumberConfirmed = true,
                SecurityStamp = "c3d4e5f6-7890-abcd-ef12-34567890abcd",
                ConcurrencyStamp = "d4e5f678-90ab-cdef-1234-567890abcdef"
            },
            new User
            {
                Id = "44444444-4444-4444-4444-444444444444",
                FirstName = "Bob",
                LastName = "Williams",
                Address = "321 Maple St",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                Email = "bob.williams@example.com",
                UserName = "bob.williams@example.com",
                UserType = UserType.Customer,
                EmailConfirmed = true,
                PhoneNumber = "777-888-9999",
                PhoneNumberConfirmed = true,
                SecurityStamp = "e5f67890-abcd-ef12-3456-7890abcdef12",
                ConcurrencyStamp = "f67890ab-cdef-1234-5678-90abcdef1234"
            }
        };

        public static List<Employee> SeedEmployees => new List<Employee>
        {
            new Employee
            {
                Id = 1,
                UserId = "11111111-1111-1111-1111-111111111111",
                EmployeeNumber = "EMP001",
                Department = "HR",
                Position = "Manager",
                HireDate = new DateTime(2020, 1, 15)
            },
            new Employee
            {
                Id = 2,
                UserId = "22222222-2222-2222-2222-222222222222",
                EmployeeNumber = "EMP002",
                Department = "IT",
                Position = "Developer",
                HireDate = new DateTime(2021, 5, 10)
            }
        };

        public static List<Customer> SeedCustomers => new List<Customer>
        {
            new Customer
            {
                Id = 1,
                UserId = "33333333-3333-3333-3333-333333333333",
                CustomerNumber = "CUST001",
                PreferredContactMethod = "Email",
                Notes = "VIP customer"
            },
            new Customer
            {
                Id = 2,
                UserId = "44444444-4444-4444-4444-444444444444",
                CustomerNumber = "CUST002",
                PreferredContactMethod = "Phone",
                Notes = "Frequent buyer"
            }
        };
    }
}
