using MakerSchedule.Application.DTOs.User;

namespace MakerSchedule.Application.DTOs.Employee;

public class UpdateEmployeeProfileDTO : UserProfileUpdateFields
{
    // Employee-Specific Properties
    public string? Department { get; set; }
    public string? Position { get; set; }
    public DateTime? HireDate { get; set; }
} 