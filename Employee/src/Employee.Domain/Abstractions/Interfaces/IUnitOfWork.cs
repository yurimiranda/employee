namespace Employee.Domain.Abstractions.Interfaces;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken = default);
}