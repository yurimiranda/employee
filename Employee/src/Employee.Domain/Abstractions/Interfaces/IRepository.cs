namespace Employee.Domain.Abstractions.Interfaces;

public interface IRepository<TEntity, TKey> where TEntity : Entity<TKey> where TKey : struct, IEquatable<TKey>
{
    Task<TEntity> Insert(TEntity entity, bool save = false, CancellationToken cancellationToken = default);
    Task<TEntity> Update(TEntity entity, bool save = false, CancellationToken cancellationToken = default);
    Task Delete(TEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> InsertAll(IEnumerable<TEntity> entities,  bool save = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> UpdateAll(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task DeleteAll(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task<bool> Exists(TKey id, CancellationToken cancellationToken = default);
    Task<TEntity> Get(TKey id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> Get(CancellationToken cancellationToken = default);
}