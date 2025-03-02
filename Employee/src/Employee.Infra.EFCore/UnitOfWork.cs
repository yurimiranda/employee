using Employee.Infra.EFCore.Abstractions.Interfaces;

namespace Employee.Infra.EFCore;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task Save(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}