using EmployeeManagement.Api.Abstractions;

namespace EmployeeManagement.Api.Features.Create;

public record EmployeeCreateCommand(
    string firstName, 
    string lastName, 
    string docNumber, 
    string email, 
    List<string> phones, 
    string password, 
    string role,
    DateTime dateOfBirth) : ICommand<Guid>;
