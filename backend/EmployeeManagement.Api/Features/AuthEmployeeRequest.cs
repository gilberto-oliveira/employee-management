using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Features.Auth;

namespace EmployeeManagement.Api.Features;

public sealed record AuthEmployeeRequest(string DocNumber, string Password) : ICommand<AuthEmployeeCommand>
{
    public AuthEmployeeCommand ToCommand() => new(DocNumber, Password);
};
