
using EmployeeManagement.Api.Models;
using EmployeeManagement.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Api.Infrastructure.Contexts;

public class EmployeeManagementDbContext: DbContext, IDbContext
{
    public DbSet<Employee> Employees { get; set; } = null!;

    public EmployeeManagementDbContext(DbContextOptions<EmployeeManagementDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeManagementDbContext).Assembly);
    }
}