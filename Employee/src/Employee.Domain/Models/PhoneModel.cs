using Employee.Domain.Abstractions;

namespace Employee.Domain.Models;

public class PhoneModel : Entity<int>
{
    public Guid EmployeeId { get; set; }
    public string AreaCode { get; set; }
    public string Number { get; set; }
    public bool IsPrimary { get; set; }

    public EmployeeModel Employee { get; set; }
}