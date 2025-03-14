namespace EmployeeManagement.Api.Interfaces;

public interface IDbContext{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}