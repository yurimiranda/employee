using Employee.Domain.Abstractions.Interfaces;
using Employee.Domain.Abstractions;
using Moq;

namespace Employee.UnitTests.Extensions;

public static class RepositoriesExtensions
{
    public static void EnsuresNonPersistence<TRepository, TEntity, TKey>(this Mock<TRepository> repository,
        bool insert = true,
        bool intertAll = true,
        bool update = true,
        bool updateAll = true,
        bool delete = true,
        bool deleteAll = true)
        where TRepository : class, IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        if (insert)
            repository.Verify(x => x.Insert(It.IsAny<TEntity>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
        if (intertAll)
            repository.Verify(x => x.InsertAll(It.IsAny<IEnumerable<TEntity>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
        if (update)
            repository.Verify(x => x.Update(It.IsAny<TEntity>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
        if (updateAll)
            repository.Verify(x => x.UpdateAll(It.IsAny<IEnumerable<TEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        if (delete)
            repository.Verify(x => x.Delete(It.IsAny<TEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        if (deleteAll)
            repository.Verify(x => x.DeleteAll(It.IsAny<IEnumerable<TEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}