namespace MakerSchedule.Application.DTO.DomainUser;

public class DomainUserDTO
{
    // Employee fields
    public Guid Id { get; set; } =  Guid.NewGuid();
    public Guid UserId { get; init; } =  Guid.NewGuid();

    // User fields
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsActive { get; set; }

}
