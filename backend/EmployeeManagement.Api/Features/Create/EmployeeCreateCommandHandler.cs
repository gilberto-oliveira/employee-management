using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Domain.Models;
using EmployeeManagement.Api.Interfaces;

namespace EmployeeManagement.Api.Features.Create;

public class EmployeeCreateCommandHandler : ICommandHandler<EmployeeCreateCommand, Guid>
{
    private readonly IEmployeeRepository _repository;
    private readonly IDbContext _dbContext;

    public EmployeeCreateCommandHandler(IEmployeeRepository repository, IDbContext dbContext)
    {
        _repository = repository;
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> Handle(EmployeeCreateCommand request, CancellationToken cancellationToken)
    {
        bool exists = await _repository.ExistsByDocAsync(request.docNumber, cancellationToken);
        if (exists)
            return Result.Failure<Guid>("Employee with this docNumber already exists", "Employee");

        var employee = Employee.Create(request.firstName, request.lastName, 
            request.docNumber, request.email, request.phones, request.password, request.role);

        _repository.Add(employee);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return employee.Id;
    }
}