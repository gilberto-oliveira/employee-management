using EmployeeManagement.Api.Abstractions;
using EmployeeManagement.Api.Infrastructure.Contexts;
using EmployeeManagement.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Api.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity>(IDbContext dbContext) : IBaseRepository<TEntity>
    where TEntity : class
{
    protected DbSet<TEntity> DbSet = dbContext.GetDbSet<TEntity>();

    public virtual ValueTask<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => DbSet.FindAsync(id, cancellationToken);
    public void Add(TEntity item) => DbSet.Add(item);
    public virtual void Delete(TEntity item) => DbSet.Remove(item);
    public void Update(TEntity item) => DbSet.Update(item);
}
