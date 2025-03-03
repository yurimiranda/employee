using Employee.Domain.Abstractions;

namespace Employee.Domain.Models;

public class Phone : Entity<int>
{
    public string AreaCode { get; set; }
    public string Number { get; set; }
    public bool IsPrimary { get; set; }
}