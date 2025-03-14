using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Domain.Models;

namespace EmployeeManagement.Api.Features.GetById;

public record EmployeeGetByIdQuery(Guid Id) : IQuery<Employee>;
