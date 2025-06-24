using MakerSchedule.Application.DTOs.Employee;

namespace MakerSchedule.Application.DTOs.Customer;

public class CustomerDTO
{
    // Employee fields
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    // User fields
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<EventSummaryDTO> EventsAttended { get; set; } = [];

}
