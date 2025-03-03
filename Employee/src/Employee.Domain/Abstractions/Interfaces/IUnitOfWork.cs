namespace Employee.Domain.Abstractions.Interfaces;

public interface IUnitOfWork
{
    Task Save(CancellationToken cancellationToken = default);
}