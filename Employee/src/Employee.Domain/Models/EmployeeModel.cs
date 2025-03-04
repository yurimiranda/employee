using Employee.Domain.Abstractions;

namespace Employee.Domain.Models;

public class EmployeeModel : Entity<Guid>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Document { get; set; }
    public string ImmediateSupervisor { get; set; }
    public int PositionId { get; set; }
    public PositionModel Position { get; set; }
    public DateTime BirthDate { get; set; }
    public ICollection<PhoneModel> Phones { get; set; }

    public bool OfLegalAge => BirthDate.AddYears(18) <= DateTime.Now;
}