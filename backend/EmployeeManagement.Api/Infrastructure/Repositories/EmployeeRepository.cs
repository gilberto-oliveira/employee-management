using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Models;
using EmployeeManagement.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Api.Infrastructure.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(IDbContext dbContext) : base(dbContext)
    { }

    public Task<bool> ExistsByDocAsync(string docNumber, CancellationToken cancellationToken)
    {
        return DbSet.AnyAsync(e => e.DocNumber == docNumber, cancellationToken);
    }

    public Task<Employee?> GetByDocNumberAndPasswordAsync(string docNumber, string password, CancellationToken cancellationToken)
    {
         return DbSet
            .Where(u => u.DocNumber == docNumber && u.Password == password)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(int Total, IEnumerable<Employee> Items)> GetPaginatedAsync(string? nameOrDocNumber, int page, int limit, CancellationToken cancellationToken)
    {
        var query = DbSet.Where(e => nameOrDocNumber == null ||
                                    (e.FirstName.Contains(nameOrDocNumber) ||
                                    e.LastName.Contains(nameOrDocNumber) ||
                                    e.DocNumber.Contains(nameOrDocNumber))
                                );

        var total = await query.CountAsync(cancellationToken);
        page = page <= 0 ? 1 : page;
        limit = limit < 0 ? 10 : limit;

        var items = await query
                            .OrderByDescending(e => e.Id)
                            .Skip((page - 1) * limit)
                            .Take(limit)
                            .ToListAsync(cancellationToken);

        return (total, items);
    }
}