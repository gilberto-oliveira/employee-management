using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class ErrorHandlingExtension
{
    public static void UseErrorHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            AllowStatusCode404Response = true,
            ExceptionHandler = async (context) =>
            {
                var problemDetailsService = context.RequestServices.GetService<IProblemDetailsService>();
                if (problemDetailsService is null) return;

                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionHandlerFeature?.Error;
                if (exception == null)// || exception.GetType() != typeof(SearchException))
                    return;

                var problemDetails = GetProblemDetails(context, exception);
                context.Response.StatusCode = problemDetails.Status!.Value;

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        });
    }

    private static ProblemDetails GetProblemDetails(HttpContext context, Exception exception)
    {
        var hostingEnvironment = context.RequestServices.GetService<IHostEnvironment>();
        var canLogDetails = !hostingEnvironment?.IsProduction() ?? true;
        return exception switch
        {
            ValidationException validationException => new ProblemDetails
            {
                Type = "https://httpstatuses.com/400",
                Title = "Validation Error: Bad Request",
                Detail = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Instance = context.TraceIdentifier,
                Extensions =
                {
                    ["errors"] = validationException.Errors
                }
            },
            _ => new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = canLogDetails ? exception.Message
                : "An unexpected error occurred. Please try again later. If the problem persists, contact the system administrator.",
                Instance = context.TraceIdentifier
            }
        };
    }
}