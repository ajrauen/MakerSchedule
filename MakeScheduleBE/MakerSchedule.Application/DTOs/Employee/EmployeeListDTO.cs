using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Application.DTOs.Employee
{
    public class EmployeeListDTO
    {
        public int Id { get; set; }
        public required string EmployeeID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
