using MediatR;

namespace EmployeeManagement.Api.Abstractions;

public interface IBaseCommand { }
public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{
}
