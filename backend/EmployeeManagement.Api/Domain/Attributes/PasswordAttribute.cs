using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EmployeeManagement.Api.Domain.Attributes;

public class PasswordAttribute : ValidationAttribute
{
    private const int MinimumLength = 8;
    
    public PasswordAttribute() 
    {
        this.ErrorMessage = "Password must be at least 8 characters long, contain an uppercase letter, a lowercase letter, a number, and a special character.";
    }

    public override bool IsValid(object value)
    {
        if (value == null)
            return false;

        var password = value.ToString();

        // Validate minimum length
        if (password.Length < MinimumLength)
            return false;

        // Validate uppercase letter
        if (!Regex.IsMatch(password, @"[A-Z]"))
            return false;

        // Validate lowercase letter
        if (!Regex.IsMatch(password, @"[a-z]"))
            return false;

        // Validate number
        if (!Regex.IsMatch(password, @"\d"))
            return false;

        // Validate special character
        if (!Regex.IsMatch(password, @"[\W_]"))
            return false;

        // Optional: You can also add additional validation, like not allowing common passwords.
        return true;
    }
}
