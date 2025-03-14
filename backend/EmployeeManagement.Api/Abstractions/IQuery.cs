using MediatR;

namespace EmployeeManagement.Api.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> 
{
}
