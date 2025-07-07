using System.ComponentModel.DataAnnotations;
using MakerSchedule.Domain.Entities;

namespace MakerSchedule.Application.DTOs.Employee;

public class CreateEmployeeDTO
{
    // User fields (required for account creation)
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    
    // Employee-specific fields
    public string EmployeeNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
} 