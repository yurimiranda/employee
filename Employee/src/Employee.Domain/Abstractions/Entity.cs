using Employee.Domain.Abstractions.Interfaces;

namespace Employee.Domain.Abstractions;

public abstract class Entity<TKey> : IEntity<TKey> where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; set; }
    public bool Active { get; set; }
}