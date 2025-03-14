using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagement.Api.Features.Patch;

public record EmployeePatchCommand(Guid Id, JsonPatchDocument<Employee> Patch) : ICommand<Unit>;
