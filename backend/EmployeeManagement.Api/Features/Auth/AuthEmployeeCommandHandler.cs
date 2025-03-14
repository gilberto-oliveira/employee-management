
using EmployeeManagement.Api.Abstractions;

namespace EmployeeManagement.Api.Features.Auth;

internal sealed class AuthEmployeeCommandHandler : ICommandHandler<AuthEmployeeCommand, AccessTokenResponse>
{
    private readonly IAuthenticationService _authService;

    public AuthEmployeeCommandHandler(IAuthenticationService authService)
    {
        _authService = authService;
    }

    public async Task<Result<AccessTokenResponse>> Handle(
        AuthEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(
            request.DocNumber,
            request.Password,
            cancellationToken);

        return result.IsFailure
            ? Result.Failure<AccessTokenResponse>(result.Error!)
            : (Result<AccessTokenResponse>)new AccessTokenResponse(result.Data);
    }
}