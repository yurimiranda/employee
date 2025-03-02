namespace Employee.Infra.EFCore.Abstractions.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<TEntity> Insert(TEntity entity, bool save = false, CancellationToken cancellationToken = default);
    Task<TEntity> Update(TEntity entity, bool save = false, CancellationToken cancellationToken = default);
    Task Delete(TEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> InsertAll(IEnumerable<TEntity> entities,  bool save = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> UpdateAll(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task DeleteAll(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task<bool> Exists(int id, CancellationToken cancellationToken = default);
    Task<TEntity> Get(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> Get(CancellationToken cancellationToken = default);
}