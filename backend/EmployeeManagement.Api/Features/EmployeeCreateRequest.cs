using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Features.Create;

namespace EmployeeManagement.Api.Features;

public sealed record EmployeeCreateRequest(
    string firstName, 
    string lastName, 
    string docNumber, 
    string email, 
    List<string> phones, 
    string password, 
    string role,
    DateTime dateOfBirth,
    Guid managerId
) : ICommand<EmployeeCreateCommand>
{
    public EmployeeCreateCommand ToCommand()
    {
        return new(firstName, lastName, docNumber, email, phones, password, role, dateOfBirth, managerId);
    }
}
