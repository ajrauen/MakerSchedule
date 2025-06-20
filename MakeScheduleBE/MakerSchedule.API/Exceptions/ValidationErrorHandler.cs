using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MakerSchedule.API.Exceptions
{
    public static class ValidationErrorHandler
    {
        public static IActionResult HandleValidationErrors(ModelStateDictionary modelState)
        {
            var errors = ((IEnumerable<KeyValuePair<string, ModelStateEntry>>)modelState)
                .Select<KeyValuePair<string, ModelStateEntry>, object>(e =>
                {
                    // Handle the special "$" field which contains deserialization errors
                    if (e.Key == "$")
                    {
                        return new
                        {
                            Field = "request",
                            Errors = e.Value.Errors.Select(err =>
                                err.ErrorMessage.Replace("JSON deserialization for type 'MakerSchedule.Application.DTOs.Employee.CreateEmployeeDTOp' was missing required properties, including the following: ",
                                "Missing required properties: ")).ToList()
                        };
                    }

                    // Handle normal validation errors
                    return new
                    {
                        Field = e.Key,
                        Errors = e.Value?.Errors.Select(err => err.ErrorMessage).ToList()
                    };
                });

            return new BadRequestObjectResult(new
            {
                error = "Validation failed",
                code = "VALIDATION_ERROR",
                details = errors
            });
        }
    }
}
