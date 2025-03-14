using EmployeeManagement.Api.Domain.Models;

namespace EmployeeManagement.Api.Abstractions;

public interface IJwtService
{
    Result<string> GenerateToken(Employee employee);
}
