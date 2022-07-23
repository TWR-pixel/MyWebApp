using MyWebApp.Data.Entities;

namespace MyWebApp.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    public ValueTask<IList<TEntity>> GetAllAsync();
    public ValueTask<TEntity> GetByIdAsync(ulong id);
    public ValueTask<IList<TEntity>> Take(int count);
    public ValueTask<TEntity> CreateAsync(TEntity entity);
    public ValueTask DeleteByIdAsync(ulong id);
}