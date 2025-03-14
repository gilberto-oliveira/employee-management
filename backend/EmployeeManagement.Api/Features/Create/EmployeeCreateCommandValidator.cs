using FluentValidation;

namespace EmployeeManagement.Api.Features.Create;

public sealed class EmployeeCreateCommandValidator : AbstractValidator<EmployeeCreateCommand>
{
    public EmployeeCreateCommandValidator()
    {
        RuleFor(c => c.firstName).NotEmpty();
        RuleFor(c => c.lastName).NotEmpty().MinimumLength(5);
        RuleFor(c => c.docNumber).NotEmpty();
        RuleFor(c => c.email).EmailAddress().MinimumLength(5).NotEmpty();
        RuleFor(c => c.role).NotEmpty();
        RuleFor(c => c.password).NotEmpty().MinimumLength(8);
        RuleFor(c => c.dateOfBirth)
            .Must(dateOfBirth => dateOfBirth <= DateOnly.FromDateTime(DateTime.Now).AddYears(-18))
            .WithMessage("The employee must be at least 18 years old.");
    }
}
