namespace EmployeeManagement.Api.Models;

public class Employee
{
    public Guid Id { get; private set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string DocNumber { get; set; } = string.Empty;

    public List<string> Phones { get; set; } = new List<string>();

    public Guid? ManagerId { get; set; }

    public Employee? Manager { get; set; }

    public string Password { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Role { get; set; } = string.Empty;

    public static Employee Create(string firstName, string lastName, string docNumber, string email, List<string> phones, string password, string role, DateTime dateOfBirth, Guid? managerId = null)
    {
        var employee = new Employee();
        employee.Id = Guid.NewGuid();
        employee.FirstName = firstName;
        employee.LastName = lastName;
        employee.DocNumber = docNumber;
        employee.Email = email;
        employee.DateOfBirth = dateOfBirth;
        employee.Phones = phones;
        employee.Password = password;
        employee.Role = role;
        employee.ManagerId = managerId;
        return employee;
    }
}
