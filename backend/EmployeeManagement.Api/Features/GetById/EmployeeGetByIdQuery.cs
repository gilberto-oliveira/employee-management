using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Models;

namespace EmployeeManagement.Api.Features.GetById;

public record EmployeeGetByIdQuery(Guid Id) : IQuery<Employee>;
