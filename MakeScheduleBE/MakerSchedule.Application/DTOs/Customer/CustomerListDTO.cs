namespace MakerSchedule.Application.DTOs.Customer;

public class CustomerListDTO
{
    public int Id { get; set; }
    public required string CustomerID { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
