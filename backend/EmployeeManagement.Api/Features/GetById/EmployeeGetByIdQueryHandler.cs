using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Domain.Models;

namespace EmployeeManagement.Api.Features.GetById;

public class EmployeeGetByIdQueryHandler : IQueryHandler<EmployeeGetByIdQuery, Employee>
{
    private readonly IEmployeeRepository _repository;
    public EmployeeGetByIdQueryHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<Employee>> Handle(EmployeeGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id, cancellationToken);
    }
}
