using Employee.Domain.Abstractions;

namespace Employee.Domain.Models;

public class User : Entity<Guid>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public Employee Employee { get; set; }
}