using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Api.Domain.Attributes;

public class NotMinorAttribute : ValidationAttribute
{
    public NotMinorAttribute() 
        : base("Employee must be at least 18 years old.")
    { }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;

            // If the employee has not had their birthday yet this year, subtract 1 from the age.
            if (DateTime.Today < dateOfBirth.AddYears(age)) age--;

            // Check if the age is less than 18
            if (age < 18)
            {
                return new ValidationResult("Employee must be at least 18 years old.");
            }
        }
        return ValidationResult.Success;
    }
}
