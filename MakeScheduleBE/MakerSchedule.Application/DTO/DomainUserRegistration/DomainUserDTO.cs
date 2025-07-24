using MakerSchedule.Domain.ValueObjects;

namespace MakerSchedule.Application.DTO.DomainUser;

public class DomainUserDTO
{
    // Employee fields
    public Guid Id { get; set; } =  Guid.NewGuid();
    public Guid UserId { get; init; } =  Guid.NewGuid();

    // User fields
    public required Email Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required PhoneNumber PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required bool IsActive { get; set; }

}
