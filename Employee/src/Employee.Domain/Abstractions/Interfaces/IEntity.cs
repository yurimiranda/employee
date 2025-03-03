namespace Employee.Domain.Abstractions.Interfaces;

public interface IEntity<TKey> where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; set; }
    public bool Active { get; set; }
}