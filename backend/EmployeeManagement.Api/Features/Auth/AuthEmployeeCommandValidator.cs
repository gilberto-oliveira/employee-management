using FluentValidation;

namespace EmployeeManagement.Api.Features.Auth;

public sealed class AuthEmployeeCommandValidator : AbstractValidator<AuthEmployeeCommand>
{
    public AuthEmployeeCommandValidator()
    {
        RuleFor(c => c.DocNumber).MinimumLength(10);
        RuleFor(c => c.Password).NotEmpty();
    }
}
