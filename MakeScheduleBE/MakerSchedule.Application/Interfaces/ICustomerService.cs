using MakerSchedule.Application.DTOs.Customer;
using MakerSchedule.Domain.Aggregates.Customer;

namespace MakerSchedule.Application.Services;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllCustomersWithDetailsAsync();
    Task<IEnumerable<CustomerListDTO>> GetAllCustomersAsync();
    Task<CustomerDTO> GetCustomerByIdAsync(int id);
    Task DeleteCustomerByIdAsync(int itd);
}
