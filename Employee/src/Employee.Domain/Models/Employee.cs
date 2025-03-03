using Employee.Domain.Abstractions;

namespace Employee.Domain.Models;

public class Employee : Entity<Guid>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Document { get; set; }
    public string ImmediateSupervisor { get; set; }
    public PositionRole Position { get; set; }
    public DateTime BirthDate { get; set; }
    public IEnumerable<Phone> Phones { get; set; }

    public bool OfLegalAge => BirthDate.AddYears(18) <= DateTime.Now;
}