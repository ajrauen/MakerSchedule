using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MakerSchedule.Application.DTOs.Employee
{
    public class EmployeeListDTO
    {
        public int Id { get; set; }
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
