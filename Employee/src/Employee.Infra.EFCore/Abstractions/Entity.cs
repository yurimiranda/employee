using Employee.Infra.EFCore.Abstractions.Interfaces;

namespace Employee.Infra.EFCore.Abstractions;

public abstract class Entity : IEntity
{
    public int Id { get; set; }
    public bool Active { get; set; }
}