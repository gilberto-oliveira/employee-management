using System.Diagnostics.CodeAnalysis;
using EmployeeManagement.Api.Abstractions;

namespace EmployeeManagement.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class ValidatedExtension
{
    public static IDictionary<string, string[]> ToProblem<T>(this Result<T> result)
    {
        if (result.IsSuccess) return new Dictionary<string, string[]>();
        return new Dictionary<string, string[]> {
            { result.Error!.Code, new[] {result.Error.Name}}
        };
    }

    public static IDictionary<string, string[]> ToProblem(this Result result)
    {
        if (result.IsSuccess) return new Dictionary<string, string[]>();
        return new Dictionary<string, string[]> {
            { result.Error!.Code, new[] {result.Error.Name}}
        };
    }
}
