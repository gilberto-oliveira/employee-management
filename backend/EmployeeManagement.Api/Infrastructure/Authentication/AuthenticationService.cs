using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Domain.Models;
using EmployeeManagement.Api.Interfaces;

namespace EmployeeManagement.Api.Infrastructure.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
  private readonly IDbContext _dbContext;
  private readonly IEmployeeRepository _employeeRepository;
  private readonly IJwtService _jwtService;

  public AuthenticationService(IDbContext dbContext, IEmployeeRepository employeeRepository, IJwtService jwtService)
  {
    _dbContext = dbContext;
    _employeeRepository = employeeRepository;
    _jwtService = jwtService;
  }

  public async Task<Result<string>> LoginAsync(string docNumber, string password, CancellationToken cancellationToken)
  {
    Employee? employee = await _employeeRepository.GetByDocNumberAndPasswordAsync(docNumber, password, cancellationToken);
    if (employee is null)
      return Result.Failure<string>("Employee not found!", "Employee.Login");

    return _jwtService.GenerateToken(employee);
  }

  public async Task<Result<string>> RegisterAsync(Employee employee, CancellationToken cancellationToken = default)
  {
    _employeeRepository.Add(employee);
    await _dbContext.SaveChangesAsync(cancellationToken);

    var token = _jwtService.GenerateToken(employee);
    return token;
  }
}
