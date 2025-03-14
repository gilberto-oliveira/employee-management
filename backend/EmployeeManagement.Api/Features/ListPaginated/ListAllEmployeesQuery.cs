using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Models;

namespace EmployeeManagement.Api.Features.ListPaginated;

public record ListAllEmployeesQuery(string? nameOrDocNumber, int Page, int Limit) : IQuery<PaginatedCollection<Employee>>;
