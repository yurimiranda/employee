namespace Employee.Infra.EFCore.Abstractions.Interfaces;

public interface IUnitOfWork
{
    Task Save(CancellationToken cancellationToken = default);
}