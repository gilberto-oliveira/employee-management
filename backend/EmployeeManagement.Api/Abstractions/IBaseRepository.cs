namespace EmployeeManagement.Api.Abstractions;

public interface IBaseRepository<TEntity> where TEntity : class
{
    ValueTask<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(TEntity item);
    void Update(TEntity item);
    void Delete(TEntity item);
}