using Employee.Domain.Abstractions;
using Employee.Domain.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Employee.Infra.EFCore.Abstractions;

public abstract class Repository<TContext, TEntity, TKey>(TContext context)
    : IRepository<TEntity, TKey>
    where TKey : struct, IEquatable<TKey>
    where TContext : DbContext
    where TEntity : Entity<TKey>
{
    protected readonly TContext Context = context;

    public Task Delete(TEntity entity, CancellationToken cancellationToken = default)
    {
        var dbSet = Context.Set<TEntity>();

        entity.Active = false;
        dbSet.Update(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAll(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var dbSet = Context.Set<TEntity>();

        var deletedList = entities.ToList();
        deletedList.ForEach(model =>
        {
            model.Active = false;
        });

        dbSet.UpdateRange(deletedList);

        return Task.CompletedTask;
    }

    public async Task<bool> Exists(TKey id, CancellationToken cancellationToken = default)
    {
        var dbSet = Context.Set<TEntity>();
        return await dbSet.AnyAsync(m => m.Id.Equals(id), cancellationToken);
    }

    public async Task<TEntity> Get(TKey id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>()
            .Where(d => d.Id.Equals(id))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> Get(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>()
            .ToListAsync(cancellationToken);
    }

    public async Task<TEntity> Insert(TEntity entity, bool save = false, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);

        if (save)
            await Context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<IEnumerable<TEntity>> InsertAll(IEnumerable<TEntity> entities,  bool save = false, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        if (save)
            await Context.SaveChangesAsync(cancellationToken);

        return entities;
    }

    public async Task<TEntity> Update(TEntity entity, bool save = false, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().Update(entity);

        if (save)
            await Context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public Task<IEnumerable<TEntity>> UpdateAll(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().UpdateRange(entities);
        return Task.FromResult(entities);
    }
}