using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Models;
using EmployeeManagement.Api.Extensions;
using EmployeeManagement.Api.Features.Auth;
using EmployeeManagement.Api.Features.GetById;
using EmployeeManagement.Api.Features.ListPaginated;
using EmployeeManagement.Api.Features.Patch;
using EmployeeManagement.Api.Features.Remove;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmployeeManagement.Api.Features;

public static class EmployeesEndpoints
{
    public static void MapEmployees(this IEndpointRouteBuilder endpoints, string apiVersion)
    {
        var group = endpoints.MapGroup($"{apiVersion}/employees");
        
        group
            .MapPost("/auth", AuthAsync)
            .WithName("AuthEmployee")
            .WithDescription("If the docNumber and password are allowed then it returns a Json Web Token (JWT)")
            .WithSummary("Return a JTW Token")
            .AllowAnonymous();

        group
            .MapGet("/", ListAllAsync)
            .WithName("ListAllEmployees")
            .WithDescription("Returns a list of employees paginated")
            .WithSummary("Returns a list of employees paginated")
            .RequireAuthorization();

        group
          .MapGet("/{id}", GetByIdAsync)
          .WithName("GetEmployee")
          .WithDescription($"Return a single employee")
          .WithSummary($"Return an employee");

        group
            .MapPost("/", PostAsync)
            .WithName("CreateEmployee")
            .WithDescription("Create a single employee")
            .WithSummary("Create new employee");

        group
           .MapPatch("/{id}", PatchAsync)
           .WithName("PatchEmployee")
           .WithDescription($"Patches a single existing employee")
           .WithSummary($"Patches one single employee");

        group
            .MapDelete("/{id}", DeleteByIdAsync)
            .WithName("DeleteEmployee")
            .WithDescription($"Remove a single employee")
            .WithSummary($"Remove an employee");

        group
            .WithTags("Employees")
            .WithOpenApi()
            .RequireAuthorization();
    }

    public static async Task<Results<Ok<AccessTokenResponse>, ValidationProblem>> AuthAsync(
        [FromServices] IMediator mediator,
        [FromBody] AuthEmployeeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var result = await mediator.Send(request.ToCommand(), cancellationToken);
        return result.IsFailure ?
            TypedResults.ValidationProblem(result.ToProblem()) :
            TypedResults.Ok(result.Data);
    }

    private static async Task<Results<Ok<PaginatedCollection<Employee>>, ValidationProblem, BadRequest<string>>> ListAllAsync(
        [FromServices] IMediator mediator,
        [FromServices] IHttpContextAccessor httpContext,
        [FromQuery] string? search = null,
        [FromQuery] int? page = null,
        [FromQuery] int? limit = null,
        CancellationToken cancellationToken = default
    )
    {
        if (httpContext.HttpContext is null || httpContext.HttpContext.User is null)
            return TypedResults.BadRequest("This is an invalid request!");

        var canFilter = httpContext.HttpContext.User.IsInRole("Admin");
        if (!canFilter)
        { search = null; }

        var result = await mediator.Send(new ListAllEmployeesQuery(search, page ?? 1, limit ?? 10), cancellationToken);
        if (result.IsFailure)
            return TypedResults.ValidationProblem(result.ToProblem());

        return TypedResults.Ok(result.Data);
    }


    public static async Task<Results<Ok<Employee>, BadRequest<string>, ValidationProblem>> GetByIdAsync(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id
    )
    {
        var query = new EmployeeGetByIdQuery(id);
        var result = await mediator.Send(query);
        if (result == null)
            return TypedResults.BadRequest("Invalid Id");
        return TypedResults.Ok(result.Data);
    }

    public static async Task<Results<ValidationProblem, BadRequest<string>, CreatedAtRoute>> PostAsync(
        [FromServices] IMediator mediator,
        HttpRequest httpRequest,
        [FromBody] EmployeeCreateRequest request
    )
    {
        var command = request.ToCommand();

        var result = await mediator.Send(command);
        if (result.IsFailure)
            return TypedResults.ValidationProblem(result.ToProblem());

        var id = result.Data;

        return TypedResults.CreatedAtRoute("GetEmployee", new { id });
    }
    public static async Task<Results<ValidationProblem, BadRequest<string>, NoContent>> PatchAsync(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id,
        [FromBody] List<Operation> operations
    )
    {
        var patchDocument = operations.ToJsonPatchDocument<Employee>();
        if (patchDocument is null)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> {
                 { "operations", ["Invalid patch document" ] }
                }
            );

        var command = new EmployeePatchCommand(id, patchDocument);
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return TypedResults.ValidationProblem(result.ToProblem());

        return TypedResults.NoContent();
    }

    public static async Task<Results<ValidationProblem, NoContent>> DeleteByIdAsync(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id
    )
    {
        var command = new EmployeeRemoveCommand(id);
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return TypedResults.ValidationProblem(result.ToProblem());

        return TypedResults.NoContent();
    }

    public static JsonPatchDocument<T>? ToJsonPatchDocument<T>(this IList<Operation> operations) where T : class
    {
        if (operations == null || operations.Count == 0) return default;

        var items = operations.Select(i => new
        {
            i.op,
            i.path,
            i.from,
            value = i.value?.ToString()
        }).ToList();
        var jsonOperations = JsonConvert.SerializeObject(items);
        return JsonConvert.DeserializeObject<JsonPatchDocument<T>>(jsonOperations);
    }
}