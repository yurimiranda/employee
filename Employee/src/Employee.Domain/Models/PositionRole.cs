using Employee.Domain.Abstractions;

namespace Employee.Domain.Models;

public class PositionRole : Entity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }

    public PositionRole(int id)
    {
        Id = id;
    }
}