namespace EmployeeManagement.Api.Exceptions;
public sealed record ValidationError(string PropertyName, string ErrorMessage);
