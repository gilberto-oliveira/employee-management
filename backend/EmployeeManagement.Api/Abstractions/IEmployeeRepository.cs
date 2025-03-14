using EmployeeManagement.Api.Models;

namespace EmployeeManagement.Api.Abstractions;

public interface IEmployeeRepository : IBaseRepository<Employee>
{
    Task<bool> ExistsByDocAsync(string docNumber, CancellationToken cancellationToken);
    Task<(int Total, IEnumerable<Employee> Items)> GetPaginatedAsync(string? nameOrDocNumber, int page, int limit, CancellationToken cancellationToken);
    Task<Employee?> GetByDocNumberAndPasswordAsync(string docNumber, string password, CancellationToken cancellationToken);
}