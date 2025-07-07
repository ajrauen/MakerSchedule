using AutoMapper;

using MakerSchedule.Application.DTOs.Customer;
using MakerSchedule.Application.DTOs.Employee;
using MakerSchedule.Application.Exceptions;
using MakerSchedule.Domain.Entities;
using MakerSchedule.Infrastructure.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MakerSchedule.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomerService> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public CustomerService(
        ApplicationDbContext context,
        ILogger<CustomerService> logger,
        UserManager<User> userManager,
        IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersWithDetailsAsync()
    {
        try
        {
            return await _context.Customers.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching customers");
            throw new BaseException("Failed to fetch customers", "FETCH_ERROR", 500, ex);
        }
    }


    public async Task<IEnumerable<CustomerListDTO>> GetAllCustomersAsync()
    {
        try
        {
            var customers = await _context.Customers
                            .Include(e => e.User)
                            .ToListAsync();

            var customerDTOs = customers.Select(customer => new CustomerListDTO
            {
                Id = customer.Id,
                CustomerID = customer.CustomerNumber,
                FirstName = customer.User?.FirstName ?? string.Empty,
                LastName = customer.User?.LastName ?? string.Empty,

            }).ToList();


            return customerDTOs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetch customer Ids");
            throw new BaseException("Failed to fetch customer IDs", "FETCH_ERROR", 500, ex);
        }
    }

    public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
    {
        try
        {
            var customer = await _context.Customers
                .Include(e => e.User)
                .Include(e => e.EventsAttended)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (customer == null)
            {
                throw new NotFoundException("Customer", id);
            }
            return new CustomerDTO
            {
                Id = customer.Id,

                UserId = customer.UserId,
                Email = customer.User?.Email ?? string.Empty,
                FirstName = customer.User?.FirstName ?? string.Empty,
                LastName = customer.User?.LastName ?? string.Empty,
                PhoneNumber = customer.User?.PhoneNumber ?? string.Empty,
                Address = customer.User?.Address ?? string.Empty,
                IsActive = customer.User?.IsActive ?? false,
                EventsAttended = customer.EventsAttended.Select(e => new EventSummaryDTO
                {
                    EventName = e.EventName,
                    Id = e.Id,
                }).ToList(),
            };
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching customer by id: {Id}", id);
            throw new BaseException("Failed to fetch customer", "FETCH_ERROR", 500, ex);
        }
    }

    public async Task DeleteCustomerByIdAsync(int id)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Id == id);
        if (customer == null)
        {
            throw new NotFoundException("Customer", id); 
        }

        var user = await _userManager.FindByIdAsync(customer.UserId);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found.", customer.UserId);
            throw new NotFoundException("User not found", customer.UserId); 
        }

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                _context.Customers.Remove(customer);
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    throw new BaseException(
                                message: $"Failed to delete user '{user.Id}'.",
                                errorCode: "USER_DELETION_FAILED",
                                statusCode: 500 // Or another appropriate status code like 400 Bad Request
                            );
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
