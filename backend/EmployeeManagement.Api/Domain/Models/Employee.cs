using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Api.Domain.Attributes;

namespace EmployeeManagement.Api.Domain.Models;

public class Employee
{
    public Guid Id { get; private set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string DocNumber { get; set; } = string.Empty;

    [Phone]
    public List<string> Phones { get; set; } = new List<string>();

    public Guid? ManagerId { get; set; }

    public Employee? Manager { get; set; }

    [Required]
    [Password]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [NotMinor]
    public DateTime DateOfBirth { get; set; }

    public string Role { get; set; } = string.Empty;

    public static Employee Create(string firstName, string lastName, string docNumber, string email, List<string> phones, string password, string role)
    {
        var employee = new Employee();
        employee.Id = Guid.NewGuid();
        employee.FirstName = firstName;
        employee.LastName = lastName;
        employee.DocNumber = docNumber;
        employee.Email = email;
        employee.DateOfBirth = DateTime.Now.AddYears(-20);
        employee.Phones = phones;
        employee.Password = password;
        employee.Role = role;
        return employee;
    }
}
