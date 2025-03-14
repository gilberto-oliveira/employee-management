using Microsoft.OpenApi.Models;

namespace EmployeeManagement.Api.Extensions;

public static class ApiSwaggerGeneration
{
    public static void AddEmployeeManagementSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Management API", Version = "v1" });

            // Configure Bearer authentication
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            };
            opt.AddSecurityDefinition("Bearer", securityScheme);
            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            };
            opt.AddSecurityRequirement(securityRequirement);
        });
    }
}