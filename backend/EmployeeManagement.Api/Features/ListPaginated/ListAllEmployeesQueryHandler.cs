using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Domain.Models;

namespace EmployeeManagement.Api.Features.ListPaginated;

public class ListAllEmployeesQueryHandler : IQueryHandler<ListAllEmployeesQuery, PaginatedCollection<Employee>>{
    private readonly IEmployeeRepository _repository;

    public ListAllEmployeesQueryHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<PaginatedCollection<Employee>>> Handle(ListAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        (int Total, IEnumerable<Employee> Items) =
            await _repository.GetPaginatedAsync(
                                request.nameOrDocNumber,
                                request.Page,
                                request.Limit,
                                cancellationToken);
                                
        return new PaginatedCollection<Employee>(request.Page, request.Limit, Total, Items);
    }
}