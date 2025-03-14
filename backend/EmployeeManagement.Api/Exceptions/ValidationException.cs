namespace EmployeeManagement.Api.Exceptions;

public sealed class ValidationException : Exception
{
  public ValidationException(IEnumerable<ValidationError> errors)
  {
    Errors = errors;
  }

  public IEnumerable<ValidationError> Errors { get; }
}

public sealed class ApiException : Exception
{
  public ApiException(string message) : base(message)
  { }
}