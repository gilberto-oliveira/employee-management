
using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Exceptions;
using FluentValidation;
using MediatR;

namespace EmployeeManagement.Api.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
          IEnumerable<IValidator<TRequest>> validators,
          ILogger<ValidationBehavior<TRequest, TResponse>> logger, IServiceProvider serviceProvider)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {

        Validate(request);

        return await next();
    }

    private void Validate(TRequest request)
    {
        _logger.LogInformation("Validating request {Request}", request.GetType().Name);
        if (!_validators.Any())
        {
            _logger.LogWarning("No validators found for request {Request}", request.GetType().Name);
            return;
        }

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
            .Select(validator => validator.Validate(context))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .Select(vf => new ValidationError(vf.PropertyName, vf.ErrorMessage))
            .ToList();

        if (validationErrors.Any())
        {
            _logger.LogWarning("Validation failed for request {Request}", request.GetType().Name);
            throw new Exceptions.ValidationException(validationErrors);
        }
        _logger.LogInformation("Validation passed for request {Request}", request.GetType().Name);
    }
}