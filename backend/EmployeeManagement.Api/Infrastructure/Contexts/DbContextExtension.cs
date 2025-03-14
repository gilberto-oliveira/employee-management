using System.Diagnostics.CodeAnalysis;
using EmployeeManagement.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Api.Infrastructure.Contexts;

[ExcludeFromCodeCoverage]
public static class DbContextExtension
{
    public static DbSet<TEntity> GetDbSet<TEntity>(this IDbContext dbContext)
        where TEntity : class
    {
        var dfqXApiDbContext = (EmployeeManagementDbContext)dbContext;
        return dfqXApiDbContext.Set<TEntity>();
    }
}