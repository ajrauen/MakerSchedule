using MakerSchedule.Application.DTOs.Customer;

namespace MakerSchedule.Application.Interfaces;

public interface ICustomerProfileService
{
    Task<int> CreateCustomerAsync(CreateCustomerDTO dto);
    Task<bool> UpdateCustomerProfileAsync(int userId, UpdateCustomerProfileDTO dto);
}