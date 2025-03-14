using FluentValidation;

namespace EmployeeManagement.Api.Features.Create;

public sealed class EmployeeCreateCommandValidator : AbstractValidator<EmployeeCreateCommand>
{
    public EmployeeCreateCommandValidator()
    {
        RuleFor(c => c.firstName).NotEmpty();
        RuleFor(c => c.lastName).NotEmpty();
        RuleFor(c => c.docNumber).NotEmpty();
        RuleFor(c => c.phones).NotEmpty();
        RuleFor(c => c.email).EmailAddress().MinimumLength(5).NotEmpty();
        RuleFor(c => c.role).NotEmpty();

        RuleFor(c => c.dateOfBirth)
            .Must(dateOfBirth => dateOfBirth <= DateTime.Now.AddYears(-18))
            .WithMessage("The employee must be at least 18 years old.");
        
        RuleFor(c => c.password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(8, 16).WithMessage("Password must be between 8 and 16 characters long.");

        RuleFor(c => c.password)
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.");

        RuleFor(c => c.password)
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.");

        RuleFor(c => c.password)
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.");

        RuleFor(c => c.password)
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character (e.g., !@#$%^&*).");
    }
}
