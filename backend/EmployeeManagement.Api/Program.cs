using System.Reflection;
using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Infrastructure.Contexts;
using EmployeeManagement.Api.Infrastructure.Repositories;
using EmployeeManagement.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine(connectionString);

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Connection string is missing");

builder.Services.AddDbContext<IDbContext, EmployeeManagementDbContext>(options =>
{
    options.UseSqlServer(connectionString, b => 
        b.MigrationsAssembly(typeof(EmployeeManagementDbContext).Assembly.FullName));
});

builder.Services.AddAuthentication();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/");

app.UseAuthentication();

app.MapGroup("/api/v1/employee")
    .MapGet("", () => Results.Ok(""))
.WithTags("employees");

app.Run();