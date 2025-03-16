using System.Reflection;
using System.Text;
using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Extensions;
using EmployeeManagement.Api.Features;
using EmployeeManagement.Api.Infrastructure.Authentication;
using EmployeeManagement.Api.Infrastructure.Contexts;
using EmployeeManagement.Api.Infrastructure.Repositories;
using EmployeeManagement.Api.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using EmployeeManagement.Api.Behaviors;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEmployeeManagementSwagger();
builder.Services.AddProblemDetails();

builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

const string APIVersion = "/api/v1";

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Connection string is missing");

builder.Services.AddDbContext<IDbContext, EmployeeManagementDbContext>(options =>
{
    options.UseSqlServer(connectionString, b =>
        b.MigrationsAssembly(typeof(EmployeeManagementDbContext).Assembly.FullName));
});

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services
.AddHttpContextAccessor()
.AddAuthorization()
.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "employee-management-issuer",
        ValidAudience = "employee-management-audience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtService.SECRETE))
    };
});

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddCors();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeManagementDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


app.UseHealthChecks("/");
app.UseAuthentication();
app.UseAuthorization();

app.UseErrorHandling();

app.MapEmployees(APIVersion);
app.MapWhen(context => context.Request.Path == "/", app => app.Run(context =>
{
    var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
    return context.Response.WriteAsync($"{assemblyName} is up and running!");
}));

app.Run();