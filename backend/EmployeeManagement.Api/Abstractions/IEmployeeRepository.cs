using EmployeeManagement.Api.Domain.Models;

namespace EmployeeManagement.Api.Abstractions;

public interface IEmployeeRepository : IBaseRepository<Employee>
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
    Task<(int Total, IEnumerable<Employee> Items)> GetPaginatedAsync(string? nameOrDocNumber, int page, int limit, CancellationToken cancellationToken);
}