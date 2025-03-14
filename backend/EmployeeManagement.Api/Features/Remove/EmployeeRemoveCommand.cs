using EmployeeManagement.Api.Abstractions;
using MediatR;

namespace EmployeeManagement.Api.Features.Remove;

public record EmployeeRemoveCommand(Guid Id) : ICommand<Unit>;

