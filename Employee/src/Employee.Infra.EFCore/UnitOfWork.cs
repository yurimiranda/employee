using Employee.Domain.Abstractions.Interfaces;

namespace Employee.Infra.EFCore;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task Commit(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}