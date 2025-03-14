using EmployeeManagement.Api.Domain.Models;

namespace EmployeeManagement.Api.Abstractions;

public interface IAuthenticationService
{
  Task<Result<string>> LoginAsync(string username, string password, CancellationToken cancellationToken);
  Task<Result<string>> RegisterAsync(Employee employee, CancellationToken cancellationToken = default);
}
