using EmployeeManagement.Api.Abstractions;

namespace EmployeeManagement.Api.Features.Auth;

public sealed record AuthEmployeeCommand(string DocNumber, string Password) : ICommand<AccessTokenResponse>;
