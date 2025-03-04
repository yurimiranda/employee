using Employee.Domain.Enums;

namespace Employee.Domain.Models;

public class UserContextModel
{
    public Guid Id { get; set; }
    public Role Role { get; set; }
    public string Username { get; set; }
}