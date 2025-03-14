using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Interfaces;
using MediatR;

namespace EmployeeManagement.Api.Features.Remove;

public class EmployeeRemoveCommandHandler : ICommandHandler<EmployeeRemoveCommand, Unit>
{
    private readonly IEmployeeRepository _repository;
    private readonly IDbContext _dbContext;

    public EmployeeRemoveCommandHandler(IEmployeeRepository repository, IDbContext dbContext)
    {
        _repository = repository;
        _dbContext = dbContext;
    }
    public async Task<Result<Unit>> Handle(EmployeeRemoveCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (employee is null)
            return Result.Success(Unit.Value);
        _repository.Delete(employee);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(Unit.Value);
    }
}

