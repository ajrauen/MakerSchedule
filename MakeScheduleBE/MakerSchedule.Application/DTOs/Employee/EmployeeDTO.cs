namespace MakerSchedule.Application.DTOs.Employee;
using MakerSchedule.Domain.Entities;

public class EmployeeDTO
{
    // Employee fields
    public int Id { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public string UserId { get; set; } = string.Empty;

    // User fields
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public List<EventSummaryDTO> EventsLed { get; set; } = [];
}
