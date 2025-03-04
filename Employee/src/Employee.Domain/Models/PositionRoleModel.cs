using Employee.Domain.Abstractions;

namespace Employee.Domain.Models;

public class PositionRoleModel : Entity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }

    public PositionRoleModel(int id)
    {
        Id = id;
    }
}