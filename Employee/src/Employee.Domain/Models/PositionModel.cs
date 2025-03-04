using Employee.Domain.Abstractions;

namespace Employee.Domain.Models;

public class PositionModel : Entity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public PositionModel(int id)
    {
        Id = id;
    }
}