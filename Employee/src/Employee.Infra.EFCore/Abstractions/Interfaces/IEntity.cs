namespace Employee.Infra.EFCore.Abstractions.Interfaces;

public interface IEntity
{
    public int Id { get; set; }
    public bool Active { get; set; }
}