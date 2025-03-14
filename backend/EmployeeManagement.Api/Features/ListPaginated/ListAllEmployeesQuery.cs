using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Domain.Models;

namespace EmployeeManagement.Api.Features.ListPaginated;

public record ListAllEmployeesQuery(string? nameOrDocNumber, int Page, int Limit) : IQuery<PaginatedCollection<Employee>>;
